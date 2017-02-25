using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour {

	public VolumeController[] vcObjects;

	public float currentMusicVolumeLevel;
	public float maxMusicVolumeLevel = 1.0f;
	public float currentSFXVolumeLevel;
	public float maxSFXVolumeLevel = 1.0f;

	// Use this for initialization
	void Start () {
		vcObjects = FindObjectsOfType<VolumeController> ();

		if (currentMusicVolumeLevel < maxMusicVolumeLevel) {
			currentMusicVolumeLevel = maxMusicVolumeLevel;
		}

		if (currentSFXVolumeLevel < maxSFXVolumeLevel) {
			currentSFXVolumeLevel = maxSFXVolumeLevel;
		}

		for (int i = 0; i < vcObjects.Length; i++) {
			try {
				vcObjects[i].GetComponentInParent<MusicController>();
				vcObjects[i].SetAudioLevel(currentMusicVolumeLevel);
			} catch { }
			try {
				vcObjects[i].GetComponentInParent<SFXManager>();
				vcObjects[i].SetAudioLevel(currentSFXVolumeLevel);
			}
			catch { }
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void updateMusicVolume(float volume) {
		currentMusicVolumeLevel = volume;
		if (currentMusicVolumeLevel > maxMusicVolumeLevel) {
			currentMusicVolumeLevel = maxMusicVolumeLevel;
		}
		for (int i = 0; i < vcObjects.Length; i++) {
			try {
				string test = vcObjects[i].GetComponentInParent<MusicController>().name;
				vcObjects[i].SetAudioLevel(currentMusicVolumeLevel);
			}
			catch { }
		}
	}

	public void updateSFXVolume(float volume) {
		currentSFXVolumeLevel = volume;
		if (currentSFXVolumeLevel > maxSFXVolumeLevel) {
			currentSFXVolumeLevel = maxSFXVolumeLevel;
		}
		for (int i = 0; i < vcObjects.Length; i++) {
			try {
				string test = vcObjects[i].GetComponentInParent<SFXManager>().name;
				vcObjects[i].SetAudioLevel(currentSFXVolumeLevel);
			}
			catch { }
		}
	}

	public float getCurrentMusicVolumeLevel() {
		return currentMusicVolumeLevel;
	}

	public float getCurrentSFXVolumeLevel() {
		return currentSFXVolumeLevel;
	}

	public void findVCObjects(){
		vcObjects = FindObjectsOfType<VolumeController> ();
		for (int i = 0; i < vcObjects.Length; i++) {
			try {
				vcObjects[i].GetComponentInParent<MusicController>();
				vcObjects[i].SetAudioLevel(currentMusicVolumeLevel);
			} catch { }
			try {
				vcObjects[i].GetComponentInParent<SFXManager>();
				vcObjects[i].SetAudioLevel(currentSFXVolumeLevel);
			}
			catch { }
		}
	}
}
