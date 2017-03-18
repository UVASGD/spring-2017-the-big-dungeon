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
	private SaveController save;
	private ScreenFader sf;
	private Vector2 startPosition;
	private Canvas optionsMenu;
	public bool inOptions = false;
	private List<GameObject> optionParts = new List<GameObject>();
	private InventoryUI inventoryMenu;
    private SellMenuUI sellObject;
    private BuyMenuUI buyObject;
	private PlayerStatsUI statsMenu;
	public bool inItems = false;
	public bool canEscape = false;
	public bool inStats = false;

	public int totalOptions = 7;
	public bool debugOn = false;



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
		save = FindObjectOfType<SaveController>();
		optionsMenu = FindObjectOfType<OptionsMenuUI>().gameObject.GetComponent<Canvas>();
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
		if (Input.GetKeyDown(KeyCode.Return) && !player.talking && !inItems && !inOptions && !inStats) {
            if (buyObject != null)
                buyObject.turnOff();
            if (sellObject != null)
                sellObject.turnOff();
			toggleMenu();
		}
		if (isActive && !inOptions && !inItems && !inStats) {
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
						break;
					//Item
					case 2:
						debug("Open ITEM");
						inventoryMenu.itemsOpened();
						inItems = true;
						canEscape = false;
						//toggleMenu();
						break;
					//Player
					case 3:
						debug ("Open PLAYER");
						statsMenu.statsOpened ();
						inStats = true;
						canEscape = false;
						break;
					//Option
					case 4:
						debug("Open OPTION");
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
		if (inOptions || inItems || isActive || inStats) {
			player.frozen = true;
		}
	}

	void toggleMenu() {
		isActive = !isActive;
		debug(isActive + "");
		pauseMenu.SetActive(isActive);
		player.frozen = isActive;
		player.inMenu = isActive;
		index = 0;
		arrow.GetComponent<RectTransform>().anchoredPosition = startPosition;
	}

	public void exitMenu() {
		isActive = true;
		inItems = false;
		inStats = false;
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
        toggleMenu();
        inItems = false;
		canEscape = false;
		
		index = 2;
		Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
		arrowPosition -= yOffset * index;
		arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
        canEscape = true;
	}

	public void reopenFromStats() {
		toggleMenu();
		inStats = false;
		canEscape = false;

		index = 3;
		Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
		arrowPosition -= yOffset * index;
		arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
		canEscape = true;
	}

	void debug(string line) {
		if (debugOn) {
			Debug.Log(line);
		}
	}

}
