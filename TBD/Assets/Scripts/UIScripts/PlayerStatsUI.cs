using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStatsUI : MonoBehaviour {

	private GameObject statsMenu;
	public bool isActive = false;
	public GameObject blankStat;
	private PauseMenuUI pause;
	public bool debugOn = false;
	private PlayerController player;

	public Text nameText, hp, str, def, hat, bod, wep;
	public GameObject confirmationWindow;

	public Text cursor;
	int cursorIndex;
	public float cursorOffset;
	Vector2 cursorInitPosition;

	bool confirming;
	public Text confirmationArrow;
	public float confirmationOffset;
	int confirmationIndex;
	Vector2 confirmationInitPosition;
	private float cooldown;

	// Use this for initialization
	void Start () {
		statsMenu = GetComponentInChildren<Image>().gameObject;
		statsMenu.SetActive(isActive);
		pause = FindObjectOfType<PauseMenuUI>();
		player = FindObjectOfType<PlayerController>();

		updateName(player.getPlayerName());
		cursorIndex = 0;
		confirmationIndex = 0;
		confirming = false;
		cursorInitPosition = cursor.rectTransform.localPosition;
		confirmationInitPosition = confirmationArrow.rectTransform.localPosition;
		cooldown = 0;
	}

	// Update is called once per frame
	void Update () {
		if (cooldown > 0) {
			cooldown -= Time.deltaTime;
			return;
		}
		if (isActive) {
			if (Input.GetKeyDown (KeyCode.Escape) && isActive) {
				if (confirming) {
					confirming = false;
					confirmationWindow.SetActive (false);
				} else {
					statsClose ();
				}
			}
			if (!confirming) {
				if (Input.GetKeyDown (KeyCode.W) || Input.GetKeyDown (KeyCode.UpArrow)) {
					cursorIndex = (cursorIndex == 0) ? 2 : cursorIndex - 1; //moves up, loops if at top
				} else if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) {
					cursorIndex = (cursorIndex + 1) % 3; //moves down, loops if at bottom
				} else if (Input.GetKeyDown (KeyCode.Space)) {
					confirming = true;
					confirmationWindow.SetActive (true);
				}
				cursor.rectTransform.localPosition = cursorInitPosition - (cursorIndex * new Vector2 (0, cursorOffset));
			} else {
				if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.LeftArrow)) {
					confirmationIndex = 0;
					confirmationArrow.rectTransform.localPosition = confirmationInitPosition;
				} else if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.RightArrow)) {
					confirmationIndex = 1;
					confirmationArrow.rectTransform.localPosition = confirmationInitPosition + new Vector2 (confirmationOffset, 0);
				} else if (Input.GetKeyDown (KeyCode.Space)) {
					if (confirmationIndex == 0) {
						switch (cursorIndex) {
						case 0:
							FindObjectOfType<InventoryManager> ().unEquipHat ();
							hat.text = "  HAT:";
							break;
						case 1:
							FindObjectOfType<InventoryManager> ().unEquipBody ();
							bod.text = "  BODY:";
							break;
						case 2:
							FindObjectOfType<InventoryManager> ().unEquipWeapon ();
							wep.text = "  WEAPON:";
							break;
						}
						foreach (BaseStat b in player.stats) {
							updateStats (b);
						}
					}
					confirming = false;
					confirmationWindow.SetActive (false);
				}
			}
		}
	}

	public void updateName(string newName) {
		nameText.text = newName;
	}

	public void statsOpened() {
		foreach (BaseStat s in player.stats) {
			updateStats (s);
		}
		isActive = true;
		statsMenu.SetActive(true);
		cooldown = 0.1f;
	}

	public void statsClose() {
		debug("called stats close");
		isActive = false;
		statsMenu.SetActive(false);
		pause.reopenFromStats();
	}

	public void turnOff()
	{
		isActive = false;
		statsMenu.SetActive(false);
	}

	public void updateStats(BaseStat s) {
		
		switch (s.statName) {
		case "HP":
			hp.text = "HP: " + s.baseVal + " (" + s.modifier + ")";
			break;
		case "str":
			str.text = "STRENGTH: " + s.baseVal + " (" + s.modifier + ")";
			break;
		case "def":
			def.text = "DEFENSE: " + s.baseVal + " (" + s.modifier + ")";
			break;
		}
	}

	public void addStat(BaseStat s) {
		
	}

	public void addEquipment(Item i) {
		switch (i.type) {
		case Item.ItemType.Hat:
			hat.text = "  HAT: " + i.name;
			break;
		case Item.ItemType.Body:
			bod.text = "  BODY: " + i.name;
			break;
		case Item.ItemType.Weapon:
			wep.text = "  WEAPON: " + i.name;
			break;
		}
	}

	void debug(string line) {
		if (debugOn) {
			Debug.Log(line);
		}
	}
}
