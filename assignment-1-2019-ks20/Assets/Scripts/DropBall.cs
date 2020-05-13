using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DropBall : MonoBehaviour
{
    public GameObject playerCube;
    public GameObject spherePrefab;
    public GameObject playerBall;
    public Rigidbody rb;

    void Start()
    {
    	rb = GetComponent<Rigidbody>();
        playerBall.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Transformation"))
        {
            Invoke("Transform", 0.15f);
        }
    }

    void Transform() {
        playerBall.SetActive(true);
        playerBall.transform.position = playerCube.transform.position;
        playerBall.transform.rotation = playerCube.transform.rotation;
        Rigidbody temp = playerBall.GetComponent<Rigidbody>();
        temp.useGravity = true;
        Destroy(playerCube);
    }

    void Update() {}

    void OnMouseDown()
    {
        rb.useGravity = true;
    }
}
