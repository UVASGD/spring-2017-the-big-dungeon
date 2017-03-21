using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class QuickTravelUI : MonoBehaviour {

	public GameObject target;
	public bool isActive;

	int selectionIndex;

	public Image confirmationWindow;

	public Text pointyArrow;
	public int pointyArrowOffset;

	public Text menuText;

	Vector2 pointyArrowInit;

	float cooldown;

	PlayerController pc;

	// Use this for initialization
	void Start() {
		isActive = false;
		target = gameObject;
		selectionIndex = 0;
		pointyArrowInit = pointyArrow.rectTransform.anchoredPosition;
		cooldown = 0.1f;
		pc = FindObjectOfType<PlayerController>();
	}

	// Update is called once per frame
	void Update() {
		cooldown -= Time.deltaTime;
		if (isActive && cooldown < 0) {
			if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
				selectionIndex = 1 - selectionIndex;
				pointyArrow.rectTransform.anchoredPosition = pointyArrowInit + (selectionIndex * new Vector2(pointyArrowOffset, 0));
				cooldown = 0.1f;
			}
			else if (Input.GetKeyDown(KeyCode.Space)) {
				if (selectionIndex == 0) {
					warp();
				}
				closeMenu();
			}
		}
	}

	public void openMenuWithTarget(GameObject t, string name) {
		if (!isActive && cooldown < 0) {
			target = t;
			isActive = true;
			confirmationWindow.gameObject.SetActive(true);
			menuText.text = "Travel to " + name + "?";
			cooldown = 0.2f;
			pc.frozen = true;
		}
	}

	public void closeMenu() {
		if (cooldown < 0) {
			confirmationWindow.gameObject.SetActive(false);
			isActive = false;
			cooldown = 0.2f;
			pc.frozen = false;
		}
	}

	public void warp() {
		PlayerController pc = FindObjectOfType<PlayerController>();
		GameObject player = pc.gameObject;
		if (target != null) {
			player.transform.position = target.transform.position;
		}
	}
}