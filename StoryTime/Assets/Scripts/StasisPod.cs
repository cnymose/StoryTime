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
        
    }
	
	// Update is called once per frame
	void Update () {
       
            
        
	}
    IEnumerator Open() {
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("Open", true);
        
        yield break;
    }
}
