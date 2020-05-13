using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArrowDirector : MonoBehaviour
{
    public GameObject m_arrowPrefab;
    public ARSessionOrigin m_sessionOrigin;
    public bool m_powerUpActive;
    public GameObject m_carFront;
    public float m_powerUpActiveTime = 10;

    private List<GameObject> m_collectibleItems;
    private GameObject m_arrow;
    private GameObject m_nearestNeighbor;
    private float m_remainingTime;

    // Start is called before the first frame update
    void Start() {
        // Init list of cubes we know of
        m_collectibleItems = m_sessionOrigin.GetComponent<PlaceMultipleObjectsOnPlane>().Coins;
        m_remainingTime = m_powerUpActiveTime;

        Debug.Log("Instantiating arrow for first time");
        // m_arrow = Instantiate(m_arrowPrefab, m_carFront.GetComponent<DrawCarFront>().m_carFrontGamePiece.transform.position, Quaternion.identity);
        m_arrow = Instantiate(m_arrowPrefab); // Don't have car front yet
        m_arrow.transform.localScale *= 0.035f; // blue arrow should be 0.3f; orange arrow at 0.015f
        m_arrow.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (m_powerUpActive) {
            if (!m_arrow.activeInHierarchy) {
                Debug.Log("Arrow inactive; activating it");
                m_arrow.SetActive(true);
            }

            // Need to start a timer for this powerup
            if (m_remainingTime <= 0) {
                Debug.Log("Timer ended. Setting power up inactive");
                m_powerUpActive = false;
                m_remainingTime = m_powerUpActiveTime;

                // Deactivate Arrow
                m_arrow.SetActive(false);
            } else {
                m_remainingTime -= Time.deltaTime;
                // Debug.Log("Remaining time: " + m_remainingTime);
            }

            m_collectibleItems = m_sessionOrigin.GetComponent<PlaceMultipleObjectsOnPlane>().Coins;

            if (m_collectibleItems.Count > 0) {
                // Debug.Log("First collectible item count location: " + m_collectibleItems[0].transform.position);

                // Place arrow above car
                m_arrow.transform.position = m_carFront.GetComponent<DrawCarFront>().m_carFrontGamePiece.transform.position;
                m_arrow.transform.position += (m_arrow.transform.up * 0.16f); // TODO: Same height as cube? (cube is at 0.18 originally, but likely needs to come down now that we have the car)

                // Start nearest neighbor calculation coroutine
                StartCoroutine(CalculateNearestNeighbor(m_arrow.transform.position, m_collectibleItems));

                // Direct arrow to look towards closest block
                m_arrow.transform.LookAt(m_nearestNeighbor.transform.position);
                // Debug.Log("Arrow pos: " + m_arrow.transform.position + "; car pos: " + m_carFront.GetComponent<DrawCarFront>().m_carFrontGamePiece.transform.position);
            }
        }
    }

    // Calculates (brute force) nearest neighbor cube to car
    IEnumerator CalculateNearestNeighbor(Vector3 currentPosition, List<GameObject> neighbors) {
        // TODO: Mutex on neighbors datastructure
        if (neighbors.Count == 0) {
            // Debug.Log("No nearest neighbor to calculate");
            m_nearestNeighbor = null; // No neighbor to point to
            yield return new WaitForSeconds(0.5f);
        } else if (neighbors.Count == 1) {
            // Debug.Log("Only 1 neighbor at position: " + neighbors[0].transform.position);
            m_nearestNeighbor = neighbors[0];
            yield return new WaitForSeconds(0.5f);
        } else {
            // Calculate min distance and nearest neighbor
            float minDistance = Vector3.Distance(neighbors[0].transform.position, currentPosition);
            m_nearestNeighbor = neighbors[0];
            for (int i=1; i < neighbors.Count; i++) {
                float d = Vector3.Distance(neighbors[i].transform.position, currentPosition);
                if (d < minDistance) {
                    minDistance = d;
                    m_nearestNeighbor = neighbors[i];
                    // Debug.Log("New nearest neighbor at: " + m_nearestNeighbor.transform.position);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

}
