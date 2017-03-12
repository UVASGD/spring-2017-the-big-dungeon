using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

	public List<BaseStat> stats = new List<BaseStat>();
	// Use this for initialization
	void Start () {
		stats.Add(new BaseStat("strength", 10, "Damage Dealt"));
		stats.Add(new BaseStat("defense", 11, "Damage Taken"));
		stats.Add(new BaseStat("HP", 12, "Health"));
    }
    //base stat + modifier
    public int getCurrentValue(string statName)
	{
		foreach (BaseStat s in stats) {
            if (String.Compare(s.statName, statName) == 0) 
			{
				return s.finalValue();
			}
		}
        return 0;
    }
    //positive value if adding. Negative value if taking away.
	public void modifyStat(string statName, int modifier)
    {
		foreach (BaseStat s in stats)
        {
			if (String.Compare(s.statName, statName) == 0)
            {
				s.modifier += modifier;
            }
        }
    }
}
