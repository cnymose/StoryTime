using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Interactable : MonoBehaviour {
    UnityEngine.UI.Image collectibleImage;
    GameObject collectibleBG;
    public Sprite collectibleTexture;
    public bool collectible;
    public EventSpawnTrigger trig;
    public int sequence;
    public int type;
    public Movie holo;
    public GameObject textObject;
    public UnityEngine.UI.Text eventText;
    public AudioClip storySound;
    AudioSource source;
    public bool hasInteracted = false;
    EventController controller;
    public GameObject glow;
    Player player;
    public string text;
    UnityEngine.UI.Image black;
    UnityEngine.UI.Image white;
    public PointerScript pointer;
    // Use this for initialization
    void Start () {
        black = GameObject.Find("BlackUI").GetComponent<UnityEngine.UI.Image>();
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player.gameLinear) {
            GetComponent<Interactable>().enabled = false;
        }
        controller = GameObject.Find("EventController").GetComponent<EventController>();
        source = GetComponent<AudioSource>();
       // source.clip = storySound;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Interact() {


        if (!hasInteracted)
        {
            if (collectible)
            {
                
                tag = "Untagged";
                collectibleBG = player.collectibleBG;
                collectibleImage = player.collectibleImage;
                player.UpdateCollectibles();
                player.canMove = false;
                collectibleBG.SetActive(true);
               
                collectibleImage.sprite = collectibleTexture;
                hasInteracted = true;
            }
            else
            {
                trig.doNotSpawn = true;
                if (glow)
                {
                    glow.GetComponent<ParticleSystem>().loop = false;
                }
                sequence--;
                hasInteracted = true;
                pointer.shouldPlay = false;
                pointer.GetComponent<ParticleSystem>().Stop();
                controller.CleanUp();
                Interaction();
                
            }

        }
        else {
            if (collectible)
            {
                player.ExitInteract();
                player.canMove = true;
                collectibleBG.SetActive(false);

                tag = " ";
                GetComponent<Interactable>().enabled = false;
            }
            else {
                Interaction();
            }
        }
        
    
    }

    void Interaction() {

        print("Interacted");
        switch (type)
        {
            case 0:
                holo.PlayMovie();
                break;
            case 1:
                GameObject.Find("Player").GetComponent<Player>().canMove = textObject.activeInHierarchy ? true : false;
                textObject.SetActive(!textObject.activeInHierarchy);
                eventText.text = "A segment of this entity's memory appears to remain intact: \r\n \r\n" + "'" + text + "'";
                break;
            case 2:
                
                StartCoroutine(EndGame());
                break;
        }
    }
    IEnumerator EndGame()
    {
        player.canMove = false;
        yield return new WaitForSeconds(1);
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Open", true);

        player.GetComponent<Animator>().SetBool("Dead", true);
        player.source.clip = player.death;
        player.source.volume = 0.2f;
        player.source.Play();
        yield return new WaitForSeconds(4);
        Color add = new Color(0, 0, 0, 0.01f);
        while (black.color.a < 1)
        {
            black.color += add;
            yield return new WaitForSeconds(0.02f);
        }
        //Start player animation of falling over while opening the door, and playing the sounds.
        yield break;
    }

}
