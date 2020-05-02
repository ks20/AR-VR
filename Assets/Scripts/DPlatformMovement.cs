using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPlatformMovement : MonoBehaviour
{
    public GameObject dPlatform;
    bool alreadyCollided;
    float movementSpeed = 10;
    
    void Start() {}

    void Update()
    {
    	if (alreadyCollided)
        {
            Vector3 dest = new Vector3(dPlatform.transform.position.x, 3, dPlatform.transform.position.x);
            transform.position = Vector3.MoveTowards(transform.position, dest, movementSpeed * Time.deltaTime);
        }
    }

    void OnMouseDown()
    {
        alreadyCollided = true;
    }
}
