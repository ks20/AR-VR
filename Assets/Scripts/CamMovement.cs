using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    //The target object that this camera is focosing on
    public GameObject targetObject;

    //A float used to tweak the camera movement
    [SerializeField]
    float cameraMovementSmooth = 1.0f;

    //An offset between camera and the target obejct
    Vector3 offset;

    void Start()
    {
        offset = transform.position - targetObject.transform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetObject.transform.position + offset, cameraMovementSmooth);
    }
}
