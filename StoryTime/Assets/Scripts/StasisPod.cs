using UnityEngine;
using System.Collections;

public class StasisPod : MonoBehaviour {
    Player player;
    float spawnTime;
    Animator anim;
    public float waitTime;
    bool opened = false;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        spawnTime = Time.time;
        player = GameObject.Find("Player").GetComponent<Player>();
        StartCoroutine(Open());
        player.canMove = false;
        
    }
	
	// Update is called once per frame
	void Update () {
       
            
        
	}
    IEnumerator Open() {
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("Open", true);
        yield return new WaitForSeconds(0.8f);
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        player.canMove = true;
        yield break;
    }
}
