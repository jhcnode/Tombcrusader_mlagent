using UnityEngine;
using System.Collections;

public class AudioVolumeManager : MonoBehaviour {
   public bool isBgm = false;
   public bool isEffect = false;
   public AudioSource audio;


   void Awake()
   {
        if (audio == null)
            audio = gameObject.GetComponent<AudioSource>();

        if (isBgm == true)
        {
            if (audio != null)
                audio.volume = Global.backgroundSoundVolume;
        }
        if (isEffect == true)
        {
            if (audio != null)
                audio.volume = Global.efffectSoundVolume;
        }
    }

	// Use this for initialization
	void Start () {
	
	
	}
	
	// Update is called once per frame
	void Update () {
        if (isBgm == true)
        {
            if (audio != null)
                audio.volume = Global.backgroundSoundVolume;
        }
        if (isEffect == true)
        {
            if (audio != null)
                audio.volume = Global.efffectSoundVolume;
        }

    }
}
