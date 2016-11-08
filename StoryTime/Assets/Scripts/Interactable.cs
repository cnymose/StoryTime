using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {
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
	// Use this for initialization
	void Start () {
        controller = GameObject.Find("EventController").GetComponent<EventController>();
        source = GetComponent<AudioSource>();
       // source.clip = storySound;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Interact() {
        if (!hasInteracted) {
            trig.doNotSpawn = true;
            glow.GetComponent<ParticleSystem>().loop = false;
            sequence--;
            hasInteracted = true;
            
            controller.CleanUp();
        }
        
        print("Interacted");
        switch (type) {
            case 0:
                holo.PlayMovie();
                break;
            case 1:
                if (!textObject.activeInHierarchy)
                {
                    textObject.SetActive(true);
                    GameObject.Find("Player").GetComponent<Player>().canMove = false;
                   // source.Play();
                }
                else {
                  //  source.Stop();
                    textObject.SetActive(false);
                    GameObject.Find("Player").GetComponent<Player>().canMove = true;
                }
                break;
            case 2:
                break;
        }
    }
}
