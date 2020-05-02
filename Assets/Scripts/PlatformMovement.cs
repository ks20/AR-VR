using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public Transform platformTransform;

    bool alreadyCollided;
    Vector3 currentEulerAngles;
    Quaternion currentRotation;
    float rotationSpeed = 3;

    void Start() {}

    void Update()
    {
    	if (alreadyCollided)
        {
            platformTransform.rotation = Quaternion.Slerp(platformTransform.rotation, Quaternion.Euler(-40, 0, 0), Time.deltaTime * rotationSpeed);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            //GetComponent<AudioSource>().Play();
            alreadyCollided = true;
        }
    }
}
