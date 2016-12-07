using UnityEngine;
using System.Collections;

public class PointerScript : MonoBehaviour {
    Player player;
    ParticleSystem part;
    private float groundHeight;
    public float groundHeightScale;
    public float distanceSize;
    public float aboveGround;
    public float heightScale;
    public float fadeThreshold;
    private Color partCol;
    public bool shouldPlay = true;
    public bool alwaysPlay = false;
    ParticleSystem.SizeOverLifetimeModule size;
    
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();
        part = GetComponent<ParticleSystem>();
        size = part.sizeOverLifetime;
        partCol = part.startColor;
        
	}
	
	// Update is called once per frame
	void Update () {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit)) {
            groundHeight = transform.position.y - hit.distance;
        }
       
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if ((dist < 700 && !part.isPlaying && shouldPlay) || (alwaysPlay && !part.isPlaying))
        {
            part.Play();
        }
        else if (dist > 700 && part.isPlaying && !alwaysPlay) {
            part.Stop();
        }
        part.startColor = dist < fadeThreshold ? new Color(partCol.r,partCol.g,partCol.b, 0) : partCol;

        
        size.size = dist * distanceSize;
        transform.position = new Vector3(transform.position.x, groundHeight * groundHeightScale + (aboveGround * heightScale * size.size.constant), transform.position.z);
    }
}
