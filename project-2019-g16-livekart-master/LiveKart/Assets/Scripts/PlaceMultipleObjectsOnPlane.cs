using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaceMultipleObjectsOnPlane : MonoBehaviour {
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_powerUpPlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject powerUpPlacedPrefab {
        get { return m_powerUpPlacedPrefab; }
        set { m_powerUpPlacedPrefab = value; }
    }

    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_coinPlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject coinPlacedPrefab {
        get { return m_coinPlacedPrefab; }
        set { m_coinPlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    /*
    THIS IS OUR SHIT
    */
    public List<GameObject> Coins;
    public List<GameObject> PowerUps;
    public GameObject sceneController;
    private SceneController sceneScript;
    private bool m_isSetupFinished;
    private GameObject m_currentObject;
    public float m_smoothTime = 0.2f;
    private bool m_isCoin = true;
    public float m_objectHeight = 0.18f;

    /// <summary>
    /// Invoked whenever an object is placed in on a plane.
    /// </summary>
    public static event Action onPlacedObject;

    ARRaycastManager m_RaycastManager;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake() {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    void Start() {
        sceneScript = sceneController.GetComponent<SceneController>();
    }

    void Update() {
        if (!m_isSetupFinished) {
            // When in setup mode, hover a cube above the plane so user can visualize possible placements
            Ray r = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (m_RaycastManager.Raycast(r, s_Hits, TrackableType.PlaneWithinPolygon)) {
                Pose hitPose = s_Hits[0].pose;
                // Debug.Log("Ray from camera hitting plane at: " + hitPose.position);
                Vector3 velocity = Vector3.zero;

                if (sceneScript.currStateIsSetup1()) {
                    // Coin instantiation
                    if (m_currentObject is null) {
                        // Debug.Log("Instantiating coin");
                        m_currentObject = Instantiate(m_coinPlacedPrefab, hitPose.position, hitPose.rotation);
                        // Place coin slightly above plane
                        m_currentObject.transform.position += (m_currentObject.transform.up * m_objectHeight);
                    }
                } else if (sceneScript.currStateIsSetup2()) {
                    if (m_isCoin) {
                        m_currentObject.SetActive(false);
                        m_currentObject.GetComponent<SelfRotate>().m_currentShadow.SetActive(false);
                        // Debug.Log("Instantiating cube from coin");
                        m_currentObject = Instantiate(m_powerUpPlacedPrefab, hitPose.position, hitPose.rotation);
                        // Place power up slightly above plane
                        m_currentObject.transform.position += (m_currentObject.transform.up * m_objectHeight);
                        m_isCoin = false;
                    }
                    if (m_currentObject is null) {
                        m_currentObject = Instantiate(m_powerUpPlacedPrefab, hitPose.position, hitPose.rotation);
                        // Place power up slightly above plane
                        m_currentObject.transform.position += (m_currentObject.transform.up * m_objectHeight);
                    }
                }

                // m_currentCube.transform.position = hitPose.position + (m_currentCube.transform.up * 0.18f);
                m_currentObject.transform.position = Vector3.SmoothDamp(
                    m_currentObject.transform.position,
                    hitPose.position + (m_currentObject.transform.up * m_objectHeight),
                    ref velocity,
                    m_smoothTime);
            }

            if (Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began) {
                    if (!IsPointerOverUIObject()) {
                        if (sceneScript.currStateIsSetup1()) {
                            Coins.Add(m_currentObject);
                            Debug.Log("Coins count: " + Coins.Count);
                            m_currentObject = Instantiate(m_coinPlacedPrefab, m_currentObject.transform.position, m_currentObject.transform.rotation);
                            gameObject.GetComponent<AudioSource>().Play();
                        } else if (sceneScript.currStateIsSetup2()) {
                            PowerUps.Add(m_currentObject);
                            m_currentObject = Instantiate(m_powerUpPlacedPrefab, m_currentObject.transform.position, m_currentObject.transform.rotation);
                            gameObject.GetComponent<AudioSource>().Play();
                        }
                        Handheld.Vibrate();
                    }
                }
            }
            if (sceneScript.currStateIsPlay() || sceneScript.currStateIsSetup3()) {
                m_isSetupFinished = true;
                Debug.Log("Setup finished");
                if (m_currentObject.activeInHierarchy) {
                    Debug.Log("Removing preview object");
                    m_currentObject.SetActive(false);
                    m_currentObject.GetComponent<SelfRotate>().m_currentShadow.SetActive(false);
                }
            }
        }
    }

    private bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
