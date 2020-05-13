using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpinner : MonoBehaviour
{
    public GameObject SpinnerPrefab;
    public Vector3 cubePosition;
    private GameObject spinner;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        spinner = Instantiate(SpinnerPrefab);
    }
}
