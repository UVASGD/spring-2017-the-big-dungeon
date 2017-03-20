using System;

[Serializable]
public class Enemy
{

    public string name { get; set; }
    public string description { get; set; }

    // Stats
    public UInt64 hp { get; set; }
    public UInt64 strength { get; set; }
    public UInt64 defense { get; set; }
    public UInt64 initiative { get; set; }

    public Enemy(string name, string description, UInt64 hp, UInt64 strength, UInt64 defense, UInt64 initiative)
    {
        this.name = name;
        this.description = description;
        this.hp = hp;
        this.strength = strength;
        this.defense = defense;
        this.initiative = initiative;
    }

}