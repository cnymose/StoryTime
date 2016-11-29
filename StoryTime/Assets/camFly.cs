using UnityEngine;
using System.Collections;

public class camFly : MonoBehaviour {
    Vector3 move;
    public Vector3 input;
    Camera cam;
	// Use this for initialization
	void Start () {
        cam = Camera.main;
	
	}
	
	// Update is called once per frame
	void Update () {
        move = cam.transform.TransformDirection(input);
        if (Input.GetKeyDown(KeyCode.W))
        {
            
        }
	
	}
}
