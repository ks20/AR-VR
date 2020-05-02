using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using TXPro;

public class StartGoldberg : MonoBehaviour
{
    public Camera camera;
    public TextMeshProUGUI textmeshPro;
    public string standByText;
    public string startedText;
    
    void Start()
    {
    	textmeshPro = FindObjectOfType<TextMeshProUGUI>();
    	standByText = "Welcome!";
    	startedText = "Good luck Kush!";
    	textmeshPro.text = standByText;
    }

    void Update()
    {
    	if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

            if (hit) {
            	Interact(hitInfo);
            	textmeshPro.text = startedText;
            }
        }
        
    }

    void Interact(RaycastHit hit) {
    	if (hit.collider.transform.name == "StartPlatform") {
    		hit.transform.gameObject.SetActive(false);
    	}

    }
}
