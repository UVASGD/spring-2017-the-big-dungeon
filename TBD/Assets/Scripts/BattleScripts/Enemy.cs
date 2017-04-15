using System;
using UnityEngine;

[Serializable]
public class Enemy
{

    public string name { get; set; }
    public string description { get; set; }

    // Stats
    public Int64 hp { get; set; }
    public UInt64 strength { get; set; }
    public UInt64 defense { get; set; }
    public UInt64 initiative { get; set; }

	public int sprite { get; set; } //The integer representing which animation state has this sprite. A bit hackish, sorry

	public Enemy(string name, string description, Int64 hp, UInt64 strength, UInt64 defense, UInt64 initiative, int sprite)
    {
        this.name = name;
        this.description = description;
        this.hp = hp;
        this.strength = strength;
        this.defense = defense;
        this.initiative = initiative;
		this.sprite = sprite;
    }

}