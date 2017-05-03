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


