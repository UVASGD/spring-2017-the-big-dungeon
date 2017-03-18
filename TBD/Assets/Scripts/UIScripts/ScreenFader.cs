using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFader : MonoBehaviour {

    Animator anim;
    bool isFading = false;
	bool initialized = false;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
		initialized = true;
	}
	
	public IEnumerator Wait(float duration) {
		yield return new WaitForSeconds(duration);
		CutBlackOut();
	}

    public IEnumerator FadeToClear() {
		if (!initialized) {
			Start();
		}
		isFading = true;
        anim.SetTrigger("FadeIn");
        while (isFading)
            yield return null;
    }

    public IEnumerator FadeToBlack() {
		if (!initialized) {
			Start();
		}
		isFading = true;
        anim.SetTrigger("FadeOut");
        while (isFading)
            yield return null;
    }

	public void BlackOut() {
		if (!initialized) {
			Start();
		}
		anim.SetBool("BlackOut", true);
	}

	public void CutBlackOut() {
		if (!initialized) {
			Start();
		}
		anim.SetBool("BlackOut", false);
	}

	void AnimationComplete() {
        isFading = false;
    }
}
