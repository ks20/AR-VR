using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlatformMovement : MonoBehaviour
{
    public Transform cTransform;
    bool alreadyCollided;
    float rotationSpeed = 3;

    void Start() {}

    void Update()
    {
    	if (alreadyCollided)
        {
            cTransform.rotation = Quaternion.Slerp(cTransform.rotation, Quaternion.Euler(60, -180, 0), Time.deltaTime * rotationSpeed);
        }
    }

    void OnMouseDown()
    {
        alreadyCollided = true;
    }
}
