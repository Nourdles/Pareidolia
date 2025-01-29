using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class SanityTracker : MonoBehaviour
{
    /// <summary>
    /// SanityTracker is a prefab that can be used to register the visibility of objects on the camera and impact sanity from there
    /// 
    /// The two public game object variables camera and stains are used to set the camera and specific objects to check LOS for
    /// 
    /// Additionally, public API function registerStain is available for use when a new stain is created
    /// 
    /// 
    /// </summary>
    public Camera camera;

    //Active stains 
    //Note that this script is responsible for null checking accessed stains
    public GameObject[] stains;

    //Sanity percentage
    private float sanity = 100;


    public int stainDamageGracePeriod = 3;
    public int stainDamageFreq = 15;

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
        Assert.IsNotNull(camera);
        for (int i = 0; i < stains.Length; i++)
        {
            stainInfo.Add(new StainInfo(stainDamageGracePeriod));
        }
        Debug.Log(stainInfo.ToString());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(camera);

        for(int i = 0; i < stains.Length; i++)
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

        if(sanity < 0)
        {
            onLoss();
        }
    }

    private void onLoss()
    {
        Debug.Log("Game Over");
    }

    private void onStainDamage(GameObject stain)
    {
        Debug.Log("Object Damage " + stain.name);
        sanity--;
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
        stains.Append(stain);
    }

    public float getSanity()
    {
        return sanity;
    }
}
