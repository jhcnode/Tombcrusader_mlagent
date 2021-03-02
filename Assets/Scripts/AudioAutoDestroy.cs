using UnityEngine;
using System.Collections;

public class AudioAutoDestroy : MonoBehaviour {

   
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.GetComponent<AudioSource>().isPlaying==false)
        {
            GameObject.Destroy(gameObject);
        }
	
	}
}
