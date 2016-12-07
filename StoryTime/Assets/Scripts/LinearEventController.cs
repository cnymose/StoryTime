using UnityEngine;
using System.Collections;

public class LinearEventController : MonoBehaviour {
    public int interacted = 0;
    public PointerScript[] pointers;
	// Use this for initialization
	void Start () {
        pointers[0].alwaysPlay = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
