using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEncounter : MonoBehaviour {

	// Collision2D, check for player, random num check (function for changing prob), get list of enemies (pass in), execute battleManager
	// Use this for initialization

	private BoxCollider2D box;
	private bool playerWithin = false;
	public GameObject player;
	private Animator playerAnim;
	private float cooldownTimer;
	private float timerMax = 10f;
	private bool cooldown = false;

	void Start () {
		this.box = GetComponent<BoxCollider2D> ();
		playerAnim = player.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (playerAnim.GetBool ("is_walking"));
		if (playerWithin && playerAnim.GetBool("is_walking") && !cooldown) {
			checkEncounter ();
		}
		if (cooldown) {
			cooldownTimer += Time.deltaTime;
			if (cooldownTimer > timerMax) {
				Debug.Log ("cooldown finished");
				cooldown = false;
				cooldownTimer = 0f;
			}
		}
	}

	public void checkEncounter() {
		int ran = Random.Range(0, 333);
		// 1 encounter / 10 seconds == 1/333
		if (ran < 1) {
			Debug.Log ("Random executed!");
			Debug.Log ("cooldown started");
			// set cooldown once exited from battle
			cooldown = true;
		}
	}

	public void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("entered");
		playerWithin = true;
	}

	public void OnTriggerExit2D(Collider2D other) {
		Debug.Log ("exited");
		playerWithin = false;
	}
}
