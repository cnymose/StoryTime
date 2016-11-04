using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {
    public int type;
    public Movie holo;
    public GameObject textObject;
    public AudioClip storySound;
    AudioSource source;
    public bool hasInteracted = false;
	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        source.clip = storySound;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Interact() {
        hasInteracted = true;
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
                    source.Play();
                }
                else {
                    source.Stop();
                    textObject.SetActive(false);
                    GameObject.Find("Player").GetComponent<Player>().canMove = true;
                }
                break;
            case 2:
                break;
        }
    }
}
