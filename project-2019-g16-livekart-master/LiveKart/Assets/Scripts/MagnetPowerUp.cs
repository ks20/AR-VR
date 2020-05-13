using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MagnetPowerUp : MonoBehaviour
{
    public float m_movementSpeed = 1.0f;
    public bool m_powerUpActive = false;
    public GameObject m_magnetPrefab;
    public float m_powerUpActiveTime = 10;
    public GameObject m_carFront;
    public ARSessionOrigin m_sessionOrigin;
    public AudioSource m_audioSource;

    private GameObject m_magnet;
    private float m_remainingTime;
    private List<GameObject> m_collectibleItems;

    // Start is called before the first frame update
    void Start() {
        m_remainingTime = m_powerUpActiveTime;

        m_collectibleItems = m_sessionOrigin.GetComponent<PlaceMultipleObjectsOnPlane>().Coins;

        Debug.Log("Initializing magnet GameObject");
        Debug.Log("Num coins: " + m_collectibleItems.Count);
        Vector3 initPos = Camera.main.transform.position + Camera.main.transform.forward;
        m_magnet = Instantiate(m_magnetPrefab, initPos, Camera.main.transform.rotation);
        m_magnet.transform.localScale *= 0.00075f; // magnetpf3 is 0.0005 and magnetpf is .5
        m_magnet.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        float step = Time.deltaTime * m_movementSpeed;
        if (m_powerUpActive) {
            Debug.Log("Num coins: " + m_collectibleItems.Count);
            // Play sound effect
            m_audioSource.Play();
            if (!m_magnet.activeInHierarchy) {
                Debug.Log("Magnet inactive, setting active");
                m_magnet.SetActive(true);
            }
            // Need to start a timer for this powerup
            if (m_remainingTime <= 0) {
                Debug.Log("Timer ended. Setting power up inactive");
                m_powerUpActive = false;
                m_remainingTime = m_powerUpActiveTime;

                // Deactivate magnet
                m_magnet.SetActive(false);
            } else {
                m_remainingTime -= Time.deltaTime;
                // Debug.Log("Remaining time: " + m_remainingTime);
            }

            // Bring objects closer to player car
            MoveObjectsTowardPlayer(m_collectibleItems, step);

            m_magnet.transform.position = m_carFront.GetComponent<DrawCarFront>().m_carFrontGamePiece.transform.position;
            m_magnet.transform.position += (m_carFront.GetComponent<DrawCarFront>().m_carFrontGamePiece.transform.up * 0.16f); // TODO: Same height as cube? (cube is at 0.18 originally, but likely needs to come down now that we have the car)
            m_magnet.transform.rotation = m_carFront.GetComponent<DrawCarFront>().m_carFrontGamePiece.transform.rotation;
            m_magnet.transform.Rotate(new Vector3(90, -90, 0));
        }
    }

    void MoveObjectsTowardPlayer(List<GameObject> objects, float step) {
        // Player is now at the carFront position, so update accordingly
        Debug.Log("Bringing '" + objects.Count + "' coins to player");
        foreach (GameObject g in objects) {
            g.transform.position = Vector3.Lerp(
                g.transform.position,
                m_carFront.GetComponent<DrawCarFront>().m_carFrontGamePiece.transform.position,
                step);
        }
    }
}
