using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance = null;
    public List<Item> items = new List<Item>();
    public int money;
    public int maxSize = 20;
	//private int currentSize;
	private InventoryUI inventoryMenu;
	public bool debugOn = false;

	private void Awake()
    {
        //Make sure only ever one InventoryManager
        if (instance == null)
        {
            instance = this;
        }else if (instance != this)
        {
            Destroy(gameObject);
        }
    } 
    // Use this for initialization
    void Start()
    {
        //amount for testing
        money = 80;
		DontDestroyOnLoad(gameObject);
		inventoryMenu = FindObjectOfType<InventoryUI>();
	}

	public void refreshItems() {
		if (inventoryMenu == null)
			inventoryMenu = FindObjectOfType<InventoryUI>();
		inventoryMenu.refreshItemListUI();
	}

	public void addStartItems(bool isContinuing) {
		if (!isContinuing) {
			Item it1 = new Item("First Item", "This is a very long description", new List<string>(){"str:+1","hp:+1"}, 2, 30, true, Item.ItemType.Consumable);
			Item it2 = new Item("Multiple Item", "How bout them items",  new List<string>(), 3, 30, true, Item.ItemType.Consumable);
			Item armor = new Item ("Basic Armor", "Adds defense and hp", new List<string>{ "str:+0", "def:+1", "hp:10" }, 1, 30, false, Item.ItemType.Body);
			Item stronk = new Item ("Stronkifier", "Makes Stronk-er", new List<string>{ "str:+10" }, 4, 100, true, Item.ItemType.Consumable);
			//Equipment weapon = new Equipment("Basic Weapon", "Adds strength", 1, 0, 0);
			addItem(it1);
			addItem(it2);
			addItem(armor);
			addItem (stronk);
			//addItem(weapon);
		}
	}

    public int spaceRemaining()
    {
        return maxSize - items.Count;
    }

    public void addItem(Item item)
    {
		if (inventoryMenu == null)
			inventoryMenu = FindObjectOfType<InventoryUI>();
		//If item already in inventory, increment quantity of item
		if (items.Contains(item)) {
			Item currentItem = items[items.IndexOf(item)];
			currentItem.quantity += item.quantity;
			inventoryMenu.updateItemQuantityUI(currentItem);
			return;
		}
		//If not, add the item
		else {
			items.Add(item);
			inventoryMenu.addNewItemUI(item);
		}
	}

    //Maybe have drop make it so you can pick it up again???
    public void dropItem()
    {

    }

    //Destroy A Specified Number of Items
    public bool destroyItem(Item item, int quantity)
    {
		if (items.Contains(item)) {
			Item currentItem = items[items.IndexOf(item)];
			if (inventoryMenu == null)
				inventoryMenu = FindObjectOfType<InventoryUI>();
			if (quantity >= currentItem.quantity) {
				inventoryMenu.removeItemUI(currentItem);
				items.Remove(currentItem);
			}
			else {
				currentItem.quantity -= quantity;
				inventoryMenu.updateItemQuantityUI(currentItem);
				return true;
			}
			return true;
		}
		return false;
    }

	public bool useItem(Item item, int quantity) {
		if (items.Contains (item)) {
			Debug.Log ("Using");
			Item currentItem = items[items.IndexOf(item)];
			if (inventoryMenu == null)
				inventoryMenu = FindObjectOfType<InventoryUI>();
			if (quantity >= currentItem.quantity) {
				int times = currentItem.quantity;
				for (int i = 0; i < times; i++) {
					currentItem.useItem ();
				}
				inventoryMenu.removeItemUI(currentItem);
				items.Remove(currentItem);
			}
			else {
				for (int i = 0; i < quantity; i++) {
					currentItem.useItem ();
				}
				inventoryMenu.updateItemQuantityUI(currentItem);
				return true;
			}
			return true;
		}
		return false;
	}

	public bool useItem(Item item) {
		return useItem (item, 1);
	}

    //Destroy All Items Passed In
    public bool destroyItem(Item item)
    {
		if (items.Contains(item)) {
			debug("going to destroy " + item.name);
			Item currentItem = items[items.IndexOf(item)];
			inventoryMenu.removeItemUI(item);
			items.Remove(currentItem);
			return true;
		}
		return false;
    }

	public int quantityItem(Item item) {
		if (items.Contains(item)) {
			Item currentItem = items[items.IndexOf(item)];
			return currentItem.quantity;
		}
		return -1;
	}

	/*
	public void testItem(Item item) {
		if (items.Contains(item)) {
			Debug.Log("Contains");
			Debug.Log(items.IndexOf(item));
		}
		for (int i = 0; i < items.Count; i++) {
			if (items[i].name.Equals(item.name)) {
				Debug.Log("Equals name");
			}
		}
	} */

	// Update is called once per frame
	void Update() {

	}

	void debug(string line) {
		if (debugOn) {
			Debug.Log(line);
		}
	}
}

/* [Serializable]
public class Item
{
    public bool special { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    //public string slug { get; set; }
    public int quantity { get; set; }
    public int price { get; set; }
    public string type { get; set; }
    public Item(string name, string description, string type, string slug , int quantity, int price, bool special)
    {
        this.name = name;
        this.description = description;
        //this.slug = slug;
        this.quantity = quantity;
        this.special = special;
        this.type = type;
        this.price = price;
    }
    public Item(string name, string description, string type, string slug , int price, bool special)
    {
        this.name = name;
        this.description = description;
        //this.slug = slug;
        this.special = special;
        this.quantity = 1;
        this.type = type;
        this.price = price;
    }
    //Copy constructor with different quantity
    public Item(Item item, int quantity)
    {
        this.name = item.name;
        this.description = item.description;
        //this.slug = item.slug;
        this.quantity = quantity;
        this.type = item.type;
        this.price = item.price;
    }


	public override bool Equals(object obj) {
		if (obj == null)
			return false;
		Item equalItem = obj as Item;
		if (equalItem == null)
			return false;
		return Equals(equalItem);
	}

	public bool Equals(Item i) {
		if (i == null)
			return false;
		return this.name.Equals(i.name);
	}

	public override int GetHashCode() {
		return this.name.GetHashCode();
	}

}

[Serializable]
public class Equipment : Item
{
	public int str { get; set; }
	public int def { get; set; }
    public int hp { get; set; }
    public Equipment(string name, string desc, int str, int def, int hp) : base(name, desc, "equipment", "???",  0, false)
	{
		this.str = str;
		this.def = def;
        this.hp = hp;
	}
}
*/