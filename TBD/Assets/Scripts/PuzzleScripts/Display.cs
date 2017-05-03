using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour {

	public Sprite[] states;
	public SpriteRenderer[] displayBois;
	public int[] currentValues;
	public int[] correctValues;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Evaluate ()) {
			//Call thing that effects world.
		}
	}

	public void Apply(int[] increments) {
		if (states.Length > 0) {
			for (int i = 0; i < currentValues.Length && i < increments.Length; i++) {
				currentValues [i] = (currentValues [i] + increments [i]) % states.Length;
			}
			UpdateDisplay ();
		}
	}

	public void UpdateDisplay() {
		for (int i = 0; i < displayBois.Length && i < currentValues.Length; i++) {
			displayBois [i].sprite = states [currentValues [i] % states.Length];
		}
	}

	public bool Evaluate() {
		for (int i = 0; i < currentValues.Length && i < correctValues.Length; i++) {
			if (currentValues [i] != correctValues [i]) {
				return false;
			}
		}
		return true;
	}
}
