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
	}

    void Update()
    {
        // This is just for testing
        if (Input.GetKeyUp(KeyCode.B) && !this.inBattle && this.canBattle)
        {
			music.SwitchTrack(4);
			// Make some dummy enemies and add them
			enemies.Add(new Enemy("Troll", "Really big and tough", 100, 75, 200, 4));
            enemies.Add(new Enemy("Troll #2", "Really big and tough", 100, 75, 200, 4));
            StartBattle();
        }
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
		this.tempSave = FindObjectOfType<SaveController> ().WriteToData (false);
        SceneManager.LoadScene("2_Battle_Scene");
    }

    public void LoadBattleMenu(BattleMenu battleMenu)
    {
        this.battleMenu = battleMenu;
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
        SceneManager.LoadScene("1_Main_Scene");
		music.SwitchTrack(0);
		this.inBattle = false;
		FindObjectOfType<SaveController> ().WriteFromData (this.tempSave);
    }

    public void addState(BattleState battleState)
    {
        stateQueue.Enqueue(battleState);
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
        this.stateQueue.Enqueue(new TextState("The enemies sit around passively."));
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