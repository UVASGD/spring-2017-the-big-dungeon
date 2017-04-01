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
	private bool _timerOn = false;
	public bool timerOn {
		get {
			return _timerOn;
		}
		private set {
			_timerOn = value;
			Debug.Log(value);
		}
	}

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
		timerOn = false;
	}

	void Update() {
		if (timerOn) {
			timer += Time.deltaTime;
			if (timer > timerMax) {
				player.gameObject.transform.position = warpTarget.position;
				Camera.main.transform.position = warpTarget.position;
				Camera.main.GetComponent<CameraManager>().setCurrentRoom(targetRoom);
				sf.FadeToClear();
				player.frozen = false;
				timer = 0f;
				Debug.Log("in here");
				timerOn = false;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (sf != null) {
			sf = GameObject.FindGameObjectWithTag("Fader").GetComponent<ScreenFader>();
			timerOn = true;
			player = FindObjectOfType<PlayerController>();
			player.frozen = true;
			sf.FadeToBlack();
		}
    }
}
