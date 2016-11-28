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
    public AudioClip storySound;
    AudioSource source;
    public bool hasInteracted = false;
    EventController controller;
    public GameObject glow;
    Player player;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();
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
                glow.GetComponent<ParticleSystem>().loop = false;
                sequence--;
                hasInteracted = true;

                controller.CleanUp();
                print("Interacted");
                switch (type)
                {
                    case 0:
                        holo.PlayMovie();
                        break;
                    case 1:
                        GameObject.Find("Player").GetComponent<Player>().canMove = textObject.activeInHierarchy ? true : false;
                        textObject.SetActive(!textObject.activeInHierarchy);
                        /*if (!textObject.activeInHierarchy)
                        {
                            textObject.SetActive(true);
                            GameObject.Find("Player").GetComponent<Player>().canMove = false;
                           // source.Play();
                        }
                        else {
                          //  source.Stop();
                            textObject.SetActive(false);
                            GameObject.Find("Player").GetComponent<Player>().canMove = true;
                        }*/
                        break;
                    case 2:
                        break;
                }
            }

        }
        else {
            if (collectible) {
                player.ExitInteract();
                player.canMove = true;
                collectibleBG.SetActive(false);
                
                tag = " ";
                GetComponent<Interactable>().enabled = false;
            }
        }
        
    
    }
}
