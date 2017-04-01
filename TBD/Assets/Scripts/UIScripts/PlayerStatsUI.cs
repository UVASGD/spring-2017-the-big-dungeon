using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStatsUI : MonoBehaviour {

	private GameObject statsMenu;
	public bool isActive = false;
	public GameObject blankStat;
	private PauseMenuUI pause;
	private GameObject statPanel;
	public bool debugOn = false;
	private PlayerController player;

	// Use this for initialization
	void Start () {
		statsMenu = GetComponentInChildren<Image>().gameObject;
		statsMenu.SetActive(isActive);
		pause = FindObjectOfType<PauseMenuUI>();
		statPanel = statsMenu.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
		player = FindObjectOfType<PlayerController>();

		updateName(player.getPlayerName());
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && isActive) {
			statsClose();
		}
	}

	public void updateName(string name) {
		if (statPanel == null) {
			statsMenu = GetComponentInChildren<Image>().gameObject;
			statPanel = statsMenu.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
		}
		statPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = name;
	}

	public void statsOpened() {
		foreach (BaseStat s in player.stats) {
			updateStats (s);
		}
		isActive = true;
		statsMenu.SetActive(true);
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
		debug("Updating for stat " + s.statName);
		foreach (Transform child in statPanel.transform) {
			Text statText = child.GetComponent<Text>();
			string[] textBits = statText.text.Split (new char[]{ ':' });
			if (textBits[0].Trim().Equals(s.statName)) {
				statText.text = " " + s.statName + ": " + s.baseVal;
				if (s.modifier > 0)
					statText.text += " (+" + s.modifier + ")";
				if (s.modifier < 0)
					statText.text += " (" + s.modifier + ")";
			}
		}
	}

	public void addStat(BaseStat s) {
		debug("Adding stat " + s.statName);
		GameObject newStat = Instantiate(blankStat, blankStat.transform.position, blankStat.transform.rotation);
		newStat.SetActive(true);
		Text newText = newStat.GetComponent<Text>();
		newText.text = " " + s.statName + ": " + s.baseVal;
		if (s.modifier > 0)
			newText.text += " (+" + s.modifier + ")";
		if (s.modifier < 0)
			newText.text += " (" + s.modifier + ")";
		newStat.transform.SetParent(blankStat.transform.parent);
		newStat.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}

	public void addEquipment(Item i) {
		bool didAdd = false;
		string itemType = "Wrong";
		switch (i.type) {
		case Item.ItemType.Hat:
			itemType = "Hat";
			break;
		case Item.ItemType.Body:
			itemType = "Body";
			break;
		case Item.ItemType.Weapon:
			itemType = "Weapon";
			break;
		}
		foreach (Transform child in statPanel.transform) {
			Text statText = child.GetComponent<Text>();
			string[] textBits = statText.text.Split (new char[]{ ':' });
			if (textBits [0].Trim () == itemType) {
				statText.text = " " + itemType + ": " + i.name;
				didAdd = true;
			}
		}

		//If it is an equipment, but was not updated, then we need to add a new element to the UI
		if ((i.type == Item.ItemType.Hat || i.type == Item.ItemType.Body || i.type == Item.ItemType.Weapon) && !didAdd) {
			GameObject newStat = Instantiate(blankStat, blankStat.transform.position, blankStat.transform.rotation);
			newStat.SetActive(true);
			Text newText = newStat.GetComponent<Text>();
			newText.text = " " + itemType + ": " + i.name;
			newStat.transform.SetParent(blankStat.transform.parent);
			newStat.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}
	}

	void debug(string line) {
		if (debugOn) {
			Debug.Log(line);
		}
	}
}
