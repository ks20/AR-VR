using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPlatformMovement : MonoBehaviour
{
	public Transform bTransform;
	bool alreadyCollided;
    float movementSpeed = 10;
    int count;

    void Start()
    {
    	count = 0;
    }

    void Update()
    {
    	if (alreadyCollided && count == 1)
        {
            Vector3 dest = new Vector3(transform.position.x, transform.position.y, 8);
            transform.position = Vector3.MoveTowards(transform.position, dest, movementSpeed * Time.deltaTime);
        }
        if (alreadyCollided && count == 2)
        {
            Vector3 dest = new Vector3(transform.position.x, transform.position.y, -13);
            transform.position = Vector3.MoveTowards(transform.position, dest, movementSpeed * Time.deltaTime);
        }
    }

    void OnMouseDown()
    {
        count = count + 1;
        alreadyCollided = true;
    }
}
