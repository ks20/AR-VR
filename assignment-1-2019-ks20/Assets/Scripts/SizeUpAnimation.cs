using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeUpAnimation : MonoBehaviour
{
    public GameObject dominoPrefab;

    void Start()
    {
    	StartCoroutine(ScaleOverTime(1));
    }

    void Update() {}

    IEnumerator ScaleOverTime(float time)
     {
         Vector3 originalScale = new Vector3(0.0f, 0.0f, 0.0f);
         Vector3 destinationScale = dominoPrefab.transform.localScale;
         
         float currentTime = 0.0f;
         
         do
         {
             dominoPrefab.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
             currentTime = currentTime + Time.deltaTime;
             yield return null;
         } while (currentTime <= time);
     }
}
