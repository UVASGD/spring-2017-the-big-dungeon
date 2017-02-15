using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MusicController : MonoBehaviour {

	public AudioSource[] musicTracks;

	public int currentTrack;

	public float fadeOutSpeed = 0.4f;
	public float fadeInSpeed = 0.2f;
	private float currentVolume = 1.0f;
	private float newVolume = 0.0f;
	private bool newTrackPlaying = false;
	private bool isFading = false;
	private int newTrack = -1;

	public bool musicCanPlay = true;

	// Use this for initialization
	void Start () {
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}
	
	// Update is called once per frame
	void Update () {
		if (musicCanPlay) {
			if (!musicTracks [currentTrack].isPlaying) {
				musicTracks [currentTrack].Play ();
			}
		} else {
			musicTracks [currentTrack].Stop ();
		}
		if (isFading) {
			fadeOut(currentTrack);
		}
		if (newTrackPlaying) {
			currentTrack = newTrack;
			fadeIn(currentTrack);
		}
	}

	public void SwitchTrack(int requestedTrack, float requestedFadeOutSpeed = 0.4f, float requestedFadeInSpeed = 0.2f) {
		newTrack = requestedTrack;
		fadeOutSpeed = requestedFadeOutSpeed;
		fadeInSpeed = requestedFadeInSpeed;
		if (currentTrack != requestedTrack) {
			isFading = true;	
		}
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		Scene currentScene = SceneManager.GetSceneByName(scene.name);
		int buildIndex = currentScene.buildIndex;
		switch (buildIndex) {
			case 0:
				SwitchTrack(2);
				break;
			case 1:
				SwitchTrack(0);
				break;
			default:
				SwitchTrack(0);
				break;
		}
	}

	void fadeIn(int newTrack) {
		if (newVolume < 1.0f) {
			newVolume += fadeInSpeed * Time.deltaTime;
			musicTracks[newTrack].volume = newVolume;
		} else {
			newTrackPlaying = false;
			newVolume = 0.0f;
			currentVolume = 1.0f;
		}
	}

	void fadeOut(int currentTrack) {
		if (currentVolume > 0.3f) {
			currentVolume -= fadeOutSpeed * Time.deltaTime;
			musicTracks[currentTrack].volume = currentVolume;
		} else {
			isFading = false;
			musicTracks[currentTrack].Stop();
			newTrackPlaying = true;
			musicTracks[newTrack].Play();
		}
	}
}
