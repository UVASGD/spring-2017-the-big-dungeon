﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {

	public static PlayerController instance = null;

	private SFXManager sfxMan;
	private Animator anim;
	private Rigidbody2D rbody;
	private CameraManager cam;

	public bool frozen = false;
	public bool debugOn = false;

	public AudioSource[] playerStepSounds;
	
	// How often the step sound occurs
	private float stepInterval = 0.4f;
	private float timer = 0.0f;
	private AudioSource currentStep;

	private bool stepsOn;

	public bool inMenu = false;
	public bool talking = false;
    public bool alive = true;

	public List<BaseStat> stats = new List<BaseStat>();
	private PlayerStatsUI statsMenu;

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
	void Start () {

		DontDestroyOnLoad (gameObject);

        anim = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody2D>();
		sfxMan = FindObjectOfType<SFXManager>();
		cam = FindObjectOfType<CameraManager>();
		statsMenu = FindObjectOfType<PlayerStatsUI> ();
		BaseStat strength = new BaseStat ("strength", 10, "Damage Dealt");
		BaseStat defense = new BaseStat ("defense", 11, "Damage Taken");
		BaseStat HP = new BaseStat ("HP", 12, "Health");
		stats.Add(HP);
		stats.Add(strength);
		stats.Add(defense);
		changeCurrentStatValue ("HP", 5);
		changeCurrentStatValue ("strength", -2);
		changeCurrentStatValue ("defense", 0);
		statsMenu.addStat (HP);
		statsMenu.addStat (strength);
		statsMenu.addStat (defense);


		debug(getCurrentStatValue("HP") + "");
	}
	
	// Update is called once per frame
	void Update () {
		if (!frozen) {
			Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * 2.5f;

			if (movement_vector != Vector2.zero) {
				anim.SetBool("is_walking", true);
                anim.SetFloat("input_x", movement_vector.x);
				anim.SetFloat("input_y", movement_vector.y);
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

			rbody.MovePosition(rbody.position + (movement_vector * Time.deltaTime));
		}
		if (frozen) {
			anim.SetBool("is_walking", false);
			timer = 0;
        }
	}

	void PlayNextSound() {
		AudioSource lastStep = currentStep;
		while (currentStep == lastStep && playerStepSounds.Length > 1) {
			currentStep = playerStepSounds[UnityEngine.Random.Range(0, playerStepSounds.Length)];
		}
		if (lastStep != null)
            sfxMan.StopSFX(lastStep);
        if (currentStep != null)
            sfxMan.PlaySFX(currentStep);

    }


	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "map") {
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

	void OnTriggerExit2D(Collider2D other) {
		if (stepsOn) {
			debug("Steps are on from exiting something");
			sfxMan.GroundChange("default");
		}
	}

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

	void debug(string line) {
		if (debugOn) {
			Debug.Log(line);
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
			if (String.Compare(s.statName, statName) == 0) {
				return s.baseVal;
			}
		}
		return 0;
	}

	//positive value if adding. Negative value if taking away.
	public void changeCurrentStatValue(string statName, int modifier) {
		foreach (BaseStat s in stats) {
			if (String.Compare(s.statName, statName) == 0) {
				s.modifier += modifier;
			}
		}
	}

}
