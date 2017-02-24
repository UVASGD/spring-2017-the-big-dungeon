using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour {

    public PlayerController player;
    public Transform warpTarget;
	public GameObject targetRoom;
    ScreenFader sf;
    //TitleFader tf;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        sf = GameObject.FindGameObjectWithTag("Fader").GetComponent<ScreenFader>();
    }

    IEnumerator OnTriggerEnter2D(Collider2D other) {
        player.frozen = true;
        yield return StartCoroutine(sf.FadeToBlack());

        other.gameObject.transform.position = warpTarget.position;
        Camera.main.transform.position = warpTarget.position;
		Camera.main.GetComponent<CameraFollow> ().setCurrentRoom (targetRoom);

        yield return StartCoroutine(sf.FadeToClear());
        player.frozen = false;
    }
}
