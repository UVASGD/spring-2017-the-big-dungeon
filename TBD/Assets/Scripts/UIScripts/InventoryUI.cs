using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryUI : MonoBehaviour {

	private GameObject itemMenu;
	public bool isActive = false;
	public GameObject blankItem;
	private PauseMenuUI pause;
	private GameObject itemPanel;
	public bool debugOn = false;
	private Resolution res;
	private InventoryManager inventory;
	private List<Item> items = new List<Item>();
	private float cellWidth;
	private float parentWidth;

	private int xIndex = 0;
	private int yIndex = 0;
	private int totalX = 2;
	private int totalY = 10;
	private GameObject arrow;
	private Vector2 yOffset = new Vector3(0f, 60f);
	private Vector2 xOffset = new Vector3(500f, 0f);
	private Vector2 startPosition;
	private bool isOdd = false;
	private bool canClick = false;
	
	private int infoIndex = 0;
	private int infoMax = 3;
	public Vector2 infoXOffset;
	private Vector2 startPositionInfo;
	public GameObject itemInfoObject;
	public GameObject infoArrow;
	public Text itemName;
	public Text itemQuantity;
	public Text itemPrice;
	public Text itemSpecial;
	public Text itemDescription;
	private bool inInspect = false;

	// Use this for initialization
	void Start () {
		itemMenu = GetComponentInChildren<Image>().gameObject;
		itemMenu.SetActive(isActive);
		pause = FindObjectOfType<PauseMenuUI>();
		itemPanel = itemMenu.GetComponentInChildren<GridLayoutGroup>().gameObject;
		res = Screen.currentResolution;
		inventory = FindObjectOfType<InventoryManager>();
		arrow = itemMenu.GetComponentInChildren<Animator>().gameObject;
		startPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
		updatePanel();
		inventory.addStartItems(false);

		startPositionInfo = infoArrow.GetComponent<RectTransform>().anchoredPosition;
		// fix this
		float panelSize = infoArrow.transform.parent.transform.parent.GetComponent<RectTransform>().rect.width;
		infoXOffset = new Vector2(panelSize/3.3f, 0f);
		itemInfoObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (items.Count != inventory.items.Count) {
			updateArrowUI();
		}
		if (isActive && !inInspect) {
			if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
				if (yIndex > 0) {
					yIndex--;
					Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
					arrowPosition += yOffset;
					arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
					arrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				}
			}
			if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
				if ((yIndex < totalY - 1)) {
					if (!isOdd || !(xIndex == 1) || yIndex < totalY - 2) {
						yIndex++;
						Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
						arrowPosition -= yOffset;
						arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
						arrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
					}
				}
			}
			if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
				if (xIndex > 0) {
					xIndex--;
					Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
					arrowPosition -= xOffset;
					arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
					arrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				}
			}
			if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
				if ((xIndex < totalX - 1) && (!isOdd || !(yIndex == totalY - 1))) {
					xIndex++;
					Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
					arrowPosition += xOffset;
					arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
					arrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				}
			}
			if (Input.GetKeyDown(KeyCode.Space) && canClick) {
				int curItemIndex = (yIndex + 1) * 2 + xIndex - 2;
				inspectItem(curItemIndex);
				canClick = false;
			}
		}
		if (inInspect) {
			if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
				if (infoIndex > 0) {
					infoIndex--;
					Vector2 arrowPosition = infoArrow.GetComponent<RectTransform>().anchoredPosition;
					arrowPosition -= infoXOffset;
					infoArrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
					infoArrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				}
			}
			if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
				if (infoIndex < infoMax - 1) {
					infoIndex++;
					Vector2 arrowPosition = infoArrow.GetComponent<RectTransform>().anchoredPosition;
					arrowPosition += infoXOffset;
					infoArrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
					infoArrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				}
			}
			if (Input.GetKeyDown(KeyCode.Space) && canClick) {
				// switch on it
				switch (infoIndex) {
					case (0):
						//use, eventually have a pop-up for yes or no
						Debug.Log("use item");
						break;
					case (1):
						//drop then exit, eventually have pop-up for yes or no
						Debug.Log("drop item");
						break;
					case (2):
						//exit
						exitInfoMenu();
						break;
				}
				canClick = false;
			}
			if (Input.GetKeyDown(KeyCode.Escape)) {
				exitInfoMenu();
				canClick = false;
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape) && isActive && !inInspect && canClick) {
			Debug.Log("in here");
			turnOff();
		}
		if (!res.Equals(Screen.currentResolution)) {
			updatePanel();
		}
		canClick = true;
	}
	
	public void exitInfoMenu() {
		inInspect = false;
		canClick = false;
		itemInfoObject.SetActive(inInspect);
		infoArrow.GetComponent<RectTransform>().anchoredPosition = startPositionInfo;
		infoIndex = 0;
	}

	public void inspectItem(int curItemIndex) {
		inInspect = true;
		Item curItem = items[curItemIndex];
		itemName.text = curItem.name;
		itemQuantity.text = "QTY: " + curItem.quantity;
		itemPrice.text = "MUN: " + curItem.price;
		if (curItem.special) {
			itemSpecial.text = "SPC: Y";
		} else {
			itemSpecial.text = "SPC: N";
		}
		itemDescription.text = "Info:\n" + curItem.description;
		itemInfoObject.SetActive(inInspect);
		canClick = false;
	}

	public void updateArrowUI() {
		debug("in update arrow");
		items = inventory.items;
		if (items.Count % 2 == 1) {
			isOdd = true;
		} else {
			isOdd = false;
		}
		totalY = Mathf.CeilToInt(items.Count / 2f);
	}

	public void updatePanel() {
		parentWidth = (itemPanel.GetComponent<RectTransform>().rect.width / 2);
		itemPanel.GetComponent<GridLayoutGroup>().cellSize = new Vector2(parentWidth, 60);
		cellWidth = parentWidth;
		xOffset = new Vector3(cellWidth + 10, 0f);
		foreach (Transform child in itemPanel.transform) {
			updateItemLength(child);
		}
		updateArrowUI();
	}

	private void updateItemLength(Transform child) {
		Text itemText = child.GetComponent<Text>();
		string truncatedName = truncatedCharacterString(itemText);
		itemText.text = truncatedName;
	}

	private string truncatedCharacterString(Text t) {
		string newString = t.text;
		float textWidth = t.preferredWidth;
		while (cellWidth < textWidth) {
			newString = newString.Substring(0, newString.Length - 1);
			t.text = newString;
			textWidth = t.preferredWidth;
		}
		return newString;
	}

	public void itemsOpened() {
        isActive = true;
		canClick = false;
		itemMenu.SetActive(isActive);
		updateArrowUI();
	}
	
    public void turnOff()
    {
        isActive = false;
		inInspect = false;
		canClick = false;
		xIndex = 0;
		yIndex = 0;
		arrow.GetComponent<RectTransform>().anchoredPosition = startPosition;
		infoArrow.GetComponent<RectTransform>().anchoredPosition = startPositionInfo;
		itemMenu.SetActive(isActive);
		itemInfoObject.SetActive(inInspect);
		pause.reopenFromInventory();
    }

	public void refreshItemListUI() {
		if (inventory == null)
			inventory = FindObjectOfType<InventoryManager>();
		items = inventory.items;
		foreach (Item i in items) {
			addNewItemUI(i);
		}
		updateArrowUI();
	}

	public void updateItemQuantityUI(Item i) {
		if (itemPanel == null) {
			if (itemMenu == null)
				itemMenu = GetComponentInChildren<Image>().gameObject;
			itemPanel = itemMenu.GetComponentInChildren<GridLayoutGroup>().gameObject;
		}
		if (inventory == null)
			items = inventory.items;
		foreach (Transform child in itemPanel.transform) {
			Text itemText = child.GetComponent<Text>();
			// Potential problem with truncated characters
			if (itemText.text == i.name) {
				Text informationText = itemText.gameObject.transform.GetChild(0).GetComponent<Text>();
				if (i.quantity == 1) {
					itemText.text = i.name;
					informationText.text = "";
				}
				else {
					itemText.text = i.name;
					informationText.text = "(*" + i.quantity + ")";
				}
			}
		}
	}

	public void addNewItemUI(Item i) {
		if (inventory == null)
			inventory = FindObjectOfType<InventoryManager>();
		items = inventory.items;
		GameObject newItem = Instantiate(blankItem, blankItem.transform.position, blankItem.transform.rotation);
		newItem.SetActive(true);
		Text newText = newItem.GetComponent<Text>();
		Text informationText = newText.gameObject.transform.GetChild(0).GetComponent<Text>();
		newText.text = i.name;
		if (i.quantity > 1) {
			informationText.text = "(*" + i.quantity + ")";
		}
		if (i.quantity == 1) {
			informationText.text = "";
		}
		newItem.transform.SetParent(blankItem.transform.parent);
		newItem.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}

	public void removeItemUI(Item i) {
		if (inventory == null)
			inventory = FindObjectOfType<InventoryManager>();
		items = inventory.items;
		if (itemPanel == null) {
			if (itemMenu == null)
				itemMenu = GetComponentInChildren<Image>().gameObject;
			itemPanel = itemMenu.GetComponentInChildren<GridLayoutGroup>().gameObject;
		}
		Transform child = itemPanel.transform.GetChild((items.IndexOf(i) + 2));
		Destroy(child.gameObject);
	}

	void debug(string line) {
		if (debugOn) {
			Debug.Log(line);
		}
	}


}
