using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEncounter : MonoBehaviour {

	// Collision2D, check for player, random num check (function for changing prob), get list of enemies (pass in), execute battleManager
	// Use this for initialization

	private BoxCollider2D box;

	void Start () {
		this.box = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("entered");
	}

	public void OnTriggerExit2D(Collider2D other) {
		Debug.Log ("exited");
	}
}
