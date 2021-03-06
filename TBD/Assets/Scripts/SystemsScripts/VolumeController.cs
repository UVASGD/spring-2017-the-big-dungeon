﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour {

	private AudioSource theAudio;
	private float audioLevel;
	public float defaultAudio = 1.0f;

	// Use this for initialization
	void Start () {
		theAudio = GetComponent<AudioSource> ();
	}

	public void SetAudioLevel(float volume){
		if (theAudio == null) {
			theAudio = GetComponent<AudioSource> ();
		}
		audioLevel = defaultAudio * volume;
		theAudio.volume = audioLevel;
	}

	public float getDefaultAudio() {
		return defaultAudio;
	}

	public void setDefaultAudio(float set) {
		defaultAudio = set;
	}
}
