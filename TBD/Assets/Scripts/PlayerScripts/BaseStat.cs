using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStat {

    public int baseVal { get; set; }
    public string statName { get; set; }
    public string description { get; set; }
    public int modifier { get; set; }
    

    public BaseStat(string statName, int baseVal, string description)
    {
        this.statName = statName;
        this.baseVal = baseVal;
        this.description = description;
        this.modifier = 0;
    }

	public BaseStat(string statName, int baseVal, string description, int modifier)
	{
		this.statName = statName;
		this.baseVal = baseVal;
		this.description = description;
		this.modifier = modifier;
	}

    public int currentValue()
    {
        return baseVal + modifier;
    }
}
