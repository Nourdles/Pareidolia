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

    public DeathManager DeathManager;

    public static bool damageEnabled = true;

    //Sanity percentage
    private float sanity;

    private int lastDamageStainIdx = -1;
    private Vector3 lastNormal;

    public int baseSanity = 30;
    private int stainDamageGracePeriod = 100;
    private int stainDamageFreq = 100;

    private int garbageCollectionPeriod = 20;

    // post-processing variable below
    public Volume postProcessingVolume;
    private Vignette vignette;
    private FilmGrain filmGrain;
    private Coroutine filmGrainRoutine; // handle multiple overlapping sanity damage events
    private bool vignetteOnCooldown = false;
    private DepthOfField depthOfField;


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
        sanity = baseSanity;
        for (int i = 0; i < stains.Count; i++)
        {
            stainInfo.Add(new StainInfo(stainDamageGracePeriod));
        }


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
        // same for blur
        if (postProcessingVolume.profile.TryGet<DepthOfField>(out DepthOfField dof))
        {
            depthOfField = dof;
            depthOfField.focusDistance.overrideState = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(camera);

        if (!SanityTracker.damageEnabled)
        {
            return;
        }

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
                        lastDamageStainIdx = i;
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
            float t = Mathf.Clamp01(1f - sanity / 30f); // sanity starts at 30
            float curveT = Mathf.SmoothStep(0f, 1f, t); // smoother ramp
            vignette.intensity.value = Mathf.Lerp(0.2f, 0.7f, curveT);
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

        //GameStateManager.Respawn();
        sanity = baseSanity;
        StartCoroutine(DeathManager.ProcessDeath(stains[lastDamageStainIdx], lastNormal));
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
                lastNormal = hit.normal;
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
        float grainMin = 0.1f;
        float grainMax = 1f;
        float dofMin = 0.1f;
        float dofMax = 10f;
        float fadeDuration = 0.2f;
        float holdDuration = 0.8f;

        filmGrain.intensity.overrideState = true;
        depthOfField.focusDistance.overrideState = true;

        // fade in
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            filmGrain.intensity.value = Mathf.Lerp(grainMin, grainMax, t);
            depthOfField.focusDistance.value = Mathf.Lerp(dofMax, dofMin, t);
            timer += Time.deltaTime;
            yield return null;
        }

        filmGrain.intensity.value = grainMax;
        depthOfField.focusDistance.value = dofMin;

        // hold
        yield return new WaitForSeconds(holdDuration);

        // fade out
        timer = 0f;
        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            filmGrain.intensity.value = Mathf.Lerp(grainMax, grainMin, t);
            depthOfField.focusDistance.value = Mathf.Lerp(dofMin, dofMax, t);
            timer += Time.deltaTime;
            yield return null;
        }

        filmGrain.intensity.value = grainMin;
        depthOfField.focusDistance.value = dofMax;
        filmGrainRoutine = null;
    }

}
