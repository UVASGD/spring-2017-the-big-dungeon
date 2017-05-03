using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class NPCManager : MonoBehaviour
{
    public static NPCManager instance = null;
    private List<NPC> npcs = new List<NPC>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        // Grab all the NPC subelements
        npcs.AddRange(gameObject.GetComponentsInChildren<NPC>());
    }

    public NPC getNPC(string name)
    {
		return npcs.First(item => item.npcName == name);
    }

}