using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class StasisPod : MonoBehaviour {
    Player player;
    float spawnTime;
    Animator anim;
    public float waitTime;
    bool opened = false;
    UnityEngine.UI.Image black;
    // Use this for initialization
    void Start () {
        black = GameObject.Find("BlackUI").GetComponent<UnityEngine.UI.Image>();
        anim = GetComponent<Animator>();
        spawnTime = Time.time;
        player = GameObject.Find("Player").GetComponent<Player>();
        StartCoroutine(Open());
        player.canMove = false;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.B)) {
            anim.SetBool("Open",true);
        }
            
        
	}
    IEnumerator Open() {
        while (black.color.a > 0) {
            black.color -= new Color(0, 0, 0, 0.01f);
                yield return new WaitForSeconds(0.03f);
        }
        yield return new WaitForSeconds(waitTime);
        anim.SetBool("Open", true);
        yield return new WaitForSeconds(0.8f);
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(1);
        player.canMove = true;
        yield break;
    }
}
