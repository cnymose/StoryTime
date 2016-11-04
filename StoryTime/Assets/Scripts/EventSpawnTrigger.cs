using UnityEngine;
using System.Collections;

public class EventSpawnTrigger : MonoBehaviour {
    public bool triggered;
    EventController controller;
	// Use this for initialization
	void Start () {
        controller = GameObject.Find("EventController").GetComponent<EventController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.gameObject.tag == "Player") {
            triggered = true;
            controller.SpawnNext();
        }
    }
}
