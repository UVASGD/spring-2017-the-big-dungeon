using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour {

	private Text fileName;
	private Text infoText;
	private SaveController sc;
	private int thisSlot = 0;

	// Use this for initialization
	void Start () {
		sc = FindObjectOfType<SaveController>();
		fileName = this.transform.GetChild(1).GetComponent<Text>();
		infoText = this.transform.GetChild(2).GetComponent<Text>();
		//updateSaveFiles();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setThisSlot(int slot) {
		this.thisSlot = slot;
	}

	public int getThisSlot() {
		return this.thisSlot;
	}

	public void updateSaveFiles() {
		sc = FindObjectOfType<SaveController>();
		fileName = this.transform.GetChild(1).GetComponent<Text>();
		infoText = this.transform.GetChild(2).GetComponent<Text>();
		sc.LoadFromSlot(thisSlot);
		fileName.text = sc.getCurrentName();
		infoText.text = "" + sc.curData.level;
	}
}
