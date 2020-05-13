using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Avatar;

public class LightsaberBehavior : MonoBehaviour
{
    //Accessing the script that take care of lightsaber's grabbing state
    OVRGrabbable grabState;

    //The Quillon that already installed on the handle. Should be inactive at the begginning of the game
    [SerializeField]
    public GameObject lightsaberQuillonInstalled;
    //The Quillon module that has not been installed yet.
    [SerializeField]
    public GameObject lightsaberQuillonModule;
    //The active area to snap the quillon module to the handle
    [SerializeField]
    public GameObject quillonConnectZone;
    bool quillonIsInstalled;

    //The Power that already installed on the handle. Should be inactive at the beginning of the game
    [SerializeField]
    public GameObject lightsaberPowerInstalled;
    //The Power module that has not been installed yet
    [SerializeField]
    public GameObject lightsaberPowerModule;
    //The active area to snap the power module to the handle
    [SerializeField]
    public GameObject powerConnectZone;
    bool powerIsInstalled;

    //bool to signal if the lightsaber is done assembling
    bool lightsaberIsAssembled;

    //The blade that already installed on the handle
    [SerializeField]
    public GameObject lightsaberBlade;
    [SerializeField]
    float lightsaberLength = 1f;
    [SerializeField]
    float bladeSmooth = 1f;
    bool bladeIsActivated;

    public AudioClip saberOn, saberOff;
    AudioSource audioSource;

    private void Awake()
    {
        //[TODO]Getting the info of OVRGrabbable
        powerIsInstalled = false;
        quillonIsInstalled = false;
        bladeIsActivated = false;

        grabState = this.GetComponent<OVRGrabbable>();
        audioSource = this.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        //[TODO]Step one: check if the power is connected.
        if (!powerIsInstalled) {
            ConnectingPower();
        }

        //[TODO]Step two: check in the Quillon is connected.
        if (powerIsInstalled) {
            if (!quillonIsInstalled) {
                ConnectingQuillon();
            }
        }

        if (powerIsInstalled) {
            if (quillonIsInstalled) {
                lightsaberBlade.SetActive(true);
            }
        }
        //[TODO]Once the lightsaber is done assembling, set the blade GameObject active.
        
        //[TODO]If the lightsaber is done assembled, change bladeIsActivated after pressing the A button on the R-Controller while the player is grabbing it
        if (grabState.isGrabbed) {
            if (OVRInput.Get(OVRInput.Button.One)) {
                bladeIsActivated = !bladeIsActivated;
                if (bladeIsActivated) {
                    audioSource.PlayOneShot(saberOn);
                }
                else {
                    audioSource.PlayOneShot(saberOff);
                }
            }
        }

        SetBladeStatus(bladeIsActivated);
    }

    void ConnectingPower()
    {
        
        //get the connector state of power
        if (powerConnectZone.GetComponent<LightsaberModuleConnector>().IsConnected) {
            lightsaberPowerInstalled.SetActive(true);
            lightsaberPowerModule.GetComponent<Renderer>().enabled = false;
            powerConnectZone.SetActive(false);
            powerIsInstalled = true;
        }
    }

    void ConnectingQuillon()
    { 
        //same process as in power connection
        if (quillonConnectZone.GetComponent<LightsaberModuleConnector>().IsConnected) {
            lightsaberQuillonInstalled.SetActive(true);
            lightsaberQuillonModule.GetComponent<Renderer>().enabled = false;
            quillonConnectZone.SetActive(false);
            quillonIsInstalled = true;
        }
    }

    void SetBladeStatus(bool bladeStatus)
    {
        if(!bladeStatus)
        {
            //Lightsaber goes back
            lightsaberBlade.transform.localScale = new Vector3(0f, lightsaberBlade.transform.localScale.y, lightsaberBlade.transform.localScale.z);
        }

        if(bladeStatus)
        {
           //Lightsaber pulls out
           lightsaberBlade.transform.localScale = new Vector3(0.4f, lightsaberBlade.transform.localScale.y, lightsaberBlade.transform.localScale.z);
        }
    }
}
