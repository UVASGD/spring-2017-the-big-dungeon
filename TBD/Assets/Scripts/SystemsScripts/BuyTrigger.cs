using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyTrigger : MonoBehaviour {

	private BuyMenuUI buy;
	private bool isWithin;

	// Use this for initialization
	void Start() {
		buy = FindObjectOfType<BuyMenuUI>();
		isWithin = false;
	}

	void Update() {
		if (isWithin && Input.GetKeyDown(KeyCode.Space))
			buy.toggleBuyMenu();
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
