using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveUIController : MonoBehaviour {

	public Image confirmationWindow;
	public Image selectionWindow;

	public Text ConfArrow;
	public Text SelectArrow;

	public float ConfirmationArrowOffset,SelectArrowOffset;

	Vector2 confArrowInit,selectArrowInit;

	public bool confirmation, selection;
	int confIndex, selIndex;

	SaveController sc;
	PlayerController player;

	float cooldown;

	public bool debugOn = false;

	// Use this for initialization
	void Start () {
		confirmation = false;
		selection = false;
		confIndex = 0;
		selIndex = 0;
		confArrowInit = ConfArrow.rectTransform.anchoredPosition;
		selectArrowInit = SelectArrow.rectTransform.anchoredPosition;
		sc = FindObjectOfType<SaveController> ();
		player = FindObjectOfType<PlayerController> ();
		cooldown = -1;
	}
	
	// Update is called once per frame
	void Update () {
		cooldown = (cooldown > 0) ? cooldown - Time.deltaTime : cooldown;
		if (cooldown > 0) {
			return;
		}
		if (confirmation) {
			if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.D) || Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetKeyDown (KeyCode.RightArrow)) {
				confIndex = 1 - confIndex;
				ConfArrow.rectTransform.anchoredPosition = confArrowInit + confIndex * (new Vector2 (ConfirmationArrowOffset, 0));
			} else if (Input.GetKeyDown (KeyCode.Space)) {
				if (confIndex == 0) {
					Save ();
				} 
				ExitMenu ();
			}
		} else if (selection) {
			if ((Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) && (selIndex > 0)) {
				selIndex--;
				SelectArrow.rectTransform.anchoredPosition = selectArrowInit - selIndex * (new Vector2 (0, SelectArrowOffset));
			} else if ((Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) && (selIndex < 3)) {
				selIndex++;
				SelectArrow.rectTransform.anchoredPosition = selectArrowInit - selIndex * (new Vector2 (0, SelectArrowOffset));
			} else if(Input.GetKeyDown(KeyCode.Space)) {
				debug(selIndex + "");
				if (selIndex < 3) {
					SetConfirmationWindow (true);
				} else {
					ExitMenu ();
				}
			}
		}
	}

	void Save() {
		sc.SaveTo ("slot" + (selIndex + 1));
	}

	void ExitMenu() {
		SetSelectionMenu (false);
		SetConfirmationWindow (false);
		confIndex = 0;
		ConfArrow.rectTransform.anchoredPosition = confArrowInit;
		selIndex = 0;
		SelectArrow.rectTransform.anchoredPosition = selectArrowInit;
		player.frozen = false;
		debug("EXIT");
		cooldown = 0.1f;
	}

	public void SetSelectionMenu(bool status) {
		if (cooldown > 0) {
			return;
		}
		selection = status;
		selectionWindow.gameObject.SetActive (status);
		player.frozen = (confirmation || selection);
		if (status) {
			cooldown = 0.1f;
		}
	}

	public void SetConfirmationWindow(bool status) {
		if (cooldown > 0) {
			return;
		}
		confirmation = status;
		confirmationWindow.gameObject.SetActive (status);
		player.frozen = (confirmation || selection);
	}

	void debug(string line) {
		if (debugOn) {
			Debug.Log(line);
		}
	}
}
