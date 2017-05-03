using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

	public static List<AudioSource> musicTracks = new List<AudioSource>();
	public static int currentTrack;
	private static int newTrack = -1;
	private static bool isFading = false;
	private static bool newTrackPlaying = false;
	public static bool musicPlaying = false;
	private static int waitTrack = -1;
	private int willKillTrack = -1;

	public bool musicCanPlay = true;
	public bool debugOn = false;
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
	private static bool started = false;

	// Use this for initialization

	void Start() {
		musicTracks.AddRange(GetComponentsInChildren<AudioSource>());
		vm = FindObjectOfType<VolumeManager>();
		Scene currentScene = SceneManager.GetActiveScene();
		int buildIndex = currentScene.buildIndex;
		debug("Starting music");
		if (!started) {
			debug("IN HERE");
			switch (buildIndex) {
				case 0:
					currentTrack = 8;
					break;
				case 1:
					currentTrack = 6;
					break;
				case 2:
					currentTrack = 4;
					break;
				default:
					currentTrack = 8;
					break;
			}
		}
		started = true;
		debug("Current track from start is " + currentTrack);
	}

	// Update is called once per frame
	void Update() {
		if (musicCanPlay && !newTrackPlaying) {
			if (!musicTracks[currentTrack].isPlaying) {
				debug("Officially starting " + currentTrack);
				musicTracks[currentTrack].Play();
			}
		} else {
			musicTracks[currentTrack].Stop();
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
		debug("Is a new track playing? " + newTrackPlaying);
		debug("Is it fading? " + isFading);
		if (newTrackPlaying || isFading) {
			if (newTrack != requestedTrack && this != null) {
				// Speed up the track changes
				willKillTrack = currentTrack;
				debug("Will kill this track " + willKillTrack);
				waitTrack = requestedTrack;
				currentTrack = newTrack;
				debug("Waiting on track " + waitTrack);
				StartCoroutine(WaitAndTryAgain(1.0f, waitTrack, 0.6f, 0.6f));
				fadeOutSpeed = 0.6f;
				fadeInSpeed = 0.6f;
			}
			if (this == null) {
				sc.rememberMusic(newTrack, requestedFadeOutSpeed, requestedFadeInSpeed);
			}
		}
		else if (this != null) {
			debug("currentTrack is " + currentTrack);
			if (currentTrack != requestedTrack) {
				debug("Switching to track " + requestedTrack);
				newTrack = requestedTrack;
				switching = true;
				waitingOnSwitch = true;
				oldAudioLevel = musicTracks[currentTrack].GetComponent<VolumeController>().getDefaultAudio();
				newAudioLevel = musicTracks[requestedTrack].GetComponent<VolumeController>().getDefaultAudio();
				if (vm == null)
					vm = FindObjectOfType<VolumeManager>();
				currentVolume = vm.getCurrentMusicVolumeLevel() * oldAudioLevel;
				fadeOutSpeed = requestedFadeOutSpeed;
				fadeInSpeed = requestedFadeInSpeed;
				isFading = true;
			}
		}
	}

	IEnumerator WaitAndTryAgain(float duration, int requestedTrack, float requestedFadeOutSpeed = 0.4f, float requestedFadeInSpeed = 0.2f) {
		yield return new WaitForSeconds(duration);
		if (currentTrack != requestedTrack) {
			debug("I see different tracks " + currentTrack + " " + requestedTrack);
			newTrack = requestedTrack;
		}
		if (!newTrackPlaying && !isFading) {
			waitingOnSwitch = true;
			newTrack = requestedTrack;
			debug("About to play " + newTrack);
			SwitchTrack(newTrack, requestedFadeOutSpeed, requestedFadeInSpeed);
		}
		else if (switching) {
			debug("Continue waiting for track " + newTrack);
			StartCoroutine(WaitAndTryAgain(1.0f, newTrack, requestedFadeOutSpeed, requestedFadeInSpeed));
		}
		else {
			debug("Failed to wait");
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
			debug("Finished");
		}
	}

	void fadeOut(int oldTrack) {
		if (currentVolume > 0.15f * vm.getCurrentMusicVolumeLevel() * oldAudioLevel) {
			currentVolume -= fadeOutSpeed * Time.deltaTime;
			musicTracks[oldTrack].volume = currentVolume;
		} else {
			debug("Stopping old track " + oldTrack);
			musicTracks[oldTrack].Stop();
			if (willKillTrack != -1) {
				debug("About to execute this track " + willKillTrack);
				musicTracks[willKillTrack].Stop();
				willKillTrack = -1;
			}
			debug("Starting new track " + newTrack);

			musicTracks[newTrack].Play();
			//currentTrack = newTrack;
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

	private void debug(string line) {
		if (debugOn) {
			UnityEngine.Debug.Log(line);
		}
	}

}
