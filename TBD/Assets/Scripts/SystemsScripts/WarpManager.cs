using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour {

    private PlayerController player;
    public Transform warpTarget;
	public GameObject targetRoom;
    ScreenFader sf;
	//TitleFader tf;
	private float timer = 0f;
	private float timerMax = 3f;
	private bool timerOn;

	// Use this for initialization
	void Start()
    {
        player = FindObjectOfType<PlayerController>();
		try {
			sf = GameObject.FindGameObjectWithTag("Fader").GetComponent<ScreenFader>();
		}
		catch {
			sf = null;
		}
		setTimerOn(true);
    }

	void Update() {
		Debug.Log(getTimerOn());
		if (getTimerOn()) {
			timer += Time.deltaTime;
			if (timer > timerMax) {
				player.gameObject.transform.position = warpTarget.position;
				Camera.main.transform.position = warpTarget.position;
				Camera.main.GetComponent<CameraManager>().setCurrentRoom(targetRoom);
				StartCoroutine(sf.FadeToClear());
				player.frozen = false;
				timer = 0f;
				setTimerOn(false);
			}
		}
	}

	public void setTimerOn(bool t) {
		this.timerOn = t;
	}

	public bool getTimerOn() {
		return this.timerOn;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (sf != null) {
			Debug.Log(getTimerOn());
			setTimerOn(true);
			Debug.Log(getTimerOn());
			player = FindObjectOfType<PlayerController>();
			player.frozen = true;
			StartCoroutine(sf.FadeToBlack());
		}
    }
}
