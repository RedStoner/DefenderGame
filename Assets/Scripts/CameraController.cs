using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraController : NetworkBehaviour {
    public int scrollSpeed = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) {
            return;
        }
        float xAxisValue = Input.GetAxis("Horizontal");
        float zAxisValue = Input.GetAxis("Vertical");
        float zoomValue = Input.GetAxis("Mouse ScrollWheel");
        if (Camera.main != null)
        {
            Camera.main.transform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue), Space.World);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Camera.main.transform.Translate(new Vector3(0.0f, 0.0f, zoomValue * scrollSpeed*3));
            } else
            {
                Camera.main.transform.Translate(new Vector3(0.0f, 0.0f, zoomValue * scrollSpeed));
            }
            
        }
    }
}
