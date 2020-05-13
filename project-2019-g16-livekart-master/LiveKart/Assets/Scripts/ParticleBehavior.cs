using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehavior : MonoBehaviour
{
    public Vector3 m_startVelocity;
    public float m_dieDelay;
    public float m_shrinkSmooth = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Random.rotation * m_startVelocity;
        Invoke("Die", m_dieDelay);
    }

    // Update is called once per frame
    void Update()
    {
        float step = m_shrinkSmooth * Time.deltaTime;
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, step);
    }

    void Die() {
        // Debug.Log("Killing particle");
        Destroy(gameObject);
    }
}
