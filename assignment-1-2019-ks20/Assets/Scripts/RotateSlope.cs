using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSlope : MonoBehaviour
{
    //A cube you want to use its position y as reference to the elevators
    public Transform slopeTransform;

    bool alreadyCollided;

    Vector3 currentEulerAngles;
    Quaternion currentRotation;
    float rotationSpeed = 1;

    void Start() {}

    void Update()
    {
    	if (alreadyCollided)
        {
            slopeTransform.rotation = Quaternion.Slerp(slopeTransform.rotation, Quaternion.Euler(0, 0, -50), Time.deltaTime * rotationSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            alreadyCollided = true;
        }
    }
}
