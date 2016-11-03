using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public AudioSource robotMovement;
   
    AudioSource source;
    public bool canMove = true;
    Camera cam;
    bool jumped;
    public float gravityScale;
    public float movementSpeed;
    public float runSpeed;
    public float gravity;
    Vector3 down = new Vector3(0,0,0);
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
    public AudioClip interactClip;
   
    public float jumpSpeed;
    public bool grounded;
    bool running;
    Animator anim;
    public AudioSource[] ambienceObjects;
    public AudioClip[] ambiences;
    public string terrain;
    public AudioClip[] fSteps;
    public AudioClip[] dSteps;
    public AudioClip[] frSteps;
    public AudioClip[] drSteps;
    public AudioClip[] moveSounds;
    public AudioClip[] runSounds;

    // Use this for initialization
    void Start() {
        cam = Camera.main;
        player = GetComponent<CharacterController>();
        source = GetComponent<AudioSource>();
        player.enabled = true;
        anim = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update() {
        
        jumped = false;
        running = Running();
      
        if (interactable) {
            if (Input.GetButtonDown("Interact"))
                {
                    interactable.GetComponent<Interactable>().Interact();
                source.clip = interactClip;
                source.Play();
                }
        }
      
        yBackup = move.y;
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
       
        /* if (oldInput.magnitude < deadThreshold && input.magnitude > deadThreshold) {
             Destroy(GameObject.Find("CameraPlaceholder(Clone)"));
             GameObject copy = (GameObject) Instantiate(camPlaceholder, cam.transform.position, cam.transform.rotation);
             camCopy = copy.transform;
         }*/
       
        ThresholdMove(input, inputThreshold);
        move = cam.transform.TransformDirection(input);
      /*  if (camCopy)
        {
            move = camCopy.TransformDirection(input);
        }*/
        ThresholdMove(move, moveThreshold);
        move.x += Mathf.Abs(move.y) * (move.x / (Mathf.Abs(move.x) + Mathf.Abs(move.z)));
        move.z += Mathf.Abs(move.y) * (move.z / (Mathf.Abs(move.x) + Mathf.Abs(move.z)));
        move.y = yBackup;
        if (float.IsNaN(move.x))
        {
            move.x = 0;
        }
        if (float.IsNaN(move.z))
        {
            move.z = 0;
        }
        if (Input.GetButtonDown("Jump") && player.isGrounded)
        {
            jumped = true;
            move.y = jumpSpeed;
        }
        if (move.magnitude > lookThreshold)
        {
            transform.LookAt(transform.position + new Vector3(move.x, 0, move.z));
        }
        if (canMove)
        {
            yBackup = move.y;
            down.y = move.y;
            move.y = 0;
            if (!running)
            {
                player.Move(move * movementSpeed * Time.deltaTime);
            }
            else {
                player.Move(move * runSpeed * Time.deltaTime);
            }
            player.Move(down * Time.deltaTime * gravityScale);
            move.y = yBackup;
        }
      
        
            move.y -= gravity;

        if (input.magnitude > 0.05f && !source.isPlaying && player.isGrounded) {
            if (!running)
            {
                if (terrain == "Grass")
                {
                    source.clip = fSteps[(int)Random.Range(0, fSteps.Length)];
                    source.Play();
                }
                else if (terrain == "Sand")
                {
                    source.clip = dSteps[(int)Random.Range(0, dSteps.Length)];
                    source.Play();
                }
                robotMovement.clip = moveSounds[(int)Random.Range(0, moveSounds.Length)];
                robotMovement.Play();
            }
           else
            {
                if (terrain == "Grass")
                {
                    source.clip = frSteps[(int)Random.Range(0, frSteps.Length)];
                    source.Play();
                }
                else if (terrain == "Sand")
                {
                    source.clip = drSteps[(int)Random.Range(0, drSteps.Length)];
                    source.Play();
                }
                robotMovement.clip = runSounds[(int)Random.Range(0, runSounds.Length)];
                robotMovement.Play();
            }
           
        }
        oldInput = input;
        grounded = player.isGrounded;
        anim.SetBool("Running", running);
        anim.SetBool("Jumping", jumped);
        anim.SetFloat("Vel", input.magnitude);
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

    void OnControllerColliderHit(ControllerColliderHit hit) {

        if (hit.gameObject.tag == "Grass") {
           
            if (!(terrain == "Grass")) {
                terrain = "Grass";
                for (int i = 0; i < ambienceObjects.Length; i++) {
                    ambienceObjects[i].clip = ambiences[0];
                    ambienceObjects[i].Play();
                }
            }
        }
        else if (hit.gameObject.tag == "Sand") {
            if (!(terrain == "Sand"))
            {
                terrain = "Sand";
                for (int i = 0; i < ambienceObjects.Length; i++)
                {
                    ambienceObjects[i].clip = ambiences[1];
                    ambienceObjects[i].Play();
                }
            }
        }
               
               
             

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
    bool Running() {
        return Input.GetButton("Run");
    }
   /* void OnTriggerStay(Collider other) {
        if (other.tag == "Interactable") {
            if (Input.GetButtonDown("Interact")) {
                other.GetComponent<Interactable>().Interact();
            }
        }
    }*/
}
