using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ARRaycastManager))]
public class SceneController_Part2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ARSessionOrigin;
    public GameObject m_PlacedCube;
    public GameObject distanceCalc;
    public LineRenderer lRPrefab;
    public GameObject lRStart;
    public GameObject lRCube;
    public GameObject shadow;
    private GameObject shadowCube;
    public Slider slider;
    public List<GameObject> distanceTexts;
    public ARRaycastManager raycastManager;

    Vector3 startPosition, finalPosition;
    
    private GameObject actualCube;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    private List<GameObject> cubes = new List<GameObject>();

    private GameObject lastCube;
    private GameObject prevCube;
    private int bezierCurvePoints = 50;
    private float smoothTime = 0.15F;
    private Vector3 velocity = Vector3.zero;
    private Camera cam;

    bool isLine = false;
    float delay = 0;
    float cubePosZ = 3.0f;
    //float cubePosZ;

    float distanceCalculation;
    float diff;
    //ARRaycastManager m_RaycastManager;
    LineRenderer lR;
    LineRenderer lR2;
    //private Pose hitPose;

    /// <summary>
	/// The object instantiated as a result of a successful raycast intersection with a plane.
	/// </summary>
	public GameObject spawnedObject { get; private set; }
    
    /*void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }*/

    void Start()
    {
        //instantiate cube at the origin
        cam = Camera.main;
        startPosition = lRStart.transform.position;
        finalPosition = lRCube.transform.position;

        raycastManager = ARSessionOrigin.GetComponent<ARRaycastManager>();

        actualCube = Instantiate(m_PlacedCube, finalPosition, new Quaternion(0,0,0,0));    
        
        shadowCube = Instantiate(shadow);

        lR = Instantiate(lRPrefab);
        lRCube.transform.LookAt(cam.transform);
        diff = Time.deltaTime;
    }

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
            return true;
        }

        touchPosition = default;
        return false;
    }

    // Update is called once per frame
    void Update() 
    {
    	startPosition = lRStart.transform.position;
        finalPosition = lRCube.transform.position;
        actualCube.transform.position =  Vector3.SmoothDamp(actualCube.transform.position, finalPosition, ref velocity, smoothTime);
        DrawCurve(startPosition, actualCube.transform.position);

        /*if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;*/

        //Ray ray = new Ray(lRCube.transform.position, -1.0f * lRCube.transform.up);
        Ray ray = new Ray(actualCube.transform.position, -1.0f * actualCube.transform.up);

        if (raycastManager.Raycast(ray, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            //Debug.Log("raycaster hit");
            Pose hit = s_Hits[0].pose;

            shadowCube.transform.position = hit.position;
            shadowCube.transform.rotation = hit.rotation;
        }
    }

    //Code implementation courtesy of: https://www.gamasutra.com/blogs/VivekTank/20180806/323709/How_to_work_with_Bezier_Curve_in_Games_with_Unity.php
    private void DrawCurve(Vector3 start, Vector3 end)
    {
        Vector3 point0 = start;
        Vector3 point1 = Vector3.Lerp(start, end, 0.5f);
        point1 = point1 + velocity / 4;
        Vector3 point2 = end;
        float t;

        Vector3 term1;
        Vector3 term2;
        Vector3 term3;

        Vector3 position;
        lR.positionCount = bezierCurvePoints;

        for (int i = 0; i < bezierCurvePoints; i++) {
            t = i / (bezierCurvePoints - 1.0f);
            //t = i / bezierCurvePoints;
            term1 = point0 * Mathf.Pow((1.0f - t), 2);
            term2 = point1 * 2.0f * (1.0f - t) * t;
            term3 = point2 * Mathf.Pow(t, 2);

            position = term1 + term2 + term3;
            lR.SetPosition(i, position);
        }
    }

    public void UndoFunctionality() {
        if (cubes.Count == 0) {
            lR2.positionCount = 0;
            isLine = false;
            return;
        }

        GameObject delCube = cubes[cubes.Count - 1];
        Destroy(delCube);
        cubes.RemoveAt(cubes.Count - 1);

        if (distanceTexts.Count > 0) {
            GameObject delText = distanceTexts[distanceTexts.Count - 1];
            distanceTexts.RemoveAt(distanceTexts.Count - 1);
            Destroy(delText);
        }

        if (cubes.Count == 0) {
            lR2.positionCount = 0;
            isLine = false;
            return;
        }

        if (lR2.positionCount > 0) {
            lR2.positionCount = lR2.positionCount - 1;
        }
    }

    public void ResetFunctionality() {
        for (int i = cubes.Count - 1; i >= 0; i--)
        {   
            GameObject delCube = cubes[i];
            cubes.RemoveAt(i);
            Destroy(delCube);
        }

        for (int i = distanceTexts.Count - 1; i >= 0; i--) {
            GameObject delText = distanceTexts[i];
            distanceTexts.RemoveAt(i);
            Destroy(delText);
        }

        lR2.positionCount = 0;
        isLine = false;
    }

	public void PlaceFunctionality() {
		spawnedObject = (GameObject) Instantiate(m_PlacedCube, lRCube.transform.position, new Quaternion(0,0,0,0));
		cubes.Add(spawnedObject);
		UpdateLineRenderer();
	}

	void UpdateLineRenderer() {
    	if (cubes.Count > 1) {
    		lastCube = cubes[cubes.Count - 1];
    		prevCube = cubes[cubes.Count - 2];

    		if (!isLine) {
    			lR2 = Instantiate(lRPrefab);
				isLine = true;
				lR2.SetPosition(0, prevCube.transform.position);
                lR2.positionCount = 2;
				StartCoroutine(DrawLine());
			}

            else {
                lR2.positionCount = lR2.positionCount + 1;
                StartCoroutine(DrawLine());
            }
		}
    }

    IEnumerator DrawLine() {
		float start = 0f;
		float totalTime = 3.5f;

		while (start <= totalTime && lR2.positionCount > 0 && cubes.Count >= 2) {
			start = start + diff;
            lR2.SetPosition(lR2.positionCount - 1, Vector3.Lerp(cubes[cubes.Count - 2].transform.position, cubes[cubes.Count - 1].transform.position, start / totalTime));
			yield return null;
		}

        DrawTextDistance();
	}

	public void changeSlider() {
       float value = slider.value;
       float newFinalPosition = cubePosZ - value;
       lRCube.transform.position = new Vector3(lRCube.transform.position.x, lRCube.transform.position.y, newFinalPosition);
   	}

    void DrawTextDistance() {
        if (cubes.Count >= 2) {
            distanceCalculation = Vector3.Distance(cubes[cubes.Count - 2].transform.position, cubes[cubes.Count - 1].transform.position);
        }
        else {
            return;
        }
        
        GameObject dist = (GameObject) Instantiate(distanceCalc);
        dist.transform.rotation = cam.transform.rotation;
        dist.transform.position = (cubes[cubes.Count - 2].transform.position + cubes[cubes.Count - 1].transform.position) / 2.0f;
        TextMesh myText = dist.GetComponent<TextMesh>();
        myText.text = distanceCalculation.ToString("#0.00") + 'm';

        distanceTexts.Add(dist);
    }
}
