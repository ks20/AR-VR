using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trophyStage : MonoBehaviour
{
    public Transform trophyTransform;
    public float rotationsPerMinute = 10.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<AudioSource>().Play();
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
    	trophyTransform.Rotate(0, 6.0f * rotationsPerMinute * Time.deltaTime, 6.0f * rotationsPerMinute * Time.deltaTime);
    }
}
