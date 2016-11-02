using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {
    public float rotationSpeed;
    public Transform player;
    public float threshold;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("RotationY") > threshold)
        {
            transform.RotateAround(player.position, player.up, rotationSpeed * Input.GetAxis("Horizontal") * Time.deltaTime);
        }
       // transform.RotateAround(player.position, player.right, rotationSpeed * Input.GetAxis("RotationY") * Time.deltaTime);
        transform.LookAt(player.position);


    }
}
