using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeTransform : MonoBehaviour
{
    public Transform bridgeTransform;
    bool alreadyCollided;
    float rotationSpeed = 3;
    int count;
    
    void Start()
    {
    	count = 0;
    }

    void Update()
    {
    	if (alreadyCollided && count == 1)
        {
            bridgeTransform.rotation = Quaternion.Slerp(bridgeTransform.rotation, Quaternion.Euler(0, 90, -5), Time.deltaTime * rotationSpeed);
        }
        if (alreadyCollided && count == 2) {
        	bridgeTransform.rotation = Quaternion.Slerp(bridgeTransform.rotation, Quaternion.Euler(0, 180, -5), Time.deltaTime * rotationSpeed);
        }
    }

    void OnMouseDown()
    {       	
        count = count + 1;
        alreadyCollided = true;
    }
}
