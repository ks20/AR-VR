using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController_Part1 : MonoBehaviour
{
    public GameObject ARSessionOrigin;
    public GameObject cubePrefab;
    public LineRenderer lRPrefab;
    public GameObject distanceCalc;

    private PlaceMultipleObjectsOnPlane scriptRef;
    private List<GameObject> cubes;
    public List<GameObject> distanceTexts;

    private GameObject lastCube;
    private GameObject prevCube;
    private Camera cam;

    bool isLine = false;
    bool isLerping = false;
    float delay = 0;
    float distanceCalculation;
    LineRenderer lR;
    
    //graphics ray cast and physics ray cast
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        scriptRef = ARSessionOrigin.AddComponent<PlaceMultipleObjectsOnPlane>();
        scriptRef.placedPrefab = cubePrefab;
        cubes = scriptRef.cubes;
    }

    // Update is called once per frame
    void Update()
    {
    	UpdateLineRenderer();
    }

    void UpdateLineRenderer() {
        if (cubes.Count > 1) {
            lastCube = cubes[cubes.Count - 1];
            prevCube = cubes[cubes.Count - 2];

            if (!isLine) {
                lR = Instantiate(lRPrefab);
                isLine = true;
                lR.SetPosition(0, prevCube.transform.position);
                lR.positionCount = 2;
                StartCoroutine(DrawLine());
            }

            else {
                lR.positionCount = lR.positionCount + 1;
                StartCoroutine(DrawLine());
            }
        }
    }

    //delete texts
    IEnumerator DrawLine() {
        float journey = 0f;
        float duration = 3.5f;

        while (journey <= duration && lR.positionCount > 0 && cubes.Count >= 2) {
            journey = journey + Time.deltaTime;
            lR.SetPosition(lR.positionCount - 1, Vector3.Lerp(cubes[cubes.Count - 2].transform.position, cubes[cubes.Count - 1].transform.position, journey / duration));
            yield return null;
        }

        DrawTextDistance();
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

	public void UndoFunctionality() {
        /*if (!isLine) {
            return;
        }*/

        if (cubes.Count == 0) {
            if (isLine) {
                lR.positionCount = 0;
                isLine = false;
                return;
            }
        }

        GameObject delCube = cubes[cubes.Count - 1];
        Destroy(delCube);
        cubes.RemoveAt(cubes.Count - 1);

        if (distanceTexts.Count > 0) {
            GameObject delText = distanceTexts[distanceTexts.Count - 1];
            distanceTexts.RemoveAt(distanceTexts.Count - 1);
            Destroy(delText);
        }

        /*if (cubes.Count == 0) {
            lR.positionCount = 0;
            isLine = false;
            return;
        }*/

        if (lR.positionCount > 0) {
            lR.positionCount = lR.positionCount - 1;
        }
    }

	public void ResetFunctionality() {
		/*if (!isLine) {
            return;
        }*/

        for (int i = cubes.Count - 1; i >= 0; i--)
        {	
            GameObject delCube = cubes[i];
            Destroy(delCube);
            cubes.RemoveAt(i);
        }

        for (int i = distanceTexts.Count - 1; i >= 0; i--) {
            GameObject delText = distanceTexts[i];
            Destroy(delText);
            distanceTexts.RemoveAt(i);
        }

        lR.positionCount = 0;
        isLine = false;
	}
}
