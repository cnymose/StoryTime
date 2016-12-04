using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Player : MonoBehaviour {
    public int collectibles = 0;
    public UnityEngine.UI.Image collectibleImage;
    public GameObject collectibleBG;
    UnityEngine.UI.Text collectibleText;
    public AudioSource robotMovement;
    CameraFilterPack_FX_EarthQuake screenShake;
    CameraFilterPack_Atmosphere_Fog fog;
    CameraFilterPack_Blizzard blizzard;
    AudioSource source;
    public bool canMove = true;
    Camera cam;
   public bool jumped;
   public bool doubleJumped;
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
    bool keyboard = false;
    
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
    public AudioClip jump;
    public AudioClip land;
    public bool paused;
    public GameObject pauseUI;
    public MenuController menuController;
    
    float lastMove;
    Vector3 oldPos;

    // Use this for initialization
    void Start() {
        fog = Camera.main.GetComponent<CameraFilterPack_Atmosphere_Fog>();

        blizzard = Camera.main.GetComponent<CameraFilterPack_Blizzard>();
        collectibleImage = GameObject.Find("CollectibleImage").GetComponent<UnityEngine.UI.Image>();
        collectibleBG = GameObject.Find("CollectibleBG");
        collectibleBG.SetActive(false);
        collectibleText = GameObject.Find("CollectibleCounter").GetComponent<UnityEngine.UI.Text>();
        collectibleText.gameObject.SetActive(false);
        screenShake = Camera.main.GetComponent<CameraFilterPack_FX_EarthQuake>();
        cam = Camera.main;
        player = GetComponent<CharacterController>();
        source = GetComponent<AudioSource>();
        source.clip = land;
        player.enabled = true;
        anim = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update() {
        if (move.magnitude > 1) {
            
            lastMove = Time.time;
        }
        if (Input.GetButtonDown("Pause"))
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            keyboard = !keyboard;
            cam.GetComponent<CamScript>().keyboard = !cam.GetComponent<CamScript>().keyboard;

            Cursor.lockState = keyboard == true ? CursorLockMode.Locked : CursorLockMode.None;

        }
        if (!paused) {
            
            
            running = Running();

            //Interact
            if (interactable) {
                if (Input.GetButtonDown("Interact"))
                {
                    interactable.GetComponent<Interactable>().Interact();
                    source.clip = interactClip;
                    source.Play();
                }
            }

            yBackup = move.y;
            input = keyboard == true ? new Vector3(Input.GetAxis("HorKey"), 0, Input.GetAxis("VertKey")) : new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));



            ThresholdMove(input, inputThreshold);
            move = cam.transform.TransformDirection(input);

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

            //Jump
            if (Input.GetButtonDown("Jump"))
            {
                if (player.isGrounded)
                {
                    source.clip = jump;
                    source.pitch = Random.Range(0.9f, 1.05f);
                    source.Play();
                    jumped = true;
                    move.y = jumpSpeed;
                }
                else if(!doubleJumped) {
                    doubleJumped = true;
                    move.y = jumpSpeed;
                              }
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
                player.Move(down * gravityScale * Time.deltaTime);
                move.y = yBackup;
            }
        }
        if (!player.isGrounded)
        {
            move.y -= gravity * Time.deltaTime;
        }
        if (player.isGrounded) {
            doubleJumped = false;
            jumped = false;
        }

        //Audio on terrain
        if (input.magnitude > 0.05f && !source.isPlaying && player.isGrounded) {
            source.pitch = 1;
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
        anim.SetBool("DoubleJumped", doubleJumped);
        anim.SetBool("Idle", Time.time > lastMove + 10);
        anim.SetBool("Running", running);
        anim.SetBool("Jumping", jumped);
        anim.SetFloat("Vel", input.magnitude);
        oldPos = transform.position;
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

    void OnControllerColliderHit(ControllerColliderHit hit) { //When the character lands on different terrain
 
        if (hit.gameObject.tag == "Grass") {
            CheckLand();
            if (!(terrain == "Grass")) {
                StartCoroutine(FadeFogOut());
                terrain = "Grass";
                for (int i = 0; i < ambienceObjects.Length; i++) {
                   
                    ambienceObjects[i].clip = ambiences[0];
                    ambienceObjects[i].Play();
                }
            }
        }
        else if (hit.gameObject.tag == "Sand") {
            CheckLand();
            if (!(terrain == "Sand"))
            {
               
                StartCoroutine(FadeFogIn());
                terrain = "Sand";
                for (int i = 0; i < ambienceObjects.Length; i++)
                {
                    ambienceObjects[i].clip = ambiences[1];
                    ambienceObjects[i].Play();
                }
            }
        }
               
               
        

        }
    IEnumerator FadeFogIn()
    {
        blizzard.enabled = true;
        fog.enabled = true;
        float fogVal = 0.75f;
        float blizzVal = 0.2f;

        fog.Fade = 0;
        blizzard._Fade = 0;
        for (int i = 0; i < 20; i++)
        {
            fog.Fade += (fogVal/20) ;
            blizzard._Fade += (blizzVal/20);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }
    IEnumerator FadeFogOut() {
        float fogVal = 0.75f;
        float blizzVal = 0.2f;
        fog.Fade = fogVal;
        blizzard._Fade = blizzVal;
        for (int i = 0; i < 20; i++)
        {
            fog.Fade -= (fogVal / 20);
            blizzard._Fade -= (blizzVal / 20);
            yield return new WaitForSeconds(0.1f);
        }
        blizzard.enabled = false;
        fog.enabled = false;
        yield break;
    }

    void CheckLand() {

        if (!grounded && source.clip.name == "Jump")
        {
            source.clip = land;
            source.pitch = Random.Range(0.9f, 1.05f);
            source.Play();
            StartCoroutine(ScreenShake());
        }
    }

    IEnumerator ScreenShake() {
        screenShake.enabled = true;
        screenShake.Speed = Random.Range(40, 60);
        screenShake.X = Random.Range(.004f, .01f);
        screenShake.Y = Random.Range(.004f, .01f);
        yield return new WaitForSeconds(Random.Range(0.25f, 0.6f));
        screenShake.enabled = false;
        yield break;
    }
    void OnTriggerEnter(Collider other)
    {

        print("Entered");
        if (other.gameObject.tag == "EventTrigger")
        {
            EventSpawnTrigger trigger = other.gameObject.GetComponent<EventSpawnTrigger>();
            if (!trigger.triggered)
            { //If we haven't triggered this collider before
                trigger.triggered = true;
                print("Spawned an Event");
                trigger.Spawn();
            }
        }
        else if (other.tag == "Interactable" || other.tag == "Collectible") {
            
            bButton.SetActive(true);
            interactText.SetActive(true);
            interactable = other.gameObject;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable")
        {
            ExitInteract();
        }
    }
    public void ExitInteract() {
        interactText.SetActive(false);
        bButton.SetActive(false);
        interactable = null;
    }

    bool Running() {
        return Input.GetButton("Run");
    }
    public void Pause() {
        paused = !paused;
        //Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        menuController.StartDetecting(paused);
        pauseUI.SetActive(!pauseUI.activeInHierarchy);
    }
    public void UpdateCollectibles() { //Update score in corner of screen.
        collectibles++;
        StartCoroutine(CollectibleTick());
    }

    IEnumerator CollectibleTick() { //Actually updates collectibles score.
        collectibleText.gameObject.SetActive(true);
        collectibleText.text = collectibles + "/??";
        yield return new WaitForSeconds(7.5f);
        collectibleText.gameObject.SetActive(false);
        yield break;
    }



   /* void OnTriggerStay(Collider other) {
        if (other.tag == "Interactable") {
            if (Input.GetButtonDown("Interact")) {
                other.GetComponent<Interactable>().Interact();
            }
        }
    }*/
}
