using UnityEngine;
using System.Collections;
using System.IO;



public class Tracker : MonoBehaviour {
    StreamWriter writer;
    string desktop = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
    // Use this for initialization
    void Start () {
        
        string name = System.DateTime.Now.ToString() +  ".txt";
       
        name = name.Replace('/', '_');
        name = name.Replace(':', '-');
        name = System.IO.Path.Combine(desktop, name);
        writer = File.CreateText(name);
        
        
        StartCoroutine(Log());
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    IEnumerator Log() {
        while (gameObject.activeInHierarchy)
        {
            string output;
            output = transform.position.x + "," + transform.position.z;
            writer.WriteLine(output);
            
            yield return new WaitForSeconds(5);
        }
        yield break;
    }

    void OnApplicationQuit() {
        writer.Close();
    }
}
