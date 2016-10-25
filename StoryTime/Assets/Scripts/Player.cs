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
    public GameObject bButton;
    public GameObject interactText;
    public GameObject interactable;
    // Use this for initialization
    void Start() {
        cam = Camera.main;
        player = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        if (interactable) {
            if (Input.GetButtonDown("Interact"))
                {
                    interactable.GetComponent<Interactable>().Interact();
                }
        }
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

    void OnTriggerEnter(Collider other)
    {
        print("Entered");
        if (other.tag == "Interactable") {
            
            bButton.SetActive(true);
            interactText.SetActive(true);
            interactable = other.gameObject;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable")
        {
            interactText.SetActive(false);
            bButton.SetActive(false);
            interactable = null;
        }
    }
   /* void OnTriggerStay(Collider other) {
        if (other.tag == "Interactable") {
            if (Input.GetButtonDown("Interact")) {
                other.GetComponent<Interactable>().Interact();
            }
        }
    }*/
}
