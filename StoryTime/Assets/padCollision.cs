using UnityEngine;
using System.Collections;

public class padCollision : MonoBehaviour {
    public GameObject bunker;
    Animator anim;
    bool hasEntered = false;
    int launchHash = Animator.StringToHash("Launch");
    // Use this for initialization
    void Start () {
        anim = bunker.GetComponent<Animator>();
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (!hasEntered && other.tag == "Player")
        {
            hasEntered = true;
            Debug.Log("FUCK");

            anim.SetBool("Launch", true);

            // anim.SetBool("isOpen", true);
            StartCoroutine(ChildFix(other.gameObject));
            
        }
       
    }
    IEnumerator ChildFix(GameObject other) {
        
        other.transform.parent = transform;
        yield return new WaitForSeconds(7);
        other.transform.parent = null;
        
        yield break;
    }
}
