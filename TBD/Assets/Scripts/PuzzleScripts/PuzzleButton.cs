using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(Collider2D))]
public class PuzzleButton : MonoBehaviour {

	public int[] changes;
	public Display disp;

	public Sprite unpressed, pressed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			disp.Apply (changes);
			gameObject.GetComponent<SpriteRenderer> ().sprite = pressed;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			gameObject.GetComponent<SpriteRenderer> ().sprite = unpressed;
		}
	}
}
