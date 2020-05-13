using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkBehavior : MonoBehaviour
{
    public Vector3 startVelocity;
    public Rigidbody rb;
    public float delay;
    public float shrinkSmooth;

    void Start()
    {
    	rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3 (1.0f, 10.0f, 2.0f);

    	startVelocity = Random.rotation * rb.velocity;

        rb.velocity = startVelocity;
        Invoke("Die", delay);
    }

    void Update()
    {
    	if (rb != null) {
            Vector3 fromScale = rb.transform.localScale;
            Vector3 toScale = Vector3.zero;
            rb.transform.localScale = Vector3.Lerp(fromScale, toScale, Time.deltaTime * shrinkSmooth);
        }
    }

    void Die() {
    	Destroy(rb);
    }
}
