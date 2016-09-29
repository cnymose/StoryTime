using UnityEngine;
using System.Collections;

public class CameraRotation : MonoBehaviour {
    public float rotationSpeed;
    public Transform player;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        transform.RotateAround(player.position, player.up, rotationSpeed * Input.GetAxis("RotationX") * Time.deltaTime);
       // transform.RotateAround(player.position, player.right, rotationSpeed * Input.GetAxis("RotationY") * Time.deltaTime);
        transform.LookAt(player.position);


    }
}
