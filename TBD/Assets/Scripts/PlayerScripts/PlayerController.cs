using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {

	//To keep track of our instance between scenes
	public static PlayerController instance = null;

	//References for various things we need to interact with
	private SFXManager sfxMan;
	private Animator anim;
	private Rigidbody2D rbody;
	private CameraManager cam;

	//true if player is locked into place; false if not.
	public bool frozen = false;
	//Enable debug.logs
	public bool debugOn = false;

	//Sounds for walking
	public AudioSource[] playerStepSounds;

	//Arbitrary speeds
    private float currentSpeed = 2.5f;
    private float normalSpeed = 2.5f;
    private float runSpeed = 4.0f;
    private float slowSpeed = 0.5f;

    // How often the step sound occurs
    private float stepInterval = 0.4f;
	//Timer for steps
	private float timer = 0.0f;
	private AudioSource currentStep;

	private bool stepsOn;

	public bool inMenu = false;
	public bool talking = false;
    public bool alive = true;

	//Current info
	public List<BaseStat> stats = new List<BaseStat>();
    public int level = 1;
    public int currentExp = 0;

	//References for menus
	private PlayerStatsUI statsMenu;
    public GameObject gameoverMenu;

    private string playerName;

	//Used to keep only 1 instance of PlayerController around; is called when a scene loads
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

	// Called when object is created
	void Start () {
		//Changes quality setting so as to smooth movement
		QualitySettings.vSyncCount = 0;

		//Allows us to persist accross scenes
		DontDestroyOnLoad (gameObject);

		//Grabs some references
        anim = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody2D>();
		sfxMan = FindObjectOfType<SFXManager>();
		cam = FindObjectOfType<CameraManager>();
		statsMenu = FindObjectOfType<PlayerStatsUI> ();
        debug(getCurrentStatValue("HP") + "");

		//Initializes stats and stats UI
		startStats ();
    }

	//Initializes player's stats, and also adds these stats to the proper UI
	public void startStats() {
		statsMenu = FindObjectOfType<PlayerStatsUI> ();
		BaseStat strength = new BaseStat ("str", 10, "Damage Dealt", -2); //Current 3 stats-- should be not hardcoded?? consider changing.
		BaseStat defense = new BaseStat ("def", 11, "Damage Taken", 0);
		BaseStat HP = new BaseStat ("HP", 12, "Health", 5);

		//Adds to our stats
		stats.Add(HP);
		stats.Add(strength);
		stats.Add(defense);

		//Adds to UI
		if (statsMenu != null) {
			statsMenu.addStat(HP);
			statsMenu.addStat(strength);
			statsMenu.addStat(defense);
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Only move if not frozen
		if (!frozen) {
			//Get direction
			Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * currentSpeed;

        if (movement_vector != Vector2.zero) {
                //Running
                if (Input.GetKey(KeyCode.LeftShift)) {
                    currentSpeed = runSpeed;
                    stepInterval = 0.3f;
					if (currentStep != null)
                    	currentStep.volume = 0.6f;
                    anim.speed = 2.0f;
                }
                //Walk Slower
                else if (Input.GetKey(KeyCode.RightShift)) {
                    currentSpeed = slowSpeed;
                    stepInterval = 0.48f;
					if (currentStep != null)
                    	currentStep.volume = 0.1f;
                    anim.speed = 0.5f;
                }
                
                //Walking
                else {
                    currentSpeed = normalSpeed;
                    stepInterval = 0.4f;
					if (currentStep != null)
                    	currentStep.volume = 0.2f;
                    anim.speed = 1f;
                }

				//Since we're moving, set appropriate variables in animator
                anim.SetBool("is_walking", true);
                anim.SetFloat("input_x", movement_vector.x);
				anim.SetFloat("input_y", movement_vector.y);

				//Timer for steps
				if (stepsOn)
					timer += Time.deltaTime;

				if (timer > stepInterval) {
					timer = 0;
					PlayNextSound();
				}
			}

			else {
				anim.SetBool("is_walking", false);
				timer = 0;
			}

			//Set our velocity, so we move.
			rbody.velocity = movement_vector;
		}
		if (frozen) {
			anim.SetBool("is_walking", false);
			rbody.velocity = new Vector2 (0f, 0f);
			timer = 0;
        }
        //Kill Yourself Instantly. Game Over Testing
		/*
        if (Input.GetKeyDown(KeyCode.M) && alive)
        {
            foreach (BaseStat s in stats)
            {
                if (String.Compare(s.statName, "HP") == 0)
                {
                    s.modifier -= 12;
                }
            }
            Debug.Log(getCurrentStatValue("HP") + "");
        }*/
		
		//If necessary, Die
        if (getCurrentStatValue("HP") <= 0 && alive)
        {
            Debug.Log("Die Please!");
            alive = false;
            gameoverMenu.SetActive(true);
        }
    }

	//Plays current step sound
	void PlayNextSound() {
		AudioSource lastStep = currentStep;

		//Gets another random sound
		while (currentStep == lastStep && playerStepSounds.Length > 1) {
			currentStep = playerStepSounds[UnityEngine.Random.Range(0, playerStepSounds.Length)];
		}

		//stops last sound
		if (lastStep != null)
            sfxMan.StopSFX(lastStep);
		//plays new sound
        if (currentStep != null)
            sfxMan.PlaySFX(currentStep);

    }

	//Getter / Setter for player name
	public void updatePlayerName(string name) {
		this.playerName = name;
	}

	public string getPlayerName() {
		return this.playerName;
	}

	//Checks for entering certain areas
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "map") {
			cam = FindObjectOfType<CameraManager> ();
			cam.setCurrentRoom(other.gameObject);
		}
        if (stepsOn) {
			debug("Steps are on from entering something");
			if (other.transform.tag == "path") {
				sfxMan.GroundChange("path");
			}
			else if (other.transform.tag == "grass") {
				sfxMan.GroundChange("grass");
			}
			else
				sfxMan.GroundChange("default");
		}
	}

	//Checks for exiting certain areas
	void OnTriggerExit2D(Collider2D other) {
		if (stepsOn) {
			debug("Steps are on from exiting something");
			sfxMan.GroundChange("default");
		}
	}

	//Update walking noises to fit the area we are walking on
	public void UpdateGround(AudioSource[] stepSounds) {
		debug("Updating the current ground!");
		if (playerStepSounds != null) {
			debug("Setting the sounds now");
			playerStepSounds = stepSounds;
			stepsOn = true;
			if (currentStep != null)
				currentStep.Stop();
			currentStep = playerStepSounds[UnityEngine.Random.Range(0, playerStepSounds.Length)];
		}
	}

	//For debuging purposes
	void debug(string line) {
		if (debugOn) {
			Debug.Log(line);
		}
	}

	//Getter / Setter for stats
	public void setStatValue(string statName, int newValue) {
		foreach (BaseStat s in stats) {
			if (String.Compare(s.statName, statName) == 0) {
				s.baseVal = newValue;
				break;
			}
		}
	}

	//base stat + modifier
	public int getCurrentStatValue(string statName) {
		foreach (BaseStat s in stats) {
			if (String.Compare(s.statName, statName) == 0) {
				return s.currentValue();
			}
		}
		return 0;
	}

	//base stat
	public int getBaseStatValue(string statName) {
		foreach (BaseStat s in stats) {
			if (String.Compare (s.statName, statName) == 0) {
				return s.baseVal;
			}
		}
		return 0;
	}

    //currently only need 1000 XP to get to the next lvl
    public int getNewLevel()
    {
        Double calculatedLvl = currentExp / 1000;
        return (int) Math.Floor(calculatedLvl) + 1;
    }

    //returns true if leveled up
    public bool addExp(int exp)
    {
        currentExp += exp;
        int newlvl = getNewLevel();
        if(newlvl > level)
        {
            levelUp(newlvl - level);
            level = newlvl;
            return true;
        }

        return false;
    }

    //adds 2 to every base stat for each level up
    public void levelUp(int numoflevels)
    {
        foreach (BaseStat s in stats) {
            s.modifier += 2 * numoflevels;
        }

        level += numoflevels;
    }
}
