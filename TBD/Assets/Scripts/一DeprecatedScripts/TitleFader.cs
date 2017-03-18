using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFader : MonoBehaviour {
    Animator anim;
    bool isFading = false;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public IEnumerator FadeToClear()
    {
        Debug.Log("IN TitleFader clear");
        isFading = true;
        anim.SetTrigger("TitleIn");
        while (isFading)
            yield return null;
    }

    public void FadeToBlack()
    {
        Debug.Log("IN TitleFader black");
        isFading = true;
        anim.SetTrigger("TitleOut");
    }

    void AnimationComplete()
    {
        isFading = false;
    }
}
