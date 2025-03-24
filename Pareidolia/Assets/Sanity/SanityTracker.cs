using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using FMODUnity;

public class SanityTracker : MonoBehaviour
{
    public EventReference damageSound; // added this line so I could attach the sound to the sanity tracker prefab

    /// <summary>
    /// SanityTracker is a prefab that can be used to register the visibility of objects on the camera and impact sanity from there
    /// 
    /// The two public game object variables camera and stains are used to set the camera and specific objects to check LOS for
    /// 
    /// Additionally, public API function registerStain is available for use when a new stain is created
    /// 
    /// 
    /// </summary>
    public new Camera camera;

    //Active stains 
    //Note that this script is responsible for null checking accessed stains
    public List<GameObject> stains;

    //Sanity percentage
    private float sanity = 100;


    public int stainDamageGracePeriod = 3;
    public int stainDamageFreq = 15;

    private int garbageCollectionPeriod = 20;

    // post-processing variable below
    public Volume postProcessingVolume;
    private Vignette vignette;
    private FilmGrain filmGrain;
    private Coroutine filmGrainRoutine; // handle multiple overlapping sanity damage events
    private bool vignetteOnCooldown = false;


    class StainInfo
    {
        public bool active;
        public int damageCounter;

        public StainInfo(int startingDamageCounter)
        {
            active = false;
            damageCounter = startingDamageCounter;
        }
    };

    // stainInfo[i] corresponds to stain[i]
    private List<StainInfo> stainInfo = new List<StainInfo>();



    void Start()
    {
        for (int i = 0; i < stains.Count; i++)
        {
            stainInfo.Add(new StainInfo(stainDamageGracePeriod));
        }
        Debug.Log(stainInfo.ToString());

        // get the vignette effect from the Global Volume
        if (postProcessingVolume.profile.TryGet<Vignette>(out Vignette v))
        {
            vignette = v;
        }
        // same for grain effect
        if (postProcessingVolume.profile.TryGet<FilmGrain>(out FilmGrain fg))
        {
            filmGrain = fg;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(camera);

        for(int i = 0; i < stains.Count; i++)
        {       
                //Skip removed stains
                if (stains[i] == null)
                {
                    continue;
                }

                //Skip stains that are on cooldown
                if (stainInfo[i].active && stainInfo[i].damageCounter > 0)
                {
                    stainInfo[i].damageCounter--;
                    continue;
                }
                if (isObjectVisible(planes, stains[i]))
                {
                    //Stain that just became visible - enters grace period
                    if (!stainInfo[i].active)
                    {
                        stainInfo[i].active = true;
                        stainInfo[i].damageCounter = stainDamageGracePeriod;
                    } else
                    { //Stain has remained on screen long enough to do damage
                        onStainDamage(stains[i]);
                        stainInfo[i].damageCounter = stainDamageFreq;
                    }
                } else
                {
                    //Stain is inactive
                    stainInfo[i].active = false;
                }
        }
        garbageCollectionPeriod--;
        if(garbageCollectionPeriod == 0)
        {
            garbageCollectionPeriod = 20;
            removeDeletedStains();
        }
        if(sanity < 0)
        {
            onLoss();
        }

        if (vignette != null)
        {
            float normalizedSanity = Mathf.Clamp01(sanity / 100f); // map sanity to a range of 0 (low) to 1 (high)
            vignette.intensity.value = Mathf.Lerp(0.2f, 0.5f, 1 - normalizedSanity); // map normalized sanity to vignette intensity (0 to 0.45)
        }
    }

    private void removeDeletedStains()
    {
        List<StainInfo> stainInfoCopy = new List<StainInfo>();
        List<GameObject> stainCopy = new List<GameObject>();

        for (int i = 0; i < stains.Count; i++)
        {
            if (stains[i] == null) continue;
            stainInfoCopy.Add(stainInfo[i]);
            stainCopy.Add(stains[i]);
        }
        stainInfo = stainInfoCopy;
        stains = stainCopy;
    }

    private void onLoss()
    {
        Debug.Log("Game Over");
        // Let player respawn
        GameStateManager.Respawn();
    }

    private void onStainDamage(GameObject stain)
    {
        sanity--;

        AudioManager.instance.PlayOneShot(damageSound, this.transform.position); // Trigger damage sfx here

        // start or restart the Film Grain intensity animation
        if (filmGrain != null)
        {
            if (filmGrainRoutine != null)
            {
                StopCoroutine(filmGrainRoutine);
            }
            filmGrainRoutine = StartCoroutine(AnimateFilmGrainIntensity());
        }

        if (vignette != null && !vignetteOnCooldown)
        {
            StartCoroutine(AnimateVignetteIntensity());
        }
    }

    //Confirms if the object is visible. Note stain must have a Collider attached
    public bool isObjectVisible(Plane[] planes, GameObject go)
    {
        
        //Confirm that the object is in the field of view of the Camera object
        if(!GeometryUtility.TestPlanesAABB(planes, go.GetComponent<Collider>().bounds))
        {
            return false;
        }

        Vector3 toObject = (go.transform.position - camera.transform.position);
        float distance = toObject.magnitude;
        Vector3 direction = toObject / distance;

        //Cast a ray from camera - ensure clean line of sight to stain -> this is for center only at the moment.
        if (Physics.Raycast(camera.transform.position, direction, out RaycastHit hit, distance))
        {
            if(hit.collider.gameObject == go)
            {
                return true;
            }
        }


        return false;
    }

    //This function registers a stain with the collector
    public void registerStain(GameObject stain)
    {
        stains.Add(stain);
        stainInfo.Add(new StainInfo(stainDamageGracePeriod));
    }


    public float getSanity()
    {
        return sanity;
    }

    private IEnumerator AnimateFilmGrainIntensity()
    {
        float maxIntensity = 1f; // max intensity when sanity damage occurs
        float minIntensity = 0.2f; // min intensity after the effect
        float animationDuration = 1f; // duration of the animation
        float elapsedTime = 0f;

        // spike to max intensity
        filmGrain.intensity.value = maxIntensity;

        // gradually reduce intensity back to the minimum
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            filmGrain.intensity.value = Mathf.Lerp(maxIntensity, minIntensity, t);
            yield return null;
        }

        filmGrain.intensity.value = minIntensity;
    }

    private IEnumerator AnimateVignetteIntensity()
    {
        if (vignetteOnCooldown) yield break;
        vignetteOnCooldown = true;

        float originalIntensity = vignette.intensity.value; // current intensity
        float surgedIntensity = 0.4f; 
        float animationDuration = 2f; 
        float cooldownDuration = 2f;
        float elapsedTime = 0f;

        vignette.intensity.value = surgedIntensity;

        // fade back to the original intensity
        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationDuration;
            vignette.intensity.value = Mathf.Lerp(surgedIntensity, originalIntensity, t);
            yield return null;
        }

        vignette.intensity.value = originalIntensity;

        yield return new WaitForSeconds(cooldownDuration);
        vignetteOnCooldown = false; // cooldown
    }

}
