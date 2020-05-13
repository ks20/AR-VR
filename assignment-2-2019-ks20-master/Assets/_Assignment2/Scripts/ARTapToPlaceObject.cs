using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{
    //private IEnumerator coroutine;
    float movementSmooth = 0.1f;
    [SerializeField]
    GameObject m_PlacedPrefab;

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    ARRaycastManager m_RaycastManager;
    ARRaycastHit tempHit;


    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    //bool TryGetTouchPosition(out Vector2 touchPosition, Vector2 touchPosition2)
    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }

        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            //touchPosition2 = Input.GetTouch(1).position;
            return true;
        }

        touchPosition = default;
        //touchPosition2 = default;
        return false;
    }

    void Update()
    {
        //if (!TryGetTouchPosition(out Vector2 touchPosition, Vector2 touchPosition2))
       	if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
            }
            else
            {
                tempHit = s_Hits[0];
                StartCoroutine(Move());
            }
        }
    }
    
    IEnumerator Move()
    {
    	var pose = tempHit.pose;
        while (spawnedObject.transform.position != pose.position)
        {
            spawnedObject.transform.position = Vector3.Lerp(spawnedObject.transform.position, pose.position, Time.deltaTime * movementSmooth * 10);
            //spawnedObject.transform.rotation = Quaternion.Lerp(spawnedObject.transform.rotation, pose.rotation, Time.deltaTime * movementSmooth);
        }
        yield return null;
    }
}
