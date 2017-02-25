using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Reflection;

public class CreditsScript : MonoBehaviour {

	private List<string> creditsLines = new List<string>();
	public TextAsset inputFile;
	public GameObject firstLine;
	private bool isHeader = true;
	private float totalHeight = 380;
	private Vector3 origPosition;
	private float middleOfScreen;
	private bool creditsRolling = false;
	private bool creditsPause = false;

	public int gap = 100;
	public int headerSize = 70;
	public int headerHeight = 120;
	public int nameSize = 50;
	public int nameHeight = 90;

	public float scrollSpeed = 0.3f;

	private MenuScript ms;
	private MusicController mc;

	private float waitTimer = 0.0f;
	public float creditWaitTime = 10.0f;

	// Use this for initialization
	void Start () {
		ms = FindObjectOfType<MenuScript>();
		mc = FindObjectOfType<MusicController>();
		origPosition = GetComponent<RectTransform>().anchoredPosition;
		middleOfScreen = -origPosition.y/2.0f;
		string wholeFile = inputFile.text;
		creditsLines.AddRange(wholeFile.Split("\n"[0]));
		foreach (string line in creditsLines) {
			GameObject newItem = Instantiate(firstLine, firstLine.transform.position, firstLine.transform.rotation);
			Text newText = newItem.GetComponent<Text>();
			newText.text = line;
			newItem.transform.SetParent(this.gameObject.transform);
			newItem.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
			if (isHeader && line.Length != 1) {
				newText.color = Color.gray;
				newText.fontSize = headerSize;
				newText.fontStyle = FontStyle.Italic;
				newItem.GetComponent<RectTransform>().sizeDelta = new Vector2(0, headerHeight);
				totalHeight += headerHeight;
				isHeader = false;
			}
			else if (line.Length != 1){
				newText.color = Color.white;
				newText.fontSize = nameSize;
				newItem.GetComponent<RectTransform>().sizeDelta = new Vector2(0, nameHeight);
				totalHeight += nameHeight;
				isHeader = false;
			}
			if (line.Length == 1) {
				newItem.GetComponent<RectTransform>().sizeDelta = new Vector2(0, gap);
				totalHeight += gap;
				isHeader = true;
			}
			if (line == creditsLines[creditsLines.Count - 1]) {
				newText.color = Color.white;
				newText.fontStyle = FontStyle.Normal;
			}
		}
		GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, totalHeight);
		
	}

	// Update is called once per frame
	void Update () {
		if (creditsRolling) {
			this.transform.position = this.transform.position + new Vector3(0.0f, scrollSpeed, 0.0f);
			if (GetComponent<RectTransform>().anchoredPosition.y >= GetComponent<RectTransform>().sizeDelta.y - middleOfScreen) {
				creditsRolling = false;
				creditsPause = true;
			}
		}
		if (creditsPause) {
			waitTimer += Time.deltaTime;
			if (waitTimer >= creditWaitTime) {
				creditsPause = false;
				waitTimer = 0.0f;
				this.GetComponent<RectTransform>().anchoredPosition = origPosition;
				endCredit();
			}
		}
	}

	void endCredit() {
		ms.endOfCredits();
		mc.SwitchTrack(2, 0.2f, 0.1f);
	}

	public void startCredits() {
		creditsRolling = true;
		mc.SwitchTrack(3);
	}

}
