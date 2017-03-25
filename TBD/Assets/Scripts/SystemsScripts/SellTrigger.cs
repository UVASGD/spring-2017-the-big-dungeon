using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellTrigger : MonoBehaviour {

	private SellMenuUI sell;
	private bool isWithin;

	// Use this for initialization
	void Start() {
		sell = FindObjectOfType<SellMenuUI>();
		isWithin = false;
	}

	void Update() {
		if (isWithin && Input.GetKeyDown(KeyCode.Space))
			sell.toggleSellMenu();
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
