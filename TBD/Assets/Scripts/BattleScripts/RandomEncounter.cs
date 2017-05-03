using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEncounter : MonoBehaviour {

	// execute battleManager
	// Use this for initialization

	//private BoxCollider2D box;
	private bool playerWithin = false;
	public GameObject player;
	private Animator playerAnim;
	private float cooldownTimer;
	private float timerMax = 5f;
	private bool cooldown = false;
	private List<Enemy> enemyList = new List<Enemy>();
	public string encounterArea;
	private int encounterSec = 10;
	private BattleManager battleMan;

	void Start () {
		//this.box = GetComponent<BoxCollider2D> ();
		this.playerAnim = player.GetComponent<Animator> ();
		updateEncounterList ();
		this.battleMan = FindObjectOfType<BattleManager> ();
		startCooldown ();
	}
	
	// Update is called once per frame
	void Update () {
		this.player = FindObjectOfType<PlayerController> ().gameObject;
		this.playerAnim = player.GetComponent<Animator> ();
		if (this.playerWithin && this.playerAnim.GetBool("is_walking") && !this.cooldown) {
			checkEncounter ();
		}
		if (this.cooldown && this.playerAnim.GetBool("is_walking")) {
			this.cooldownTimer += Time.deltaTime;
			if (this.cooldownTimer > this.timerMax) {
				this.cooldown = false;
				this.cooldownTimer = 0f;
			}
		}
	}

	public void updateEncounterList() {
		if (encounterArea == "outskirts") {
			this.enemyList.Add (new Enemy ("Swift Sweepster", "Sweepster no swifting!", 20, 13, 4, 4, 0));
			this.enemyList.Add (new Enemy ("Tin Can", "This is one can that you shouldn't kick.", 25, 12, 7, 5, 2));
			this.enemyList.Add (new Enemy ("Grock", "Can you smell what the Grock is cooking?", 30, 11, 8, 6, 1));
			setEncounterSec (10);
		}
		if (encounterArea == "jungle") {
			this.enemyList.Add (new Enemy ("Swift Sweepster", "Sweepster no swifting!", 22, 15, 6, 6, 0));
			this.enemyList.Add (new Enemy ("Tin Can", "This is one can that you shouldn't kick.", 27, 14, 9, 7, 2));
			this.enemyList.Add (new Enemy ("Grock", "Can you smell what the Grock is cooking?", 32, 13, 10, 8, 1));
			setEncounterSec (10);
		}
		if (encounterArea == "water") {
			this.enemyList.Add (new Enemy ("Swift Sweepster", "Sweepster no swifting!", 23, 16, 7, 7, 0));
			this.enemyList.Add (new Enemy ("Tin Can", "This is one can that you shouldn't kick.", 28, 15, 10, 8, 2));
			this.enemyList.Add (new Enemy ("Grock", "Can you smell what the Grock is cooking?", 33, 14, 11, 9, 1));
			setEncounterSec (10);
		}
		if (encounterArea == "lava") {
			this.enemyList.Add (new Enemy ("Swift Sweepster", "Sweepster no swifting!", 30, 17, 10, 6, 0));
			this.enemyList.Add (new Enemy ("Tin Can", "This is one can that you shouldn't kick.", 35, 14, 8, 5, 2));
			this.enemyList.Add (new Enemy ("Grock", "Can you smell what the Grock is cooking?", 40, 15, 9, 5, 1));
			setEncounterSec (10);
		}
		// add more encounters
	}

	public void checkEncounter() {
		int ran = Random.Range(0, 33 * encounterSec);
		// 1 encounter per second == 1/33
		if (enemyList.Count > 0 && ran < 1) {
			int ran2 = Random.Range(0, enemyList.Count);
			// need to eventually call this from battle manager
			//FindObjectOfType<ScreenFader>().FadeToBlack();
			this.battleMan.loadEnemy(enemyList[ran2]);
			this.battleMan.StartBattle();
		}
	}

	public void catfishEncounter() {
		this.enemyList.Clear ();
		this.enemyList.Add(new Enemy ("Catfish", "Who dat is", 100, 12, 15, 10, 3));
		this.battleMan.loadEnemy(enemyList[0]);
		this.battleMan.StartBattle();
	}

	public void bossEncounter() {
		this.enemyList.Clear ();
		this.enemyList.Add (new Enemy ("THE MAD SCIENTIST", "Ah shit", 1000, 20, 10, 4, 4));
		this.battleMan.loadEnemy (enemyList [0]);
		this.battleMan.StartBattle ();
	}

	public void setEncounterSec(int num) {
		this.encounterSec = num;
	}

	public int getEncounterSec() {
		return this.encounterSec;
	}

	//should normally add way to encounter multiple enemies

	public void startCooldown() {
		// set cooldown once exited from battle
		this.cooldown = true;
	}

	public void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			this.playerWithin = true;
		}
	}

	public void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			this.playerWithin = false;
		}
	}
}
