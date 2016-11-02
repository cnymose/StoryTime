using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour {
    bool[] positionsUsed; //Possible spawn locations used (Follows Vector3[] positions)
    public Vector3 positions; //Possible spawn locations
    public GameObject[][] events; //2-dimensional array of event GameObjects (2-3xX)
    public float[] times; //Wanted times of spawns / Intervals
    public bool[] ready; //Follows "times". Is that timestamp ready?
    public float reCheckTime; //Time to wait if spawn is not ready
	// Use this for initialization
	void Start () {
        StartCoroutine("Spawns");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    IEnumerator Spawns() {
        for (int i = 0; i < times.Length; i++) {
            while (!ready[i]) { //If not ready to spawn the next capsule
                yield return new WaitForSeconds(reCheckTime); //Wait for a set amount of time and check again if ready
            }

        }
    }
}
