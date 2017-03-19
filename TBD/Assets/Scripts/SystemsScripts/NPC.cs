using System;
using UnityEngine;

[Serializable]
public class NPC : MonoBehaviour
{

    public string npcName;
    public Sprite npcSprite;

    public NPC(string name, Sprite sprite)
    {
        this.npcName = name;
        this.npcSprite = sprite;
    }

}