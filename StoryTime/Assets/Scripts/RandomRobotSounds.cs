using UnityEngine;
using System.Collections;

public class RandomRobotSounds : MonoBehaviour {
    AudioSource aud;
    public AudioClip[] clips;
	// Use this for initialization
	void Start () {
        aud = GetComponent<AudioSource>();
       // StartCoroutine(Sounds());	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
  /*  IEnumerator Sounds() {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5, 10));
            int random = (int)Random.Range(0, clips.Length);
           
            aud.clip = clips[random];
            aud.Play();
        }
    }*/
}
