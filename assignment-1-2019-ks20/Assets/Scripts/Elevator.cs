using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    //Elevator cube 1-4 that you want to control in this script
    public GameObject[] elevators;

    //A cube you want to use its position y as reference to the elevators
    public Transform referenceTransform;

    //The position y of the reference cube
    float targetYPos;
    
    //A bool to show if the elevator switch has been collided with the ball
    bool alreadyCollided;

    //A float number to tweak the movement speed of the elevators in the editor
    [SerializeField]
    float movementSmooth = 20.0f;
    
    void Start() {}

    void Update()
    {
        if(alreadyCollided)
        {
            foreach (GameObject elevator in elevators)
            {
                MoveUp(elevator);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            alreadyCollided = true;
        }
    }

    void MoveUp(GameObject thisElevator)
    {
        float step =  movementSmooth;
        Vector3 target = new Vector3(thisElevator.transform.position.x, referenceTransform.position.y, thisElevator.transform.position.z);
        thisElevator.transform.position = Vector3.MoveTowards(thisElevator.transform.position, target, step);
    }
}
