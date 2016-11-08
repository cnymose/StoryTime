using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour {
    public int currentEvent = 0;   //Which event number are we currently at in the sequence?
    public EventSpawnTrigger[] triggers;
	// Use this for initialization
	void Start () {
        GameObject[] triggerObj = GameObject.FindGameObjectsWithTag("EventTrigger");
        triggers = MakeTriggerArray(triggerObj);
       
    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CleanUp() {
        currentEvent++;
        GameObject[] events = GameObject.FindGameObjectsWithTag("Interactable"); //Find all Interactables as they are the "events"
        for (int i = 0; i < events.Length; i++) {
            if (events[i].GetComponent<Interactable>().sequence == currentEvent - 1) { //If they are part of the event we just had -

                Destroy(events[i]); //- then Destroy
            }
        }
        for (int j = 0; j < triggers.Length; j++) {
            if (!triggers[j].doNotSpawn) {
                triggers[j].triggered = false;
            }
        }
    }

    EventSpawnTrigger[] MakeTriggerArray(GameObject[] triggerObj)  
    {
        EventSpawnTrigger[] trigArr = new EventSpawnTrigger[triggerObj.Length];
        print(triggerObj.Length);
        print(trigArr.Length);
        for (int i = 0; i < triggerObj.Length; i++)
        {
            trigArr[i] = triggerObj[i].GetComponent<EventSpawnTrigger>();
        }
        return trigArr;
    }

    
}
