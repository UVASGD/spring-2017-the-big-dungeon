using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour {

	private GameObject pauseMenu;
	private bool isActive = false;
	private PlayerController player;
	private GameObject arrow;
	private int index = 0;
	private Vector2 yOffset = new Vector3(0f, 105f);
	private SaveController save;
	private ScreenFader sf;
	private Vector2 startPosition;
	private Canvas optionsMenu;
	public bool inOptions = false;
	private List<GameObject> optionParts = new List<GameObject>();
	private ItemScript itemMenu;
	public bool inItems = false;
	public bool canEscape = false;

	public int totalOptions = 7;



	// Use this for initialization
	void Start() {
		pauseMenu = GetComponentInChildren<Image>().gameObject;
		pauseMenu.SetActive(isActive);
		itemMenu = FindObjectOfType<ItemScript>();
		player = FindObjectOfType<PlayerController>();
		arrow = pauseMenu.GetComponentInChildren<Animator>().gameObject;
		startPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
		save = FindObjectOfType<SaveController>();
		optionsMenu = FindObjectOfType<SettingScript>().gameObject.GetComponent<Canvas>();
		foreach (Image i in optionsMenu.GetComponentsInChildren<Image>()) {
			optionParts.Add(i.gameObject);
		}
		try {
			sf = FindObjectOfType<ScreenFader>();
		} catch {
			sf = null;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && canEscape) {
			exitMenu();
		}
		if (Input.GetKeyDown(KeyCode.Return) && !player.talking) {
			toggleMenu();
		}
		if (isActive && !inOptions && !inItems) {
			if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
				if (index > 0) {
					index--;
					Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
					arrowPosition += yOffset;
					arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
					arrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				}
			}
			else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
				if (index < totalOptions - 1) {
					index++;
					Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
					arrowPosition -= yOffset;
					arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
					arrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				}
			}
			else if (Input.GetKeyDown(KeyCode.Space)) {
				switch (index) {
					//Resume
					case 0:
						toggleMenu();
						break;
					//Bag
					case 1:
						Debug.Log("Open BAG");
						break;
					//Item
					case 2:
						Debug.Log("Open ITEM");
						itemMenu.itemsOpened();
						inItems = true;
						canEscape = false;
						toggleMenu();
						break;
					//Player
					case 3:
						Debug.Log("Open PLAYER");
						break;
					//Option
					case 4:
						Debug.Log("Open OPTION");
						OptionsOpened();
						break;
					//Save
					case 5:
						// eventually player name
						save.SaveTo("default");
						break;
					//Exit
					case 6:
						// Could break a lot of stuff switching between scenes
						if (sf != null) {
							sf.BlackOut();
						}
						SceneManager.LoadScene(0);
						break;
					default:
						break;
				}
			}
		}
		if (inOptions || inItems || isActive) {
			player.frozen = true;
		}
	}

	void toggleMenu() {
		isActive = !isActive;
		pauseMenu.SetActive(isActive);
		player.frozen = isActive;
		player.inMenu = isActive;
		index = 0;
		arrow.GetComponent<RectTransform>().anchoredPosition = startPosition;
	}

	void exitMenu() {
		isActive = true;
		inItems = false;
		inOptions = false;
		canEscape = false;
		toggleMenu();
	}

	public void OptionsOpened() {
		foreach (GameObject g in optionParts) {
			g.SetActive(true);
		}
		optionsMenu.enabled = true;
		inOptions = true;
		canEscape = false;
	}

	public void OptionsClose() {
		foreach (GameObject g in optionParts) {
			g.SetActive(false);
		}
		optionsMenu.enabled = false;
		inOptions = false;
		canEscape = true;
	}

	public void reopenFromInventory() {
		inItems = false;
		canEscape = true;
		toggleMenu();
		index = 2;
		Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
		arrowPosition -= yOffset * index;
		arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
	}

}
