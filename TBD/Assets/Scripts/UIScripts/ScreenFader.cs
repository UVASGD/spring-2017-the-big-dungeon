using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour {

    private Animator anim;
    public bool isFading = false;
	private bool initialized = false;

	// Use this for initialization
	void Start () {
		Debug.Log("Started fader");
        anim = GetComponent<Animator>();
		initialized = true;
		isFading = true;
	}

	public IEnumerator Wait(float duration) {
		yield return new WaitForSeconds(duration);
		CutBlackOut();
	}

    public void FadeToClear() {
		if (!initialized) {
			Start();
		}
		isFading = true;
        anim.SetTrigger("FadeIn");
    }

    public void FadeToBlack() {
		if (!initialized) {
			Start();
		}
		isFading = true;
		anim.SetTrigger("FadeOut");
	}

	public void BlackOut() {
		if (!initialized) {
			Start();
		}
		this.anim.SetBool("BlackOut", true);
	}

	public void CutBlackOut() {
		if (!initialized) {
			Start();
		}
		this.anim.SetBool("BlackOut", false);
	}

	void AnimationComplete() {
		Debug.Log("animation complete");
        isFading = false;
    }

}
