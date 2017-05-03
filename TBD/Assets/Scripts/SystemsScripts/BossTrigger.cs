using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour {

	private RandomEncounter ran;
	private bool isWithin;
	public int boss;


	// Use this for initialization
	void Start() {
		ran = FindObjectOfType<RandomEncounter>();
		isWithin = false;
	}

	void Update() {
		if (isWithin && Input.GetKeyDown (KeyCode.Space)) {
			switch (boss) {
			case 0:
				ran.catfishEncounter ();
				break;
			case 1:
				ran.bossEncounter ();
				break;
			}
		}

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name == "Player") {
			isWithin = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.name == "Player") {
			isWithin = false;
		}
	}
}
