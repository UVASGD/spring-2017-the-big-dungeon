using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance = null;
    
    private List<Enemy> enemies = new List<Enemy>();

    // Battle components that we dynamically load
    private BattleMenu battleMenu = null;
    private BattleInfo battleInfo = null;
	private MusicManager music;

	private SaveData tempSave;

	// State machine
	private bool inBattle = false;
    private Queue<BattleState> stateQueue = new Queue<BattleState>();
	private bool canBattle = false;
	private PlayerController player;

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

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
		music = FindObjectOfType<MusicManager>();
		player = FindObjectOfType<PlayerController> ();
	}

	public void setCanBattle(bool set) {
		this.canBattle = set;
	}

    public void loadEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    //Can't implement this yet
    public void removeEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
    }

    //Can't implement this yet
    public void removeAll()
    {
        enemies.Clear();
    }

    public List<Enemy> getList()
    {
        return this.enemies;
    }

    // State Machine stuff
    public void StartBattle()
    {
        this.inBattle = true;
        this.stateQueue.Enqueue(new PlayerState());
		if (player == null)
			player = FindObjectOfType<PlayerController> ();
		player.frozen = true;
		this.tempSave = FindObjectOfType<SaveController> ().WriteToData (false);

        SceneManager.LoadScene(2);
    }

    public void LoadBattleMenu(BattleMenu battleMenu)
    {
        this.battleMenu = battleMenu;

		this.battleMenu.enemySprite.GetComponent<Animator> ().SetInteger ("Enemy", enemies [0].sprite);
		Debug.Log (enemies [0].sprite);

		// Try to display the enemy sprite
		//this.battleMenu.enemySprite.sprite = Resources.Load<Sprite>(enemies[0].sprite);
		//Debug.Log ("Changed Sprite");
		//RuntimeAnimatorController rac = (RuntimeAnimatorController)Resources.Load("animations/enemies/grock-idle_0");
		//this.battleMenu.enemySprite.GetComponent<Animator> ().runtimeAnimatorController = Instantiate<RuntimeAnimatorController> (rac);
    }

    public void LoadBattleInfo(BattleInfo battleInfo)
    {
        this.battleInfo = battleInfo;
    }

    public void EndBattle()
    {
        this.stateQueue.Clear();
        Destroy(this.battleMenu);
        this.battleMenu = null;
        Destroy(this.battleInfo);
        this.battleInfo = null;
		SceneManager.LoadScene (1);
		music.SwitchTrack(0);
		this.inBattle = false;
		player.frozen = false;
		FindObjectOfType<SaveController> ().WriteFromData (this.tempSave);
    }

    public void addState(BattleState battleState)
    {
        stateQueue.Enqueue(battleState);
    }

	public void playerAttack() {
		this.addState (new TextState ("You swing a wild punch at the " + enemies[0].name));

		enemies [0].hp = Math.Max(enemies[0].hp - 3, 0);

		this.addState (new TextState ("The " + enemies[0].name + " has " + enemies[0].hp + " HP left."));

		if (enemies[0].hp <= 0) {
			// You won!
			this.addState (new TextState ("The " + enemies[0].name + " has fallen!"));
			this.addState(new TextState("{end}"));
			this.ProcessState ();
		}

		this.addState (new EnemyState ());
		this.ProcessState ();
	}

    public void ProcessState()
    {
        BattleState current = null;
        try
        {
            current = stateQueue.Dequeue();
        } catch {
            Debug.Log("Out of states, exiting battle");
            EndBattle();
            return;
        }
        
        if (current is PlayerState)
        {
            handlePlayerState(current as PlayerState);
        } else if (current is EnemyState)
        {
            handleEnemyState(current as EnemyState);
        } else if (current is TextState)
        {
            handleTextState(current as TextState);
        }
        else
        {
            Debug.Log("Invalid state, ending the battle");
            EndBattle();
            
        }
    }

    private void handlePlayerState(PlayerState state)
    {
        this.battleMenu.EnableMenu();
    }

    private void handleEnemyState(EnemyState state)
    {
		// For now, we just use the first enemy
		Enemy e = enemies[0];
		this.stateQueue.Enqueue(new TextState("The " + e.name + " strikes out viciously!"));

		player.setStatValue ("HP", player.getBaseStatValue ("HP") - 1);

		this.stateQueue.Enqueue(new TextState("You take 1 damage."));

		this.battleMenu.hpText.text = "" + player.getCurrentStatValue ("HP");

		// Check if player is dead
		if (player.getCurrentStatValue ("HP") == 0) {
			Debug.Log ("you have died!");	
		}

        this.stateQueue.Enqueue(new PlayerState());
        ProcessState();
    }

    private void handleTextState(TextState state)
    {
        List<string> dialogue = new List<string>();
        dialogue.Add(state.text);
        this.battleInfo.ShowDialogue(dialogue);
    }

}