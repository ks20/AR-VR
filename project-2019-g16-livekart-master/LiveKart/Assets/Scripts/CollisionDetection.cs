using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionDetection : MonoBehaviour
{
    public GameObject m_sceneController;
    public GameObject m_arSessionOrigin;
    public bool m_isCollisionActive;
    public GameObject m_powerUpSpinnerPrefab;
    public float m_spinnerActiveTime = 3.0f;
    // public GameObject m_carGamePiece;
    private GameObject m_carFrontGamePiece;
    private GameObject m_currentSpinner;
    private bool m_spinnerActive;
    private float m_remainingTime;
    private string m_face;
    public AudioSource m_spinnerAudio;
    public AudioSource m_collisionAudio;
    public GameObject m_splatCubePrefab;
    public GameObject m_arrowCubePrefab;
    public GameObject m_magnetCubePrefab;
    public GameObject m_clockCubePrefab;

    private GameObject m_splatCube;
    private GameObject m_arrowCube;
    private GameObject m_magnetCube;
    private GameObject m_clockCube;
    private GameObject m_currentPowerUpCube;

    // Start is called before the first frame update
    void Start() {
        m_carFrontGamePiece = GetComponent<DrawCarFront>().m_carFrontGamePiece;    
        if (m_currentSpinner is null) {
            Debug.Log("Instantiating power up spinner for first time");
            m_currentSpinner = Instantiate(m_powerUpSpinnerPrefab);
            m_currentSpinner.SetActive(false);
        }
        m_remainingTime = m_spinnerActiveTime;

        // Instantiate all cubes, but hide for now
        m_splatCube = Instantiate(m_splatCubePrefab, Vector3.zero, Quaternion.identity);
        m_arrowCube = Instantiate(m_arrowCubePrefab, Vector3.zero, Quaternion.identity);
        m_magnetCube = Instantiate(m_magnetCubePrefab, Vector3.zero, Quaternion.identity);
        m_clockCube = Instantiate(m_clockCubePrefab, Vector3.zero, Quaternion.identity);
        m_splatCube.SetActive(false);
        m_arrowCube.SetActive(false);
        m_magnetCube.SetActive(false);
        m_clockCube.SetActive(false);

        if (m_currentPowerUpCube is null) {
            m_currentPowerUpCube = m_splatCube;
            m_currentPowerUpCube.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update() {
        m_currentSpinner.transform.position = m_carFrontGamePiece.transform.position + (m_carFrontGamePiece.transform.up * 0.23f);
        m_currentPowerUpCube.transform.position = m_carFrontGamePiece.transform.position + (m_carFrontGamePiece.transform.up * 0.23f);

        // Need to start a timer for this powerup
        if (m_spinnerActive) {
            if (!m_currentSpinner.activeInHierarchy) {
                Debug.Log("Spinner inactive, setting active");
                m_currentSpinner.transform.localScale = m_powerUpSpinnerPrefab.transform.localScale;
                m_currentSpinner.transform.position = m_carFrontGamePiece.transform.position + (m_carFrontGamePiece.transform.up * 0.23f);
                m_currentSpinner.SetActive(true);
            }
            if (m_remainingTime <= 2) {
                // Deactivate spinner and activate power up cube
                m_currentSpinner.SetActive(false);

                switch (m_face) {
                    case ("Magnet"):
                        m_currentPowerUpCube = m_magnetCube;
                        break;
                    case ("Arrow"):
                        m_currentPowerUpCube = m_arrowCube;
                        break;
                    case ("Ink"):
                        m_currentPowerUpCube = m_splatCube;
                        break;
                    case ("Clock"):
                        m_currentPowerUpCube = m_clockCube;
                        break;
                    default:
                        Debug.Log("Unknown face");
                        break;
                }
                m_currentPowerUpCube.transform.localScale = m_clockCubePrefab.transform.localScale;
                m_currentPowerUpCube.transform.position = m_carFrontGamePiece.transform.position + (m_carFrontGamePiece.transform.up * 0.23f);
                m_currentPowerUpCube.SetActive(true);
            }
            if (m_remainingTime <= 0) {
                // Activate power up
                ActivatePowerUp(m_face);

                Debug.Log("Timer ended. Setting spinner inactive");
                m_spinnerActive = false;
                m_remainingTime = m_spinnerActiveTime;

                // Deactivate powre up cube
                m_currentPowerUpCube.SetActive(false);

                // Deactivate spinner
                // m_currentSpinner.SetActive(false);
            } else {
                m_remainingTime -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (m_isCollisionActive) {
            if (other.CompareTag("Coin")) {
                Debug.Log("Hit Coin");

                // Play sound effect for colliding with Coin
                m_collisionAudio.Play();

                // Update as needed for new implementation
                // Remove from datastructure storing this item
                // List<GameObject> spawnedObjects = m_arSessionOrigin.GetComponent<PlaceMultipleObjectsOnPlane>().Coins;
                m_arSessionOrigin.GetComponent<PlaceMultipleObjectsOnPlane>().Coins.Remove(other.gameObject);
                // spawnedObjects.Remove(other.gameObject); // This is bad!
                // spawnedObjects.RemoveAt(spawnedObjects.Count - 1);

                // Debug.Log("Spawned objects count after collision: " + spawnedObjects.Count);
                Debug.Log("Spawned objects count after collision: " + m_arSessionOrigin.GetComponent<PlaceMultipleObjectsOnPlane>().Coins.Count);

                // Deactivate and destroy item and its shadow
                other.gameObject.SetActive(false);
                GameObject shadow = other.gameObject.GetComponent<SelfRotate>().m_currentShadow;
                shadow.SetActive(false);
                Destroy(shadow);

            } else if (other.CompareTag("PowerUp")) {
                Debug.Log("Hit powerup");

                // TODO: Remove from power ups list and/or deactivate/destroy power up
                GameObject shadow = other.gameObject.GetComponent<SelfRotate>().m_currentShadow;
                shadow.SetActive(false);
                Destroy(shadow);

                // Play explosion animation
                other.gameObject.GetComponent<ParticleExplosion>().Explode();
                other.gameObject.SetActive(false);
                Destroy(other);

                // Instantiate the power up spinner top left of banner
                Vector3 spinnerPos = m_carFrontGamePiece.transform.position;
                m_currentSpinner.SetActive(true);
                m_currentSpinner.transform.position = spinnerPos;

                int numSpins = m_currentSpinner.GetComponent<RotatePowerUp>().GetSpins(8, 20);
                m_currentSpinner.GetComponent<RotatePowerUp>().StartRotate(numSpins);
                m_face = m_currentSpinner.GetComponent<RotatePowerUp>().returnFace(numSpins);
                Debug.Log("Face take 3: " + m_face + "; num spins: " + numSpins);

                // TODO: REmove
                // m_face = "Magnet";

                // Start timer for setting spinner inactive
                m_spinnerActive = true;

                // Play spinner sound effect
                m_spinnerAudio.Play();
            }
        }
    }

    void ActivatePowerUp(string powerUp) {
        switch(powerUp) {
            case ("Magnet"):
                Debug.Log("Activating magnet power up");
                m_sceneController.GetComponent<MagnetPowerUp>().m_powerUpActive = true;
                break;
            case ("Arrow"):
                Debug.Log("Activating arrow powerup");
                m_sceneController.GetComponent<ArrowDirector>().m_powerUpActive = true;
                break;
            case ("Ink"):
                Debug.Log("Activating Ink power down");
                m_sceneController.GetComponent<InkSplatter>().m_powerUpActive = true;
                break;
            case ("Clock"):
                Debug.Log("Activating Clock power up");
                // TODO: Alter time by a 5 seconds
                m_sceneController.GetComponent<SceneController>().DecrementTime(6.0f);
                break;
            default:
                Debug.Log("Unknown power up");
                break;
        }
    }

    void KillSpinner() {
        m_currentSpinner.SetActive(false);
    }
}
