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
	private SaveController sc;
	private bool switching = false;
	private bool canSwitch = true;

	// Use this for initialization
	void Start () {
		musicTracks.AddRange(GetComponentsInChildren<AudioSource>());
		vm = FindObjectOfType<VolumeManager>();
		Scene currentScene = SceneManager.GetActiveScene();
		int buildIndex = currentScene.buildIndex;
		Debug.Log ("Starting music");
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
		if (!switching) {

		}
	}

	public void SwitchTrack(int requestedTrack, float requestedFadeOutSpeed = 0.4f, float requestedFadeInSpeed = 0.2f) {
		//Debug.Log("Calling SwitchTrack from: " + new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name);
		if (newTrackPlaying || isFading) {
			if (newTrack != requestedTrack && this != null) {
				// Speed up the track changes
				newTrack = requestedTrack;
				Debug.Log("Waiting on track " + newTrack);
				StartCoroutine(WaitAndTryAgain(1.0f, newTrack, 0.8f, 0.8f));
				fadeOutSpeed = 0.8f;
				fadeInSpeed = 0.8f;
			}
			if (this == null) {
				sc.rememberMusic (newTrack, requestedFadeOutSpeed, requestedFadeInSpeed);
			}
		}
		else if (this != null){
			if (currentTrack != requestedTrack) {
				Debug.Log ("Switching to track " + requestedTrack);
				switching = true;
				newTrack = requestedTrack;
				waitingOnSwitch = true;
				oldAudioLevel = musicTracks[currentTrack].GetComponent<VolumeController>().getDefaultAudio();
				newAudioLevel = musicTracks[requestedTrack].GetComponent<VolumeController>().getDefaultAudio();
				currentVolume = vm.getCurrentMusicVolumeLevel() * oldAudioLevel;
				fadeOutSpeed = requestedFadeOutSpeed;
				fadeInSpeed = requestedFadeInSpeed;
				isFading = true;
			}
		}
	}

	IEnumerator WaitAndTryAgain(float duration, int requestedTrack, float requestedFadeOutSpeed = 0.4f, float requestedFadeInSpeed = 0.2f) {
		yield return new WaitForSeconds(duration);
		if (currentTrack != requestedTrack)
			newTrack = requestedTrack;
		if (!newTrackPlaying && !isFading) {
			waitingOnSwitch = true;
			newTrack = requestedTrack;
			Debug.Log ("About to play " + newTrack);
			SwitchTrack(newTrack, requestedFadeOutSpeed, requestedFadeInSpeed);
		}
		else if (!switching){
			Debug.Log ("Continue waiting for track " + newTrack);
			StartCoroutine(WaitAndTryAgain(1.0f, newTrack, requestedFadeOutSpeed, requestedFadeInSpeed));
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
			switching = false;
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
