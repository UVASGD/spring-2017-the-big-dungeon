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

	// Use this for initialization
	void Start () {
		itemMenu = GetComponentInChildren<Image>().gameObject;
		itemMenu.SetActive(isActive);
		pause = FindObjectOfType<PauseScript>();
		inventory = FindObjectOfType<InventoryManager>();
		Item it1 = new Item("First Item", "This is a very long description", "Equipment", "?", 30, false);
		Item it2 = new Item("Multiple Item", "How bout them items", "Equipment", "What", 3, 30, false);
		Equipment armor = new Equipment("Basic Armor", "Adds defense and hp", 0, 1, 10);
		Equipment weapon = new Equipment("Basic Weapon","Adds strength", 1, 0, 0);		
		inventory.addItem(it1);
		inventory.addItem(it2);
		inventory.addItem(armor);
		inventory.addItem(weapon);
		
		foreach (Item i in inventory.items) {
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
}
