using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour {

	private GameObject pauseMenu;
	private bool isActive = false;
	private PlayerController player;
	private GameObject arrow;
	private int index = 0;
	private Vector2 yOffset = new Vector3(0f, 105f);
	private ScreenFader sf;
	private Vector2 startPosition;
	public OptionsMenuUI optionsMenu;
	public bool inOptions = false;
	private List<GameObject> optionParts = new List<GameObject>();
	private InventoryUI inventoryMenu;
    private SellMenuUI sellObject;
    private BuyMenuUI buyObject;
	private PlayerStatsUI statsMenu;
	public bool inItems = false;
	public bool canEscape = false;
	public bool inStats = false;

	public int totalOptions = 5;
	public bool debugOn = false;

	private bool escapeWait = true;
	public GameObject exitConfirmMenu;
	public bool inExit = false;
	private int exitIndex = 0;
	private GameObject exitArrow;
	private Vector2 exitOffset = new Vector2(285f, 0f);


	// Use this for initialization
	void Start() {
		pauseMenu = GetComponentInChildren<Image>().gameObject;
		pauseMenu.SetActive(isActive);
        sellObject = FindObjectOfType<SellMenuUI>();
        buyObject = FindObjectOfType<BuyMenuUI>();
		inventoryMenu = FindObjectOfType<InventoryUI>();
		statsMenu = FindObjectOfType<PlayerStatsUI>();
		player = FindObjectOfType<PlayerController>();
		arrow = pauseMenu.GetComponentInChildren<Animator>().gameObject;
		startPosition = arrow.GetComponent<RectTransform>().anchoredPosition;

		if (optionsMenu == null) {
			optionsMenu = FindObjectOfType<OptionsMenuUI>();
		}
		foreach (Image i in optionsMenu.GetComponentsInChildren<Image>()) {
			optionParts.Add(i.gameObject);
		}
		try {
			sf = FindObjectOfType<ScreenFader>();
		} catch {
			sf = null;
		}
		exitArrow = exitConfirmMenu.GetComponentInChildren<Animator>().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && canEscape && isActive && !escapeWait) {
            exitMenu();
		}
		if (inExit) {
			if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
				if (exitIndex > 0) {
					exitIndex--;
					Vector2 arrowPosition = exitArrow.GetComponent<RectTransform>().anchoredPosition;
					arrowPosition -= exitOffset;
					exitArrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
					exitArrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				}
			}
			else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
				if (exitIndex < 1) {
					exitIndex++;
					Vector2 arrowPosition = exitArrow.GetComponent<RectTransform>().anchoredPosition;
					arrowPosition += exitOffset;
					exitArrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
					exitArrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				}
			}
			else if (Input.GetKeyDown(KeyCode.Space)) {
				Debug.Log(exitIndex);
				switch (exitIndex) {
					case 0:
						exitConfirm();
						break;
					case 1:
						exitCancel();
						break;
				}
			}
			else if (Input.GetKeyDown(KeyCode.Escape)) {
				exitCancel();
			}
		}
		if (Input.GetKeyDown(KeyCode.Return) && !player.talking && !inItems && !inOptions && !inStats && !inExit) {
            if (buyObject != null)
                buyObject.turnOff();
            if (sellObject != null)
                sellObject.turnOff();
			toggleMenu();
		}
		if (isActive && !inOptions && !inItems && !inStats && !inExit && !escapeWait) {
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
						debug("Open BAG");
						inventoryMenu.itemsOpened();
						inItems = true;
						canEscape = false;
						//toggleMenu();
						break;
					//Player
					case 2:
						debug ("Open PLAYER");
						statsMenu.statsOpened ();
						inStats = true;
						canEscape = false;
						break;
					//Option
					case 3:
						debug("Open OPTION");
						OptionsOpened();
						break;
					//Exit
					case 4:
						debug("Open EXIT");
						ExitOpened();
						break;
					default:
						break;
				}
			}
		}
		if (inOptions || inItems || isActive || inStats || inExit) {
			player.frozen = true;
		}
		escapeWait = false;
	}

	void toggleMenu() {
		isActive = !isActive;
		debug(isActive + "");
		pauseMenu.SetActive(isActive);
		player.frozen = isActive;
		player.inMenu = isActive;
		index = 0;
		arrow.GetComponent<RectTransform>().anchoredPosition = startPosition;
		canEscape = !canEscape;
	}

	public void exitMenu() {
		isActive = true;
		inItems = false;
		inStats = false;
		inOptions = false;
		inExit = false;
		toggleMenu();
	}

	public void ExitOpened() {
		exitConfirmMenu.SetActive(true);
		inExit = true;
		canEscape = false;
	}

	public void exitConfirm() {
		// Could break a lot of stuff switching between scenes
		if (sf != null) {
			sf.BlackOut();
		}
		SceneManager.LoadScene(0);
	}

	public void exitCancel () {
		exitIndex = 0;
		Vector2 arrowPosition = exitArrow.GetComponent<RectTransform>().anchoredPosition;
		arrowPosition -= exitOffset;
		exitArrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
		exitConfirmMenu.SetActive(false);
		inExit = false;
		canEscape = true;
		escapeWait = true;
		
	}

	public void OptionsOpened() {
		foreach (GameObject g in optionParts) {
			g.SetActive(true);
		}
		optionsMenu.toggleMenu();
		inOptions = true;
		canEscape = false;
	}

	public void OptionsClose() {
		foreach (GameObject g in optionParts) {
			g.SetActive(false);
		}
		optionsMenu.toggleMenu();
		inOptions = false;
		canEscape = true;
		escapeWait = true;
	}

	public void reopenFromInventory() {
        inItems = false;
		canEscape = true;
		escapeWait = true;
	}

	public void reopenFromStats() {
		inStats = false;
		canEscape = true;
		escapeWait = true;
	}

	void debug(string line) {
		if (debugOn) {
			Debug.Log(line);
		}
	}

}
