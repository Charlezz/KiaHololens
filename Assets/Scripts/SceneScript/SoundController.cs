using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

    AudioSource audioSource;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlaySound(AudioClip clip)
    {
        
            if (!audioSource)
            {
                audioSource = this.gameObject.AddComponent<AudioSource>();
               
            }


        audioSource.PlayOneShot(clip, 1.0f);



    }
}
