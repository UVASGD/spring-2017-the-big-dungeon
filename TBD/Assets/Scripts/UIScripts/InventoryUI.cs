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

	// Use this for initialization
	void Start () {
		itemMenu = GetComponentInChildren<Image>().gameObject;
		itemMenu.SetActive(isActive);
		pause = FindObjectOfType<PauseMenuUI>();
		itemPanel = itemMenu.GetComponentInChildren<GridLayoutGroup>().gameObject;
		res = Screen.currentResolution;
		inventory = FindObjectOfType<InventoryManager>();
		
		updatePanel();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && pause.inItems) {
			itemsClose();
		}
		if (!res.Equals(Screen.currentResolution)) {
			updatePanel();
		}
	}

	public void updatePanel() {
		parentWidth = (itemPanel.GetComponent<RectTransform>().rect.width / 2);
		itemPanel.GetComponent<GridLayoutGroup>().cellSize = new Vector2(parentWidth, 60);
		cellWidth = parentWidth;
		foreach (Transform child in itemPanel.transform) {
			updateItemLength(child);
		}
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
		itemMenu.SetActive(true);
	}

	public void itemsClose() {
        isActive = false;
		itemMenu.SetActive(false);
        pause.reopenFromInventory();
	}

    public void turnOff()
    {
        isActive = false;
        itemMenu.SetActive(false);
    }

	public void refreshItemListUI() {
		items = inventory.items;
		foreach (Item i in items) {
				addNewItemUI(i);
		}
	}

	public void updateItemQuantityUI(Item i) {
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
		items = inventory.items;
		Transform child = itemPanel.transform.GetChild((items.IndexOf(i)+1));
		Destroy(child.gameObject);
	}

	void debug(string line) {
		if (debugOn) {
			Debug.Log(line);
		}
	}


}
