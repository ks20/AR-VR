using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APlatformMovement : MonoBehaviour
{
    public Transform platformTransform;
    bool alreadyCollided;
    float rotationSpeed = 3;

    void Start() {}

    void Update()
    {
    	if (alreadyCollided)
        {
            platformTransform.rotation = Quaternion.Slerp(platformTransform.rotation, Quaternion.Euler(20, 90, 0), Time.deltaTime * rotationSpeed);
        }
    }

    void OnMouseDown()
    {
        alreadyCollided = true;
    }
}
