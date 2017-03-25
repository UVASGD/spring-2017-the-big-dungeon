using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;


	
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

		public Item(Item i, int quant) {
			this.name = i.name;
			this.description = i.description;
			this.effects = i.effects;
			this.quantity = quant;
			this.price = i.price;
			this.isUsable = i.isUsable;
			this.type = i.type;
		}

		public static bool isEquipment(Item i) { //utility method
			return (i.type == ItemType.Hat || i.type == ItemType.Body || i.type == ItemType.Weapon);
		}

		public bool useItem() { //Use a usable item
			if (!isUsable || quantity < 1) { //if not usable, or if out of item, dont use.
				return false;
			}
			apply (); //otherwise apply effect
			quantity--; //consume one item
			return true; //return true
		}

		public void apply() { //separated so that equipment/non-usable buffs can be applied as well
		foreach (BaseStat s in GameObject.FindObjectOfType<PlayerController> ().stats) {  //for each stat, check each effect against it
				foreach (String eff in effects) {
					string[] bits = eff.Split (new char[]{ ':',',' }); //split effect string into stat name && effect (if properly formatted ....)
					if (bits [0].ToLower ().Equals (s.statName.ToLower ())) { //if stat names match, apply
						try {
							s.modifier += Int32.Parse (bits [1]); //catch bad formatting errors, such as failing to split string up accordingly and using poorly formatted numbers
						} catch {
							//Do nothing
						}
					}
				}
			}
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
	}
