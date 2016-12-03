using UnityEngine;
using System.Collections;

public class reverseLaunchPad : MonoBehaviour {

    Animator anim;
    bool reset = false;
	
    // Use this for initialization
	void Start () {

        anim = GetComponentInParent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {

            anim.SetBool("isOffPad", true);
            //AnimationClip clip = transform.parent.FindChild("Sphere").GetComponent<padCollision>().GetAnimationClip("toOrigin");
        }
    }
}
