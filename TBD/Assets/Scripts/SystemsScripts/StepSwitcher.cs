using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSwitcher : MonoBehaviour {

	private PlayerController player;
	private SFXManager sfxMan;

	public string ground;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController> ();
		sfxMan = FindObjectOfType<SFXManager>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Checks for entering certain areas
	void OnTriggerEnter2D(Collider2D other) {
		if (player.stepsOn && other.tag == "Player") {
			if (ground == "grass") {
				sfxMan.GroundChange("grass");
			}
			else if (ground == "water") {
				sfxMan.GroundChange("water");
			}
			else
				sfxMan.GroundChange("default");
		}
	}
		
	void OnTriggerStay2D(Collider2D other) {
		if (player.stepsOn && other.tag == "Player") {
			if (ground == "grass") {
				sfxMan.GroundChange("grass");
			}
			else if (ground == "water") {
				sfxMan.GroundChange("water");
			}
			else
				sfxMan.GroundChange("default");
		}
	}

	//Checks for exiting certain areas
	void OnTriggerExit2D(Collider2D other) {
		if (player.stepsOn && other.tag == "Player") {
			sfxMan.GroundChange("default");
		}
	}


}
