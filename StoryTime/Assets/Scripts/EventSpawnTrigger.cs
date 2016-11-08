using UnityEngine;
using System.Collections;

public class EventSpawnTrigger : MonoBehaviour {
    public bool doNotSpawn = false;
    public bool triggered;
    EventController controller;
    public GameObject[] spawnEvent;
	// Use this for initialization
	void Start () {
        controller = GameObject.Find("EventController").GetComponent<EventController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  
    public void Spawn() {
        spawnEvent[controller.currentEvent].SetActive(true); //Sets the current int in sequence active at this position.
        spawnEvent[controller.currentEvent].GetComponent<Interactable>().sequence = controller.currentEvent; //Sets the number of sequence on the object so that we can delete it later.
    }
}
