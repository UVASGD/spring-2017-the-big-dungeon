using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {

	PlayerController player;
	SaveController sc;

	bool withinRange;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ();
		sc = FindObjectOfType<SaveController> ();
		withinRange = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (withinRange) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				sc.SaveTo ("default");
				Debug.Log ("Saved");
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			withinRange = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			withinRange = false;
		}
	}
}
