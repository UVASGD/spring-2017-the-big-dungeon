using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickTravel : MonoBehaviour {

	public GameObject targetLocation;
	public string targetName;
	public QuickTravelUI quic;
	bool withinRange;

	// Use this for initialization
	void Start () {
		withinRange = false;
		quic = FindObjectOfType<QuickTravelUI>();
	}

	// Update is called once per frame
	void Update () {
		if (withinRange) {
			quic = FindObjectOfType<QuickTravelUI>();
			if (Input.GetKeyDown (KeyCode.Space) && !quic.isActive) {
				quic.openMenuWithTarget(targetLocation, targetName);
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
