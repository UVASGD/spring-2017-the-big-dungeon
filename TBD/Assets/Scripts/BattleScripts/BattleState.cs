abstract public class BattleState
{
}

class PlayerState : BattleState
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
