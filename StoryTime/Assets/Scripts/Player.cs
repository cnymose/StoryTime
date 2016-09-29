using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    Camera cam;
    public float movementSpeed;
    public float gravity;
    public Vector3 move;
    CharacterController player;
    public float lookThreshold;
    public float moveThreshold;
    public float inputThreshold;
    public float deadThreshold;
    float yBackup = 0;
    public Vector3 input;
    Transform camCopy;
    Vector3 oldInput;
    public GameObject camPlaceholder;
    // Use this for initialization
    void Start() {
        cam = Camera.main;
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        yBackup = move.y;
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (oldInput.magnitude < deadThreshold && input.magnitude > deadThreshold) {
            Destroy(GameObject.Find("CameraPlaceholder(Clone)"));
            GameObject copy = (GameObject) Instantiate(camPlaceholder, cam.transform.position, cam.transform.rotation);
            camCopy = copy.transform;
        }
        ThresholdMove(input, inputThreshold);
        if (camCopy)
        {
            move = camCopy.TransformDirection(input);
        }
        ThresholdMove(move, moveThreshold);
        move.x += Mathf.Abs(move.y) * (move.x / (Mathf.Abs(move.x) + Mathf.Abs(move.z)));
        move.z += Mathf.Abs(move.y) * (move.z / (Mathf.Abs(move.x) + Mathf.Abs(move.z)));
        move.y = yBackup;
        if (move.magnitude > lookThreshold)
        {
            transform.LookAt(transform.position + new Vector3(move.x, 0, move.z));
        }
        player.Move(move * movementSpeed * Time.deltaTime);
        if (!player.isGrounded)
        {
            move.y -= gravity;
        }
        else {
            move.y = 0;
        }
        oldInput = input;
    }

    Vector3 ThresholdMove(Vector3 input, float threshold) {
        if (Mathf.Abs(input.x) < threshold)
        {
            input.x = 0;
        }
        if (Mathf.Abs(input.z) < threshold)
        {
            input.z = 0;
        }
        return input;

    }
}
