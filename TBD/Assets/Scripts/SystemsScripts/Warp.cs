using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour {

    public PlayerMovement player;
    public Transform warpTarget;
	public GameObject targetRoom;
    ScreenFader sf;
    TitleFader tf;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        sf = GameObject.FindGameObjectWithTag("Fader").GetComponent<ScreenFader>();
        tf = GameObject.FindGameObjectWithTag("Title").GetComponent<TitleFader>();
        tf.FadeToBlack();
    }

    IEnumerator OnTriggerEnter2D(Collider2D other) {
        Debug.Log("An object Collided.");

        Debug.Log("PRE FADE OUT");
        player.frozen = true;
        yield return StartCoroutine(sf.FadeToBlack());

        Debug.Log("UPDATE PLAYER POS");

        other.gameObject.transform.position = warpTarget.position;
        Camera.main.transform.position = warpTarget.position;
		if (targetRoom) {
			Camera.main.GetComponent<CameraFollow> ().setCurrentRoom (targetRoom);
		}

        if (warpTarget.name == "Hole")
        {
            yield return StartCoroutine(tf.FadeToClear());
        }

        yield return StartCoroutine(sf.FadeToClear());
        player.frozen = false;
        Debug.Log("FADE IN COMPLETE");
    }
}
