using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.UI;

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
    private Queue<BattleState> stateQueue = new Queue<BattleState>();
	private PlayerController player;
	private bool waitForPlayer = false;
	private int currentKey;
	private List<string> currentKeyList = new List<string>();
	private List<string> possibleKeys = new List<string>(){"W", "A", "S", "D", "UpArrow", "DownArrow", "LeftArrow", "RightArrow"};
	private TimerUI timer;
	private bool isReady;
	private int successfulAttacks;
	private int failedAttacks;

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
		timer = FindObjectOfType<TimerUI> ();
	}

	void Update() {
		if (this.waitForPlayer) {
			if (Input.anyKey && isReady) {
				if (Input.GetKeyDown ((KeyCode)System.Enum.Parse (typeof(KeyCode), this.currentKeyList [this.currentKey]))) {
					this.currentKey++;
					this.successfulAttacks++;
					isReady = false;
					if (this.currentKey >= this.currentKeyList.Count) {
						//completed in time!
						attemptAttack (true);
					}
					parseKey ();
				}
				else if (!Input.GetKeyDown ((KeyCode)System.Enum.Parse (typeof(KeyCode), this.currentKeyList [this.currentKey]))) {
					this.currentKey++;
					this.failedAttacks++;
					isReady = false;
					if (this.currentKey >= this.currentKeyList.Count) {
						//completed in time!
						attemptAttack (true);
					}
					parseKey ();
				}
			}
			if (timer.isOutOfTime()) {
				// failed!!
				attemptAttack (false);
			}
			if (!isReady && !Input.anyKey) {
				isReady = true;
			}

		}
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
	public void StartBattle(int boss)
    {
        this.stateQueue.Enqueue(new PlayerState());
		if (player == null)
			player = FindObjectOfType<PlayerController> ();
		player.frozen = true;
		this.tempSave = FindObjectOfType<SaveController> ().WriteToData (false);
		if (boss == -1)
			music.SwitchTrack(4);
		else if (boss == 0) 
			music.SwitchTrack(9);
		else if (boss == 1) 
			music.SwitchTrack(0);
		else if (boss == 2) 
			music.SwitchTrack(2);
		else 
			music.SwitchTrack(4);
        SceneManager.LoadScene(2);
    }

    public void LoadBattleMenu(BattleMenu battleMenu)
    {
        this.battleMenu = battleMenu;

		this.battleMenu.enemySprite.GetComponent<Animator> ().SetInteger ("Enemy", enemies [0].sprite);

		Color c = this.battleMenu.keyBack.color;
		c.a = 0;
		this.battleMenu.keyBack.color = c;

		if (timer == null)
			timer = FindObjectOfType<TimerUI> ();
		
		timer.hideTimer ();


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

	public void EndBattle(bool isDead)
    {
		removeAll ();
        this.stateQueue.Clear();
        Destroy(this.battleMenu);
        this.battleMenu = null;
        Destroy(this.battleInfo);
        this.battleInfo = null;
		SceneManager.LoadScene (1);
		music.SwitchTrack(0);
		player.frozen = false;
		FindObjectOfType<ScreenFader> ().startLevel ();
		if (!isDead)
			FindObjectOfType<SaveController> ().WriteFromData (this.tempSave);
		if (isDead) {
			player.killPlayer ();
			//FindObjectOfType<SaveController> ().LoadFromSlot (FindObjectOfType<SaveController> ().getCurrentSlot ());
		}
			
    }

    public void addState(BattleState battleState)
    {
        stateQueue.Enqueue(battleState);
    }

	public void playerAttack() {
		this.addState (new PAttackState ());
		this.ProcessState ();
	}

	public void attemptAttack(bool success) {
		this.waitForPlayer = false;
		this.currentKey = 0;
		timer.resetTimer ();
		timer.hideTimer ();
		Color c = this.battleMenu.keyBack.color;
		c.a = 0;
		this.battleMenu.keyBack.color = c;
		if (success) {
			
			int damage = Math.Max (successfulAttacks - failedAttacks + (int) player.getCurrentStatValue ("str") - (int) enemies [0].defense, 0);
			if (failedAttacks > successfulAttacks) {
				damage = 0;
			}
			this.addState (new TextState ("You swing a wild punch at the " + enemies[0].name + " and deal " + damage + " damage!"));

			enemies [0].hp = Math.Max(enemies[0].hp - damage , 0);

			this.addState (new TextState ("The " + enemies[0].name + " has " + enemies[0].hp + " HP left."));

			if (enemies[0].hp <= 0) {
				// You won!
				this.addState (new TextState ("The " + enemies[0].name + " has fallen!"));
				this.addState(new TextState("{end}"));
				removeAll ();
				//this.ProcessState ();
			}
		} else {
			this.addState (new TextState ("You swing a wild punch at the " + enemies[0].name + " but miss!"));
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
            EndBattle(false);
            return;
        }
        
		if (current is PlayerState) {
			handlePlayerState (current as PlayerState);
		} else if (current is EnemyState) {
			handleEnemyState (current as EnemyState);
		} else if (current is TextState) {
			handleTextState (current as TextState);
		} else if (current is PAttackState) {
			handlePAttackState (current as PAttackState);
		}
        else
        {
            EndBattle(false);
            
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
		int damage = Math.Max ((int) enemies [0].strength - (int) player.getCurrentStatValue ("def"), 0);

		this.stateQueue.Enqueue(new TextState("The " + e.name + " strikes out viciously!"));

		player.setCurrentStatValue("HP", - damage);

		this.stateQueue.Enqueue(new TextState("You take " + damage + " damage."));

		this.battleMenu.hpText.text = "" + player.getCurrentStatValue ("HP");


		this.stateQueue.Enqueue(new PlayerState());

		// Check if player is dead
		if (player.getCurrentStatValue ("HP") <= 0) {
			EndBattle (true);
			//gameover.setActive ();
		}
        ProcessState();
    }

    private void handleTextState(TextState state)
    {
        List<string> dialogue = new List<string>();
        dialogue.Add(state.text);
        this.battleInfo.ShowDialogue(dialogue);
    }

	private void handlePAttackState(PAttackState state)
	{
		this.waitForPlayer = false;
		Enemy e = enemies [0];
		currentKeyList.Clear ();
		for (int i = 0; i < (int)e.defense; i++) {
			string s = possibleKeys[UnityEngine.Random.Range(0,possibleKeys.Count)];
			currentKeyList.Add (s);
		}
		this.currentKey = 0;
		this.successfulAttacks = 0;
		this.failedAttacks = 0;
		parseKey ();
		if (timer == null)
			timer = FindObjectOfType<TimerUI> ();
		timer.changeMaxTime (enemies [0].initiative);
		timer.startTimer ();
		timer.showTimer ();
		Color c = this.battleMenu.keyBack.color;
		c.a = 255;
		this.battleMenu.keyBack.color = c;
		this.waitForPlayer = true;
		//FUCK?
		isReady = false;
	}

	private void parseKey() {
		
		string s = this.currentKeyList [this.currentKey];
		if (s.Length == 1) {
			this.battleMenu.keyText.text = s;
		} else if (s == "UpArrow") {
			this.battleMenu.keyText.text = "⇧";
		} else if (s == "DownArrow") {
			this.battleMenu.keyText.text = "⇩";
		} else if (s == "LeftArrow") {
			this.battleMenu.keyText.text = "⇦";
		} else if (s == "RightArrow") {
			this.battleMenu.keyText.text = "⇨";
		}
		else
		{
			Debug.Log("Invalid key, ending the battle");
			EndBattle(false);

		}
	}


}