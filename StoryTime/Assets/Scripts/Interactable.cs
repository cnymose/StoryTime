using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour {
    public int type;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Interact() {
        print("Interacted");
        switch (type) {
            case 0:
                GetComponent<Movie>().PlayMovie();
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
}
