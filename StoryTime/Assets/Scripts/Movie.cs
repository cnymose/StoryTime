using UnityEngine;
using System.Collections;

public class Movie : MonoBehaviour {
    public MovieTexture movie;
    AudioSource source;
    public AudioClip audClip;
	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.mainTexture = movie as MovieTexture;
        source = GetComponent<AudioSource>();
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void PlayMovie() {
        if (!movie.isPlaying)
        {
            GetComponent<Renderer>().material.color = Color.white;
            source.clip = audClip;
            source.Play();
            movie.Play();
            
            
        }
        
    }
  
}
