using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkSplatter : MonoBehaviour {
    // public GameObject m_inkSplatterPrefab;
    // public GameObject m_inkSplatterSpritePrefab;
    // public int m_numSplatters;
    public bool m_powerUpActive;
    public float m_powerUpActiveTime = 10;
    // public int m_splatterDelay = 1;
    public GameObject m_squidPrefab;
    public GameObject m_splattersParent;
    public AudioSource m_audioSource;

    // private List<GameObject> m_splatters = new List<GameObject>();
    private float m_remainingTime;

    void Start() {
        m_remainingTime = m_powerUpActiveTime;
    }

    // Update is called once per frame
    void Update() {
        if (m_powerUpActive) {
            // Sound effect
            m_audioSource.Play();

            if (!m_splattersParent.activeInHierarchy) {
                Debug.Log("splatters inactive; activating it");
                m_splattersParent.SetActive(true);
            }
            // Need to start a timer for this powerup
            if (m_remainingTime <= 0) {
                Debug.Log("Timer ended. Setting power up inactive");
                m_powerUpActive = false;
                m_remainingTime = m_powerUpActiveTime;

                // Deactivate Splatters
                m_splattersParent.SetActive(false);
            } else {
                m_remainingTime -= Time.deltaTime;
                // Debug.Log("Remaining time: " + m_remainingTime);
            }
        }
    }

    /*
    void DeactivateSplatters() {
        Debug.Log("Deactivating " + m_splatters.Count + " splatters");
        foreach (GameObject splatter in m_splatters) {
            // Deactivate 
            splatter.SetActive(false);
        }
    }

    IEnumerator GenerateSplatter() {
        for (int i = 0; i < m_numSplatters; i ++) {
            m_splatters[i].SetActive(true);
            Debug.Log("Splatter position: " + m_splatters[i].transform.position);
            yield return new WaitForSeconds(m_splatterDelay);
        }
    }
    */
}
