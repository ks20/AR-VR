using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleExplosion : MonoBehaviour
{
    public GameObject m_particlePrefab;
    public int m_particleCount;
    public float m_particleSizeMin;
    public float m_particleSizeMax;

    private bool m_alreadyExploded;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnDestroy() {
        // When object is destroyed, play explode animation
        // Debug.Log("OnDestroy called");
        Explode();
    }

    public void Explode() {
        for (int i = 0; i < m_particleCount; i++) {
            // Debug.Log("Exploding particle: " + i);
            GameObject newParticle = Instantiate(m_particlePrefab, transform.position, Quaternion.identity);
            newParticle.transform.localScale = new Vector3(
                Random.Range(m_particleSizeMin, m_particleSizeMax),
                Random.Range(m_particleSizeMin, m_particleSizeMax),
                Random.Range(m_particleSizeMin, m_particleSizeMax));
        }
    }
}
