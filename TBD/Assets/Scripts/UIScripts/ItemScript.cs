using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour {

	private InventoryManager inventory;
	private GameObject itemMenu;
	private bool isActive = false;
	public GameObject blankItem;
	private PauseScript pause;

	// Use this for initialization
	void Start () {
		itemMenu = GetComponentInChildren<Image>().gameObject;
		itemMenu.SetActive(isActive);
		pause = FindObjectOfType<PauseScript>();
		inventory = FindObjectOfType<InventoryManager>();
		Item it1 = new Item("First Item", "This is a very long description", "?", false);
		Item it2 = new Item("Multiple Item", "How bout them items", "What", 3, false);
		Equipment armor = new Equipment("Basic Armor", 0, 1);
		Equipment weapon = new Equipment("Basic Weapon", 1, 0);
		Equipment equip1 = new Equipment("Empty Equipment Slot 3", 0, 0);
		Equipment equip2 = new Equipment("Empty Equipment Slot 4", 0, 0);
		Equipment equip3 = new Equipment("Empty Equipment Slot 5", 0, 0);
		
		inventory.addItem(it1);
		inventory.addItem(it2);
		inventory.addItem(armor);
		inventory.addItem(weapon);
		inventory.addItem(equip1);
		inventory.addItem(equip2);
		inventory.addItem(equip3);
		foreach (Item i in inventory.items) {
			GameObject newItem = Instantiate(blankItem, blankItem.transform.position, blankItem.transform.rotation);
			newItem.SetActive(true);
			Text newText = newItem.GetComponent<Text>();
			newText.text = i.name;
			if (i.quantity > 1) {
				newText.text += " (x" + i.quantity + ")";
			}
			newItem.transform.SetParent(blankItem.transform.parent);
			newItem.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 1.0f, 1.0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && pause.inItems) {
			itemsClose();
		}
	}

	public void itemsOpened() {
		itemMenu.SetActive(true);
	}

	public void itemsClose() {
		itemMenu.SetActive(false);
		pause.reopenFromInventory();
	}
}
