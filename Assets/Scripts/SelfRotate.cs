using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotate : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform trophyTransform;
    public float rotationsPerMinute = 10.0f;

    void Start() {}

    void Update()
    {
    	trophyTransform.Rotate(0, 6.0f * rotationsPerMinute * Time.deltaTime, 6.0f * rotationsPerMinute * Time.deltaTime);
    }
}
