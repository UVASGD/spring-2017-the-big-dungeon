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
		playerAnim = player.GetComponent<Animator> ();
		updateEncounterList ();
		battleMan = FindObjectOfType<BattleManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		player = FindObjectOfType<PlayerController> ().gameObject;
		playerAnim = player.GetComponent<Animator> ();
		if (playerWithin && playerAnim.GetBool("is_walking") && !cooldown) {
			checkEncounter ();
		}
		if (cooldown && playerAnim.GetBool("is_walking")) {
			cooldownTimer += Time.deltaTime;
			if (cooldownTimer > timerMax) {
				Debug.Log ("cooldown finished");
				cooldown = false;
				cooldownTimer = 0f;
			}
		}
	}

	public void updateEncounterList() {
		if (encounterArea == "outskirts") {
			enemyList.Add (new Enemy ("Swift Sweepster", "Sweepster no swifting!", 4, 5, 4, 3, 0));
			enemyList.Add (new Enemy ("Tin Can", "This is one can that you shouldn't kick.", 5, 4, 7, 2, 2));
			enemyList.Add (new Enemy ("Grock", "Can you smell what the Grock is cooking?", 8, 2, 8, 1, 1));
			setEncounterSec (10);
		}
		// add more encounters
	}

	public void checkEncounter() {
		int ran = Random.Range(0, 33 * encounterSec);
		// 1 encounter per second == 1/33
		if (ran < 1) {
			int ran2 = Random.Range(0, enemyList.Count);
			Debug.Log ("You've encountered " + enemyList [ran2].name);
			// need to eventually call this from battle manager
			//FindObjectOfType<ScreenFader>().FadeToBlack();
			battleMan.loadEnemy(enemyList[ran2]);
			battleMan.StartBattle();
			startCooldown ();
		}
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
		cooldown = true;
	}

	public void OnTriggerEnter2D(Collider2D other) {
		Debug.Log ("entered");
		playerWithin = true;
	}

	public void OnTriggerExit2D(Collider2D other) {
		Debug.Log ("exited");
		playerWithin = false;
	}
}
