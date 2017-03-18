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

	// Use this for initialization
	void Start () {
		statsMenu = GetComponentInChildren<Image>().gameObject;
		statsMenu.SetActive(isActive);
		pause = FindObjectOfType<PauseMenuUI>();
		statPanel = statsMenu.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && pause.inItems) {
			statsClose();
		}
	}

	public void statsOpened() {
		isActive = true;
		statsMenu.SetActive(true);
	}

	public void statsClose() {
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
			if (statText.text == s.statName) {
				statText.text = s.statName + ": " + s.baseVal;
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
		newText.text = s.statName + ": " + s.baseVal;
		if (s.modifier > 0)
			newText.text += " (+" + s.modifier + ")";
		if (s.modifier < 0)
			newText.text += " (" + s.modifier + ")";
		newStat.transform.SetParent(blankStat.transform.parent);
		newStat.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}

	void debug(string line) {
		if (debugOn) {
			Debug.Log(line);
		}
	}
}
