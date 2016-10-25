using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
    public Vector3[] positions;
    bool[] hasSpawned = { false, false, false };
    int spawn = 0;
    public GameObject[] hiero;
    public GameObject[] holo;
    public GameObject[] qr;
	// Use this for initialization
	void Start () {
        Instantiate(RandomObject(), positions[spawn], transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    GameObject RandomObject() {
        int random = (int)Random.Range(0, 3);
        random = (int) Mathf.Min(random, 2);
        if (!hasSpawned[random]) {
            spawn++;
            switch (random) {
                
                case 0:
                   
                    return hiero[spawn];
                case 1:
                    
                    return holo[spawn];
                case 2:
                 
                    return qr[spawn];
            }
        }
        else {
            return RandomObject();
        }
        return null;

    }
}

