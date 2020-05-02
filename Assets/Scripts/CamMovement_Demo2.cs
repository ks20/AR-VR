using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamMovement_Demo2 : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject targetObject2;

    [SerializeField]
    float cameraMovementSmooth = 1.0f;

    Vector3 offset;
    Vector3 offset2;

    void Start()
    {
        offset = transform.position - targetObject.transform.position;
        offset2 = transform.position - targetObject2.transform.position;
    }

    void Update()
    {
        if (targetObject2.transform.position.y < -30) {
            SceneManager.LoadScene("Demo_2");
        }

        if (targetObject != null) {
        	this.transform.position = Vector3.Lerp(this.transform.position, targetObject.transform.position + offset, cameraMovementSmooth);
        }
        else {
            this.transform.position = Vector3.Lerp(this.transform.position, targetObject2.transform.position + offset2, cameraMovementSmooth);
        }
    }
}
