using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour {
    AudioSource oldAudio;
    AudioSource newAudio;
    float audio1Volume = 0.6f;
    float audio2Volume = 0.0f;
    bool track2Playing = false;
    bool startChange = false;

	// Use this for initialization
	void Start () {
        newAudio = GetComponent<AudioSource>();
        oldAudio = GameObject.FindObjectOfType<Camera>().GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (startChange)
        {
            fadeOut();
            if (audio1Volume <= 0.3f)
            {
                if (track2Playing == false)
                {
                    track2Playing = true;
                    newAudio.Play();
                }
                fadeIn();
            }
        }
	}

    void fadeIn()
    {
        if (audio2Volume < 0.6f)
        {
            audio2Volume += (float) 0.1 * Time.deltaTime;
            newAudio.volume = audio2Volume;
        }
    }

    void fadeOut()
    {
        if (audio1Volume > 0.1f)
        {
            audio1Volume -= (float) 0.1 * Time.deltaTime;
            oldAudio.volume = audio1Volume;
        } else
        {
            oldAudio.Stop();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        startChange = true;
    }
}
