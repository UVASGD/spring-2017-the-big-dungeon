using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MusicController : MonoBehaviour {

	public static List<AudioSource> musicTracks = new List<AudioSource>();
	public static int currentTrack;
	private static int newTrack = -1;
	private static bool isFading = false;
	private static bool newTrackPlaying = false;

	public  bool musicCanPlay = true;
	private VolumeManager vm;
	public float fadeOutSpeed = 0.4f;
	public float fadeInSpeed = 0.2f;
	private float currentVolume = 1.0f;
	private float newVolume = 0.0f;
	private float oldAudioLevel;
	private float newAudioLevel;
	private bool waitingOnSwitch = false;

	// Use this for initialization
	void Start () {
		musicTracks.AddRange(GetComponentsInChildren<AudioSource>());
		vm = FindObjectOfType<VolumeManager>();
		Scene currentScene = SceneManager.GetActiveScene();
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
	
	// Update is called once per frame
	void Update () {
		if (musicCanPlay && !newTrackPlaying) {
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
			fadeIn(newTrack);
		}
	}

	public void SwitchTrack(int requestedTrack, float requestedFadeOutSpeed = 0.4f, float requestedFadeInSpeed = 0.2f) {
		//Debug.Log("Calling SwitchTrack from: " + new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name);
		if (newTrackPlaying || isFading) {
			if (newTrack != requestedTrack) {
				// Speed up the track changes
				StartCoroutine(WaitAndTryAgain(1.0f, requestedTrack, 0.8f, 0.8f));
				fadeOutSpeed = 0.8f;
				fadeInSpeed = 0.8f;
			}
		} else {
			if (currentTrack != requestedTrack) {
				waitingOnSwitch = true;
				oldAudioLevel = musicTracks[currentTrack].GetComponent<VolumeController>().getDefaultAudio();
				newAudioLevel = musicTracks[requestedTrack].GetComponent<VolumeController>().getDefaultAudio();
				currentVolume = vm.getCurrentMusicVolumeLevel() * oldAudioLevel;
				newTrack = requestedTrack;
				fadeOutSpeed = requestedFadeOutSpeed;
				fadeInSpeed = requestedFadeInSpeed;
				isFading = true;
			}
		}
	}

	IEnumerator WaitAndTryAgain(float duration, int requestedTrack, float requestedFadeOutSpeed = 0.4f, float requestedFadeInSpeed = 0.2f) {
		yield return new WaitForSeconds(duration);
		if (!newTrackPlaying && !isFading) {
			waitingOnSwitch = true;
			SwitchTrack(requestedTrack, requestedFadeOutSpeed, requestedFadeInSpeed);
		}
		else {
			StartCoroutine(WaitAndTryAgain(1.0f, requestedTrack, requestedFadeOutSpeed, requestedFadeInSpeed));
		}
	}

	void fadeIn(int requestedTrack) {
		if (newVolume < vm.getCurrentMusicVolumeLevel() * newAudioLevel) {
			newVolume += fadeInSpeed * Time.deltaTime;
			musicTracks[requestedTrack].volume = newVolume;
		} else {
			// Done
			newTrackPlaying = false;
			currentVolume = newVolume;
			newVolume = 0.0f;
			currentTrack = requestedTrack;
			newTrack = -1;
			fadeOutSpeed = 0.4f;
			fadeInSpeed = 0.2f;
			waitingOnSwitch = false;
		}
	}

	void fadeOut(int oldTrack) {
		if (currentVolume > 0.15f * vm.getCurrentMusicVolumeLevel() * oldAudioLevel) {
			currentVolume -= fadeOutSpeed * Time.deltaTime;
			musicTracks[oldTrack].volume = currentVolume;
		} else {
			musicTracks[oldTrack].Stop();
			musicTracks[newTrack].Play();
			isFading = false;
			newTrackPlaying = true;
		}
	}

	public int getCurrentTrack() {
		return currentTrack;
	}

	public bool isWaitingOnSwitch() {
		return waitingOnSwitch;
	}
}
