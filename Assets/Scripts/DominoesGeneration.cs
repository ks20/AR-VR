using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DominoesGeneration : MonoBehaviour
{
    public Transform platformTransform;
    bool alreadyCollided;

    void Start() {}

    void Update() {}

    IEnumerator GenerateDomino(float time)
     {
         for (int i = 0; i < 5; i++) {
            GameObject gObj = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Domino.prefab", typeof(GameObject));
            GameObject tempDomino = (GameObject) Instantiate(gObj, new Vector3(7, -9, 10 - (2*i)), Quaternion.identity);
         	tempDomino.transform.eulerAngles = new Vector3(0, 90, 0);
         }
         yield return null;
     }

     private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            StartCoroutine(GenerateDomino(1));
        }
    }
}
