using UnityEngine;
using System.Collections;

public class padCollision : MonoBehaviour {
    public AudioSource firstDoor;
    public AudioSource secondDoor;
    AudioSource board;
    Animator anim;
    bool hasEntered = false;
    
    // Use this for initialization

    void Start () {
        board = GetComponent<AudioSource>();
        anim = GetComponentInParent<Animator>();
       
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !hasEntered)
        {
            hasEntered = true;
            
            
            
            anim.SetBool("Launch", true);
           
            StartCoroutine(ChildFix(other.gameObject));
            
        }

      

    }
    IEnumerator ChildFix(GameObject other) {


        firstDoor.Play();
        other.transform.parent = transform;
        other.GetComponent<Player>().canMove = false;
        yield return new WaitForSeconds(2);
        secondDoor.Play();
        board.Play();
        yield return new WaitForSeconds(5);
        other.GetComponent<Player>().canMove = true;
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
