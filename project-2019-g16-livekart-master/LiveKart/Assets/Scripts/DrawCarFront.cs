using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DrawCarFront : MonoBehaviour
{
    public GameObject m_carFrontPrefab;
    public GameObject m_arSessionOrigin;
    public GameObject m_carFrontGamePiece;
    public GameObject m_sceneController;

    private ARRaycastManager m_arRaycastManager;
    private List<ARRaycastHit> m_hits = new List<ARRaycastHit>();
    private bool m_drawCar;
    public bool m_isRaycastRenderMode;

    // Start is called before the first frame update
    void Start() {
        m_arRaycastManager = m_arSessionOrigin.GetComponent<ARRaycastManager>();

        // Collider for front of the car
        // transform.localScale *= 0.025f;
        // transform.localScale = new Vector3(Screen.width * 1.5f, Screen.height * 1.5f, 0);

        // TODO: Bool for testing
        // m_drawCar = true;

        Debug.Log("Instantiating car for first time");
        Debug.Log("Raycast render mode is: " + m_isRaycastRenderMode);
        m_carFrontGamePiece = Instantiate(m_carFrontPrefab);
        if (m_isRaycastRenderMode) {
            m_carFrontGamePiece.transform.localScale *= 0.11f;
            // transform.localScale = m_carFrontGamePiece.transform.localScale * 6.0f;
            transform.localScale = new Vector3(m_carFrontGamePiece.transform.localScale.x * 2.0f, m_carFrontGamePiece.transform.localScale.y * 4.0f, m_carFrontGamePiece.transform.localScale.z * 2.0f);
            // transform.localScale = m_carFrontGamePiece.transform.localScale * 2.0f; // 2.0f a little too small
        } else {
            m_carFrontGamePiece.transform.localScale *= 0.12f; // TODO: Make this slightly bigger eventually to fill up screen better?
            transform.localScale = m_carFrontGamePiece.transform.localScale * 4.0f;
        }
        m_carFrontGamePiece.SetActive(false);

        // Collider for front of the car
        // transform.localScale = m_carFrontGamePiece.transform.localScale;
        // transform.localScale.Set(transform.localScale.x * 2.0f, transform.localScale.y * 2.0f, transform.localScale.z * 2.0f);
        // transform.localScale *= 3.0f;
        // gameObject.SetActive(false);
        // transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2.0f, transform.localScale.z * 2.0f);
    }

    public void ToggleRenderMode() {
        m_isRaycastRenderMode ^= true;
    }

    // Update is called once per frame
    void Update() {
        // Cast ray onto plane to place the car during Game mode (or setup 3 probably);
        if (m_sceneController.GetComponent<SceneController>().currStateIsSetup3() || m_sceneController.GetComponent<SceneController>().currStateIsPlay() || m_sceneController.GetComponent<SceneController>().currStateIsGameOver()) {

            if (!m_isRaycastRenderMode) {
                // Mount car mode
                if (!m_carFrontGamePiece.activeInHierarchy) {
                    m_carFrontGamePiece.SetActive(true);
                }
                // Car game piece rendering
                Vector3 carPos = Camera.main.transform.forward + Camera.main.transform.position + new Vector3(0, -0.2f, 0);
                m_carFrontGamePiece.transform.position = carPos;
                Vector3 lookpos = m_carFrontGamePiece.transform.position - Camera.main.transform.position;
                lookpos.y = m_carFrontGamePiece.transform.localScale.y; // Raises car look position up slightly
                m_carFrontGamePiece.transform.rotation = Quaternion.LookRotation(lookpos);

                // Collider
                // transform.position = m_carFrontGamePiece.transform.position + (m_carFrontGamePiece.transform.up * 0.1f);
                // transform.position = m_carFrontGamePiece.transform.position;
                transform.position = Camera.main.transform.position + Camera.main.transform.forward + new Vector3(0, -0.2f, 0);
                // Collider Look at camera
                transform.LookAt(Camera.main.transform);
                // Vector3 cubePos = m_arSessionOrigin.GetComponent<PlaceMultipleObjectsOnPlane>().PowerUps[0].transform.position;
                // Debug.Log("Collider pos: " + transform.position + "; car pos: " + carPos + "; cube pos: " + cubePos);
            } else {
                // TODO: Draw ray directly down then move the car forward slightly
                Ray r = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
                // Ray r = new Ray(Camera.main.transform.position, -Camera.main.transform.up); // Using down vector now
                if (m_arRaycastManager.Raycast(r, m_hits, TrackableType.PlaneWithinPolygon)) {

                    // Instantiate car game piece if we haven't already
                    // if (m_carFrontGamePiece == null) {
                    if (!m_carFrontGamePiece.activeInHierarchy) {
                        // Debug.Log("Instantiating car for first time");
                        // m_carFrontGamePiece = Instantiate(m_carFrontPrefab, m_hits[0].pose.position, m_hits[0].pose.rotation);
                        // m_carFrontGamePiece.transform.localScale *= 0.05f; // TODO: Make this slightly bigger eventually to fill up screen better?
                        m_carFrontGamePiece.SetActive(true);
                    }
                    // TODO: Use hit position, but make the x and z position slightly closer to camera?
                    Vector3 carPos = new Vector3((m_hits[0].pose.position.x * 0.5f), m_hits[0].pose.position.y, (m_hits[0].pose.position.z * 0.5f));

                    m_carFrontGamePiece.transform.position = m_hits[0].pose.position; //  + (Camera.main.transform.forward * 3.0f); // TODO: Bring car closer to camera slightly?
                    // Debug.Log("Camera posotion: " + Camera.main.transform.position + "; Car pos: " + m_carFrontGamePiece.transform.position);
                    // Direct car to look away from camera
                    Vector3 lookpos = m_carFrontGamePiece.transform.position - Camera.main.transform.position;
                    lookpos.y = m_carFrontGamePiece.transform.localScale.y; // Raises car look position up slightly
                    m_carFrontGamePiece.transform.rotation = Quaternion.LookRotation(lookpos);

                    // Update collider position to match that of car
                    transform.position = m_carFrontGamePiece.transform.position;
                    // transform.position = m_carFrontGamePiece.transform.position + new Vector3(0, 2.0f, 0);
                    /*
                    if (m_sceneController.GetComponent<SceneController>().currStateIsPlay()) {
                        Vector3 cubePos = m_arSessionOrigin.GetComponent<PlaceMultipleObjectsOnPlane>().PowerUps[0].transform.position;
                        Debug.Log("; car pos: " + carPos + "; cube pos: " + cubePos);
                    }
                    */
                    // transform.LookAt(Camera.main.transform);
                }
            }
        }
    }
}
