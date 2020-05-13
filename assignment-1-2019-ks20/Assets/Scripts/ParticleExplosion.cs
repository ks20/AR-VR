using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleExplosion : MonoBehaviour
{
	public GameObject originalObject;
	public GameObject particlePrefab;
	public Transform particleTransform;
    public GameObject tempPrefab;

	public int particleCount;
	public float delay;
	public float particleMinSize;
	public float particleMaxSize;

	bool alreadyExploded;

    void Start() {}

    void Update()
    {
    	if (!originalObject.activeInHierarchy && !alreadyExploded) {
    		GetComponent<AudioSource>().Play();
            Invoke("Explode", delay);
    		alreadyExploded = true;
    	}
    }

    void Explode() {
        Instantiate(particlePrefab, particleTransform.position, Quaternion.identity);
        for(int i = 0; i < particleCount; i++)
        {	
            tempPrefab = (GameObject) Instantiate(particlePrefab, particleTransform.position, Quaternion.identity);
            tempPrefab.transform.localScale = tempPrefab.transform.localScale * Random.Range(particleMinSize, particleMaxSize);
        }
    }
}
