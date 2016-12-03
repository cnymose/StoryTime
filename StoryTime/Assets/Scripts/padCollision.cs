using UnityEngine;
using System.Collections;

public class padCollision : MonoBehaviour {
    
    Animator anim;
    bool hasEntered = false;
    
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
            hasEntered = true;
            Debug.Log("FUCK");
            
            
            anim.SetBool("Launch", true);
           
            StartCoroutine(ChildFix(other.gameObject));
            
        }

      

    }
    IEnumerator ChildFix(GameObject other) {

      

        other.transform.parent = transform;
        yield return new WaitForSeconds(7);
        other.transform.parent = null;
        

        yield break;
    }


    public AnimationClip GetAnimationClip (string name)
    {
        if (!anim) return null;

        foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
        {
            if(clip.name == name)
            {
                return clip;
            }
        }
        return null;
    }
}
