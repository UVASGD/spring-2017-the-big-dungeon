using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

abstract public class BattleState
{
}

class PlayerState : BattleState
{

}

class PAttackState : BattleState
{
	// attempt battle
	// wait to press a key
	public KeyCode currentKey;

}

class EnemyState : BattleState
{

}

class TextState : BattleState
{
    public string text;
    public TextState(string text)
    {
        this.text = text;
    }
}


