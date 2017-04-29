using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour {

    public bool isFading = false;
	private bool initialized = false;

	public enum FadeStatus
	{
		FADE_TO_BLACK,FADE_TO_CLEAR,INSTANT_BLACK,INSTANT_CLEAR
	}

	private FadeStatus state;
	private float alpha;

	static GameObject instance;

	void Awake() {
		if (instance == null) {
			instance = this.gameObject;
		} else if(instance != this) {
			Destroy (this);
		}
		Debug.Log ("Fading in.................");
	}

	// Use this for initialization
	void Start () {
		if (!initialized) {
			Debug.Log("Started fader");
			initialized = true;
			isFading = true;
		}
		BlackOut ();
		FadeToClear ();
	}

	void Update() {
		switch (state) {
		case FadeStatus.FADE_TO_BLACK:
			alpha = Mathf.Min (1, alpha + (Time.deltaTime * 0.5f));
			break;
		case FadeStatus.FADE_TO_CLEAR:
			alpha = Mathf.Max (0, alpha - (Time.deltaTime * 0.5f));
			break;
		case FadeStatus.INSTANT_BLACK:
			alpha = 1;
			break;
		case FadeStatus.INSTANT_CLEAR:
			alpha = 0;
			break;
		}
		this.GetComponent<Image>().color = new Color (0, 0, 0, alpha);
	}

	public IEnumerator Wait(float duration) {
		yield return new WaitForSeconds(duration);
		CutBlackOut();
	}

    public void FadeToClear() {
		if (!initialized) {
			Start();
		}
		this.state = FadeStatus.FADE_TO_CLEAR;
    }

    public void FadeToBlack() {
		if (!initialized) {
			Start();
		}
		this.state = FadeStatus.FADE_TO_BLACK;
	}

	public void BlackOut() {
		if (!initialized) {
			Start();
		}
		this.state = FadeStatus.INSTANT_BLACK;
		alpha = 1;
	}

	public void CutBlackOut() {
		if (!initialized) {
			Start();
		}
		this.state = FadeStatus.INSTANT_CLEAR;
		alpha = 0;
	}

	void AnimationComplete() {
		Debug.Log("animation complete");
        isFading = false;
    }

	public void startLevel() {
		Debug.Log ("Kill me Raggy");
		BlackOut ();
		FadeToClear ();
	}
}
