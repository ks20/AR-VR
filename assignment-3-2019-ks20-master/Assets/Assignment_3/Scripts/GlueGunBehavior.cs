using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar;

public class GlueGunBehavior : MonoBehaviour
{
    OVRGrabbable grabState;
    //DistanceGrabbable grabState;

    //The interactive area that would be activated when pressing down the trigger while grabbing the gluegun
    [SerializeField]
    GameObject glueZone;

    private void Awake()
    {
        //Get component of the OVRGrabbable
        grabState = this.GetComponent<OVRGrabbable>();
    }

    private void FixedUpdate()
    {
        //If the gluegun is being grabbed, the gluezone is active while the trigger is pressed
        if (grabState.isGrabbed) {
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) {
                glueZone.SetActive(true);
                glueZone.GetComponent<MeshRenderer>().enabled = false;
                glueZone.transform.GetChild(0).gameObject.SetActive(true);
            }
            else {
                glueZone.SetActive(false);
            }
        }
        else {
            glueZone.SetActive(false);
        }
    }
}
