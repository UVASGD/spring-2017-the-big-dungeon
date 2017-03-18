using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour {

	private InventoryManager inventory;
	private GameObject itemMenu;
	public bool isActive = false;
	public GameObject blankItem;
	private PauseScript pause;
	private GameObject itemPanel;
	public bool debugOn = false;

	// Use this for initialization
	void Start () {
		itemMenu = GetComponentInChildren<Image>().gameObject;
		itemMenu.SetActive(isActive);
		pause = FindObjectOfType<PauseScript>();
		inventory = FindObjectOfType<InventoryManager>();
		itemPanel = itemMenu.GetComponentInChildren<VerticalLayoutGroup>().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && pause.inItems) {
			itemsClose();
		}
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

	public void updateItems(Item i) {
		debug("Updating for item " + i.name);
		foreach (Transform child in itemPanel.transform) {
			Text itemText = child.GetComponent<Text>();
			string[] result = itemText.text.Split(new string[] { " (*" }, System.StringSplitOptions.None);
			if (result[0] == i.name) {
				itemText.text = i.name + " (*" + i.quantity + ")";
			}
		}
	}

	public void removeItem(Item i) {
		debug("Removing item " + i.name);
		foreach (Transform child in itemPanel.transform) {
			Text itemText = child.GetComponent<Text>();
			string[] result = itemText.text.Split(new string[] { " (*" }, System.StringSplitOptions.None);
			if (result[0] == i.name) {
				Destroy(child.gameObject);
			}
		}
	}

	public void addItem(Item i) {
		debug("Adding item " + i.name);
		GameObject newItem = Instantiate(blankItem, blankItem.transform.position, blankItem.transform.rotation);
		newItem.SetActive(true);
		Text newText = newItem.GetComponent<Text>();
		newText.text = i.name;
		if (i.quantity > 1) {
			newText.text += " (*" + i.quantity + ")";
		}
		newItem.transform.SetParent(blankItem.transform.parent);
		newItem.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}

	void debug(string line) {
		if (debugOn) {
			Debug.Log(line);
		}
	}


}
