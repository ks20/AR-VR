using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goodbye : MonoBehaviour
{
    public GameObject tCube;
    private Vector3 originalPosition;
    
    void Start()
    {
        originalPosition = tCube.transform.position;
    }

    void Update()
    {
        if (tCube.transform.position != originalPosition) {
            Invoke("SwitchScene", 6);
        }
    }

    void SwitchScene() {
         SceneManager.LoadScene("Demo_2");
    }
}
