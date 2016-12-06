using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Player : MonoBehaviour {
    public int collectibles = 0;
    public UnityEngine.UI.Text areaText;
    public UnityEngine.UI.Image collectibleImage;
    public GameObject collectibleBG;
    public UnityEngine.UI.Text collectibleText;
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
    bool landed;
    public float jumpSpeed;
    public bool grounded;
    bool running;
    Animator anim;
    public AudioSource ambienceObject;
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
    public AudioSource Soundtrack;
    public AudioClip ForestTrack;
    public AudioClip DesertTrack;
    public bool paused;
    public GameObject pauseUI;
    public MenuController menuController;

    //
    private Vector3 contactPoint;
    private RaycastHit hit;
    public  float rayDistance = 1;
    public float slideLimit = 50;
    public float slideSpeed;
    bool sliding = false;
    public bool gameLinear;
    float lastMove;
    Vector3 oldPos;

    // Use this for initialization
    void Start() {
        fog = Camera.main.GetComponent<CameraFilterPack_Atmosphere_Fog>();
        blizzard = Camera.main.GetComponent<CameraFilterPack_Blizzard>();
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

            sliding = false;
            running = Running();

            //Interact
            if (interactable) {
                if (Input.GetButtonDown("Interact"))
                {
                    if (!gameLinear)
                    {
                        interactable.GetComponent<Interactable>().Interact();
                        source.clip = interactClip;
                        source.Play();
                    }
                    else {
                        interactable.GetComponent<LinearInteractable>().Interact();
                    }
                }
            }

            if (player.isGrounded)
            {
                
                // See if surface immediately below should be slid down. We use this normally rather than a ControllerColliderHit point,
                // because that interferes with step climbing amongst other annoyances
                if (Physics.Raycast(transform.position, -Vector3.up, out hit, rayDistance))
                {
                    if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                        sliding = true;
                    
                }
                // However, just raycasting straight down from the center can fail when on steep slopes
                // So if the above raycast didn't catch anything, raycast down from the stored ControllerColliderHit point instead
                else
                {
                    Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit);
                    if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
                        sliding = true;
                    
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
                if (player.isGrounded && !sliding)
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
                if (sliding)
                {
                    
                    Vector3 hitNormal = hit.normal;
                    move = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                    Vector3.OrthoNormalize(ref hitNormal, ref move);
                    move *= slideSpeed;
                }
               
                    yBackup = move.y;
                    down.y = move.y;
                    move.y = 0;
                    if (!running)
                    {
                        player.Move(move * movementSpeed * Time.deltaTime);
                    }
                    else
                    {
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
        else {
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
                else
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
                else
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
        anim.SetBool("Landing", landed);
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
        contactPoint = hit.point;
        if (!grounded)
        {
            StartCoroutine(Landed());
        }
            CheckLand();      
                  }

    IEnumerator Landed() {
        landed = true;
        yield return new WaitForSeconds(0.2f);
        landed = false;
        yield break;
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


    IEnumerator FadeText(string text)
    {
        areaText.gameObject.SetActive(true);
        UnityEngine.UI.Image bar = areaText.GetComponentInChildren<UnityEngine.UI.Image>();
        areaText.color = new Color(areaText.color.r, areaText.color.b, areaText.color.g, 0);
        bar.color = new Color(bar.color.r, bar.color.b, bar.color.g, 0);
        areaText.text = text;
        Color colorAdd = new Color(0, 0, 0, 0.02f); 
        while (areaText.color.a < 1) {
            bar.color += colorAdd;
            areaText.color += colorAdd;
            yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(5);
        while (areaText.color.a > 0)
        {
            bar.color -= colorAdd;
            areaText.color -= colorAdd;
            yield return new WaitForSeconds(0.03f);
        }
        areaText.gameObject.SetActive(false);
        yield break;
    }

    IEnumerator CrossFadeSound(float volume, float time, bool forest) {
        print("STARTED THE FUCKING COROUTINE");
        while (Soundtrack.volume > 0) {
            Soundtrack.volume -= volume/(time * 30);
            yield return new WaitForSeconds(0.0166f);
        }
        Soundtrack.clip = forest ? ForestTrack : DesertTrack;
        
        Soundtrack.Play();
        while (Soundtrack.volume < volume)
        {
            Soundtrack.volume += volume / (time * 60);
            yield return new WaitForSeconds(0.0166f);
        }
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
        else if (other.tag == "Interactable" || other.tag == "Collectible")
        {

            bButton.SetActive(true);
            interactText.SetActive(true);
            interactable = other.gameObject;
        }

        if (other.gameObject.tag == "Grass")
        {
            if (!(terrain == "Grass"))
            {
                print("Switched to ´forest");
                StartCoroutine(CrossFadeSound(Soundtrack.volume, 1.2f, true));
                StartCoroutine(FadeFogOut());
                terrain = "Grass";
                ambienceObject.clip = ambiences[0];
                ambienceObject.Play();
                
            }
        }
        else if (other.gameObject.tag == "Sand")
        {
                   
            if (!(terrain == "Sand"))
            {
                print("Switched to sand");
                StartCoroutine(CrossFadeSound(Soundtrack.volume, 1.2f, false));
                StartCoroutine(FadeFogIn());
                terrain = "Sand";
                ambienceObject.clip = ambiences[1];
                ambienceObject.Play();
                
            }
        }
        if (other.gameObject.tag == "TextCollider") {
            StartCoroutine(FadeText(other.GetComponent<TextCollider>().text));
            if (other.GetComponent<TextCollider>().destroy) {
                Destroy(other.gameObject, 2);
            }
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
