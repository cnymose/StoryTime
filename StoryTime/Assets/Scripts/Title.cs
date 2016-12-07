using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Title : MonoBehaviour {
    public Movie movie;
    public UnityEngine.UI.Image black;
    public AudioClip startSound;
    public GameObject[] ui;
    AudioSource source;
    bool started = false;
	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Pause") && !started) {
            started = true;
            StartCoroutine(StartGame());
        }
	}

    IEnumerator StartGame() {
        source.clip = startSound;
        source.Play();
        
        while (black.color.a < 1) {
            black.color += new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.02f);
        }
        movie.PlayMovie();
        for (int i = 0; i < ui.Length; i++) {
            ui[i].SetActive(false);
        }
      
       
        black.color = new Color(0, 0, 0, 0);
        yield return new WaitForSeconds(15);
        while (black.color.a < 1)
        {
            black.color += new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.02f);
        }
        Application.LoadLevel(1);
        yield break;
    }
}
