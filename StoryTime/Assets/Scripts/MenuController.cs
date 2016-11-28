using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MenuController : MonoBehaviour {
    public Color[] colors;
    public UnityEngine.UI.Text[] texts;
    int currentText = 0;
    float waitTime = 0.3f;
    Player player;
    CamScript cam;


	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();
        cam = Camera.main.GetComponent<CamScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void StartDetecting(bool start) {
        if (start)
        {
            UpdateSelection();
            StartCoroutine("GetInput");
        }
        else {
            StopAllCoroutines();
        }
    }

    void UpdateSelection() {
        for (int i = 0; i < texts.Length; i++) {
            texts[i].color = i == currentText ? colors[0] : colors[1];
        }
    }

    void ToggleInvert() {
        cam.invert *= -1;
        texts[1].text = cam.invert == 1 ? "Inverted Controls: On" : "Inverted Controls: Off";
    }

    void Unpause() {
        player.Pause();
    }
    IEnumerator GetInput() {
        while (true) {
            print("Checking");
            print(Input.GetAxis("UpDown"));
            if (Input.GetAxis("UpDown") == 1) {
                currentText = currentText == texts.Length - 1 ? 0 : currentText + 1;
                UpdateSelection();
                yield return new WaitForSeconds(waitTime);

            }
            else if (Input.GetAxis("UpDown") == -1)
            {
                currentText = currentText == 0 ? texts.Length - 1 : currentText - 1;
                UpdateSelection();
                yield return new WaitForSeconds(waitTime);
            }
            else if (Input.GetButtonDown("Jump")){
                switch (currentText) {
                    case 0:
                        Unpause();
                        break;

                    case 1:
                        ToggleInvert();
                        break;
                }
            }
            yield return new WaitForSeconds(0.016f);
            }

        
        yield break;
    }
}
