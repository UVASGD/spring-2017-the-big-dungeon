using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;


namespace ItemOverhaul {
	
	[Serializable]
	public class Item {

		public enum ItemType {
			Consumable,
			Special,
			Hat,
			Body,
			Weapon
		}

		public int quantity;
		public int price;

		public string name;
		public string description;
		public List<string> effects;

		public ItemType type;
		public bool isUsable;

		public Item(string name, string description, List<string> effects, int quantity, int price, bool isUsable, ItemType type) {
			this.name = name;
			this.description = description;
			this.effects = effects;
			this.quantity = quantity;
			this.price = price;
			this.isUsable = isUsable;
			this.type = type;
		}

		public Item(Item i) {
			this.name = i.name;
			this.description = i.description;
			this.effects = i.effects;
			this.quantity = i.quantity;
			this.price = i.price;
			this.isUsable = i.isUsable;
			this.type = i.type;
		}
	}
}
