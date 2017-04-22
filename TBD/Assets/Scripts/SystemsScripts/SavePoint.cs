using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {

	public SaveMenuUI suic;

	bool withinRange;

	// Use this for initialization
	void Start () {
		suic = FindObjectOfType<SaveMenuUI> ();
		withinRange = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (withinRange) {
			suic = FindObjectOfType<SaveMenuUI>();
			PauseMenuUI pauseMenu = FindObjectOfType<PauseMenuUI> ();
			if (Input.GetKeyDown (KeyCode.Space) && !suic.selection && !pauseMenu.isActive) {
				suic.SetSelectionMenu (true);
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
