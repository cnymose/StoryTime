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
    public AudioSource source;
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
    private string area;
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
    public AudioClip[] soundTracks;
    public AudioSource zap;
    public AudioClip death;
    Coroutine soundRoutine = null;
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
        Cursor.visible = false;
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
        if (move.magnitude > 1) { // Used for long idle animation 
            
            lastMove = Time.time; // Logs last timestamp of movement
        }
        if (Input.GetButtonDown("Pause"))
        {
            Pause(); //Pause the game if we press pause
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            runSpeed = runSpeed < 100 ? 200 : 22; //Internal test purposes
        }
        if (Input.GetKeyDown(KeyCode.U)) { //Internal test purposes
            transform.position += new Vector3(0, 200, 0);
                
                }
        if (Input.GetKeyDown(KeyCode.Y)) //Internal test purposes
        {
            transform.position += Vector3.forward * 50;

        }
        if (Input.GetKeyDown(KeyCode.K)) //Internal test purposes
        {
            keyboard = !keyboard;
            cam.GetComponent<CamScript>().keyboard = !cam.GetComponent<CamScript>().keyboard;

            Cursor.lockState = keyboard == true ? CursorLockMode.Locked : CursorLockMode.None;

        }
        if (!paused) { //If not paused

            sliding = false; //Set sliding down hills to false by default
            running = Running(); //Check if we are holding down the run button

            //Interact
            if (interactable) { //If we are near an interactable object
                if (Input.GetButtonDown("Interact")) //And we press the button to interact with it
                {
                    if (!gameLinear) //If the game is in non-linear mode
                    {
                        interactable.GetComponent<Interactable>().Interact(); //Interact with it and play a sound
                        source.clip = interactClip;
                        source.Play();
                    }
                    else {
                        interactable.GetComponent<LinearInteractable>().Interact(); //Else do the same, just for the linear version
                        source.clip = interactClip;
                        source.Play();
                    }
                }
            }

            if (player.isGrounded) //If the character touches the ground
            {
                
                // See if surface immediately below should be slid down. We use this normally rather than a ControllerColliderHit point,
                // because that interferes with step climbing amongst other annoyances
                if (Physics.Raycast(transform.position, -Vector3.up, out hit, rayDistance))
                {
                    if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit) //If the angle of the terrain we are on is steep enough
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
            //Get different input for movement depending on whether we are on Keyboard or controller

            ThresholdMove(input, inputThreshold); //Threshhold the movement we get from the controller to a certain value
            move = cam.transform.TransformDirection(input); //Rotate the movement from the controller to always be in the direction that the character is facing
            ThresholdMove(move, moveThreshold);
            move.x += Mathf.Abs(move.y) * (move.x / (Mathf.Abs(move.x) + Mathf.Abs(move.z))); //Makes sure that if the camera is tilted, that we don't try to move "into the ground" and lose speed on some occasions.
            move.z += Mathf.Abs(move.y) * (move.z / (Mathf.Abs(move.x) + Mathf.Abs(move.z)));
            move.y = yBackup;
            if (float.IsNaN(move.x)) //Checks if we actually get an input value. Fixes a null-pointer.
            {
                move.x = 0;
            }
            if (float.IsNaN(move.z))
            {
                move.z = 0;
            }

            //Jump statement
            if (Input.GetButtonDown("Jump") && canMove)
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
            if (move.magnitude > lookThreshold && canMove)
            {
                transform.LookAt(transform.position + new Vector3(move.x, 0, move.z)); //Character looks in the direction that the controller is directed.
            }
            yBackup = move.y;
            down.y = move.y;
            move.y = 0;
            if (canMove) //Next 30 lines applies the movement to the character.
            {
                if (sliding)
                {
                    
                    Vector3 hitNormal = hit.normal;
                    move = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                    Vector3.OrthoNormalize(ref hitNormal, ref move);
                    move *= slideSpeed;
                }
               

                    if (!running)
                    {
                        player.Move(move * movementSpeed * Time.deltaTime);
                    }
                    else
                    {
                        player.Move(move * runSpeed * Time.deltaTime);
                    }
                   
                
            }
            player.Move(down * gravityScale * Time.deltaTime);
            move.y = yBackup;
        }
        if (!player.isGrounded)
        {
            move.y -= gravity * Time.deltaTime;
        }
        else {
            doubleJumped = false;
            jumped = false;
        }

        //Audio on different types of terrain
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
        //Finish the Update by setting some booleans and animation states.
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

    Vector3 ThresholdMove(Vector3 input, float threshold) { //Thresholds a value to zero if too low.
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

    IEnumerator Landed() { //Turn on that we landed for a few seconds for animation purposes.
        landed = true;
        yield return new WaitForSeconds(0.2f);
        landed = false;
        yield break;
    }
  

    IEnumerator FadeFogIn() //Fades the fog in when we enter the desert
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
    IEnumerator FadeFogOut() { //Fades the fog out.
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

    void CheckLand() { //Screenshakes when we land after having jumped or fallen.

        if (!grounded && source.clip.name == "Jump")
        {
            source.clip = land;
            source.pitch = Random.Range(0.9f, 1.05f);
            source.Play();
            StartCoroutine(ScreenShake());
        }
    }

    IEnumerator ScreenShake() { //Shakes the screen, using a filter. 
        screenShake.enabled = true;
        screenShake.Speed = Random.Range(40, 60);
        screenShake.X = Random.Range(.004f, .01f);
        screenShake.Y = Random.Range(.004f, .01f);
        yield return new WaitForSeconds(Random.Range(0.25f, 0.6f));
        screenShake.enabled = false;
        yield break;
    }


    IEnumerator FadeText(string text) //Fades the text on screen in and out when transferring between areas.
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

    IEnumerator CrossFadeSound(float volume, float time, int track) { //Crossfades two sounds,  to switch music and ambience across different game areas.
       
        while (Soundtrack.volume > 0) {
            if (Soundtrack.volume < 0.03f) {
                Soundtrack.volume = 0;
            }
            Soundtrack.volume -= volume/(time * 30);
            yield return new WaitForSeconds(0.0166f);
        }
        Soundtrack.clip = soundTracks[track];
        
        Soundtrack.Play();
        while (Soundtrack.volume < volume)
        {
            Soundtrack.volume += volume / (time * 60);
            yield return new WaitForSeconds(0.0166f);
        }
        yield break;
    }


    void OnTriggerEnter(Collider other) //When we collide with colliders marked as triggers. 
    {

        print("Entered");
        if (other.gameObject.tag == "EventTrigger") //If we collide with an event-spawner, the event will be generated at its designated location.
        {
            EventSpawnTrigger trigger = other.gameObject.GetComponent<EventSpawnTrigger>();
            if (!trigger.triggered)
            { //If we haven't triggered this collider before
                trigger.triggered = true;
                print("Spawned an Event");
                trigger.Spawn();
            }
        }
        else if (other.tag == "Interactable" || other.tag == "Collectible") //If we approach with an event, or a collectible object.
        {

            bButton.SetActive(true); //Show the B-button tooltip on the screen and be ready to interact. See rest in Update function
            interactText.SetActive(true);
            interactable = other.gameObject;
        }

        if (other.tag == "Grass" && other.tag != terrain) //If we land on grass
        {
            StartCoroutine(FadeFogOut());
            ChangeSounds(0, 0, other.tag);
        }
        else if (other.tag == "Sand" && other.tag != terrain) //If we land on Sand
        {
            StartCoroutine(FadeFogIn());
            ChangeSounds(1, 1, other.tag);
            
        }
        else if (other.tag == "Church" && other.tag != terrain) //If we enter the church
        {

            ChangeSounds(2, 2, other.tag);
            

        }
        else if (other.tag == "Bunker" && other.tag != terrain) { //If we enter a bunker
            StartCoroutine(FadeFogOut());
            ChangeSounds(3,3,other.tag);
            
        }


        if (other.GetComponent<TextCollider>()) //If we change area in general.
        {
            if (area != other.GetComponent<TextCollider>().text) {
                area = other.GetComponent<TextCollider>().text;
                zap.Play();
            StartCoroutine(FadeText(other.GetComponent<TextCollider>().text)); //Run the function that fades the text of the area in and out. 
            if (other.GetComponent<TextCollider>().destroy)
            {
                Destroy(other.gameObject, 2);
            }
        }
        }
    }
    void ChangeSounds(int amb, int music, string tag) { //Another function that helps us change sound across areas. 
        if (soundRoutine != null)
        {
            StopCoroutine(soundRoutine);
        }
        terrain = tag;
        soundRoutine = StartCoroutine(CrossFadeSound(0.3f, 1.2f, music));
        ambienceObject.clip = ambiences[amb];
        ambienceObject.Play();

    }
    void OnTriggerExit(Collider other) //When leaving an interactable collider, we hide tooltips etc. 
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

    bool Running() { //Checks if we press the run button
        return Input.GetButton("Run");
    }
    public void Pause() { //Pauses the game
        paused = !paused;
        menuController.StartDetecting(paused);
        pauseUI.SetActive(!pauseUI.activeInHierarchy);
    }
    public void UpdateCollectibles() { //Update score in corner of screen.
        collectibles++;
        StartCoroutine(CollectibleTick());
    }

    IEnumerator CollectibleTick() { //Actually updates collectibles score.
        collectibleText.gameObject.SetActive(true);
        collectibleText.text = collectibles + "/8";
        yield return new WaitForSeconds(7.5f);
        collectibleText.gameObject.SetActive(false);
        yield break;
    }
}
