using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SelfRotate : MonoBehaviour {
    public float m_rotateSpeed = 32.0f;
    public GameObject m_shadowPrefab;
    public GameObject m_currentShadow;

    private List<ARRaycastHit> m_hits = new List<ARRaycastHit>();
    private GameObject m_arSessionOrigin;
    private ARRaycastManager m_raycastManager;

    public bool m_isCoin;

    // Start is called before the first frame update
    void Start() {
        m_arSessionOrigin = GameObject.Find("AR Session Origin");
        m_raycastManager = m_arSessionOrigin.GetComponent<ARRaycastManager>();
        m_rotateSpeed = m_rotateSpeed + Mathf.RoundToInt(Random.Range(0, 10));
        // Debug.Log("Rotate speed: " + m_rotateSpeed);

        // Debug.Log("Shadow null, creating it");
        m_currentShadow = Instantiate(m_shadowPrefab);
        m_currentShadow.SetActive(false);
        if (!m_isCoin) {
            m_currentShadow.transform.localScale = m_shadowPrefab.transform.localScale; // new Vector3(transform.localScale.x, 0.0001f, transform.localScale.z);
        } else {
            m_currentShadow.transform.localScale = m_shadowPrefab.transform.localScale * 0.05f;
        }
    }

    // Update is called once per frame
    void Update() {
        float step = m_rotateSpeed * Time.deltaTime;
        transform.RotateAround(transform.position, new Vector3(0, 90, 0), step);

        // Create a shadow to give better depth cues
        Ray ray = new Ray(transform.position, -transform.up);
        if (m_raycastManager.Raycast(ray, m_hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)) {
            if (!m_currentShadow.activeInHierarchy) {
                m_currentShadow.transform.position = m_hits[0].pose.position;
                m_currentShadow.transform.rotation = transform.rotation;
                m_currentShadow.SetActive(true);
            }
            m_currentShadow.transform.position = m_hits[0].pose.position;
            m_currentShadow.transform.rotation = transform.rotation;
        }
    }
}
