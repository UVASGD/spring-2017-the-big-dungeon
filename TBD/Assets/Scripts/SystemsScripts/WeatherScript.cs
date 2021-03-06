﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherScript : MonoBehaviour {
	private GameObject rainController, snowController, fogController;
	private ParticleSystem rainParticleSystem, snowParticleSystem, fogParticleSystem;
	private ParticleSystem.EmissionModule rainEmissionModule, snowEmissionModule, fogEmissionModule;
	private PlayerController player;
	//Player's Offset From Weather
	float yOffset = 3.0f, xOffset = 0.0f;
	public float myRate = 100f;
	private bool isRaining, isSnowing, isFogging;
	private List<AudioSource> sounds;
	private List<SpriteRenderer> sprites;
	private List<MeshRenderer> renderers;
	public float brightness = 0.5f;


	public static WeatherScript instance = null;

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
	void Start() {

		//Allows us to persist accross scenes
		DontDestroyOnLoad (gameObject);

		//Get Needed Objects
		sounds = new List<AudioSource>();
		player = FindObjectOfType<PlayerController>();
		sounds.AddRange(GetComponentsInChildren<AudioSource>());
		sprites = new List<SpriteRenderer>();
		sprites.AddRange(FindObjectsOfType<SpriteRenderer>());
		renderers = new List<MeshRenderer>();
		renderers.AddRange(FindObjectsOfType<MeshRenderer>());
		rainController = transform.GetChild(0).gameObject;
		fogController = transform.GetChild(1).gameObject;
		snowController = transform.GetChild(2).gameObject;

		//Get Particle Systems of controllers
		rainParticleSystem = rainController.GetComponent<ParticleSystem>();
		fogParticleSystem = fogController.GetComponent<ParticleSystem>();
		snowParticleSystem = snowController.GetComponent<ParticleSystem>();

		//Get Emission Modules of Particle Systems
		rainEmissionModule = rainParticleSystem.emission;
		fogEmissionModule = fogParticleSystem.emission;
		snowEmissionModule = snowParticleSystem.emission;

		//Update the Emmision Rates
		updateRainRate(myRate);
		updateSnowRate(myRate);
		updateFogRate(myRate);

		//Make sure no weather is active on start
		isRaining = false;
		isSnowing = false;
		isFogging = false;

		rainParticleSystem.Stop();
		snowParticleSystem.Stop();
		fogParticleSystem.Stop();
		rainParticleSystem.Clear();
		snowParticleSystem.Clear();
		fogParticleSystem.Clear();
		//Make sure no sounds are playing
		for (int i = 0; i < sounds.Count; i++) {
			sounds[i].Stop();
		}

		updateBrightness(brightness);

	}

	// Update is called once per frame
	void Update() {
		if (player == null)
			player = FindObjectOfType<PlayerController>();
		this.transform.position = player.transform.position + new Vector3(xOffset, yOffset, 0.0f);
		/*
		if (Input.GetKeyDown(KeyCode.R)) {
			toggleRain();
		} else if (Input.GetKeyDown(KeyCode.F)) {
			toggleFog();
		} else if (Input.GetKeyDown(KeyCode.C)) {
			toggleSnow();
		}*/
	}

	public void allWeatherOff() {
		sounds[1].Stop();
		sounds[0].Stop();
		isRaining = false;
		rainParticleSystem.Stop();
		rainParticleSystem.Clear();
		isFogging = false;
		fogParticleSystem.Stop();
		fogParticleSystem.Clear();
		isSnowing = false;
		snowParticleSystem.Stop();
		snowParticleSystem.Clear();
	}

	public void toggleRain() {
		isRaining = !isRaining;
		sounds[1].Stop();
		if (isRaining) {
			sounds[0].Play();
			//Stop other weather effects
			isFogging = false;
			isSnowing = false;
			snowParticleSystem.Stop();
			fogParticleSystem.Stop();
			snowParticleSystem.Clear();
			fogParticleSystem.Clear();

			rainParticleSystem.Play();
		}
		else {
			sounds[0].Stop();
			rainParticleSystem.Stop();
			rainParticleSystem.Clear();
		}
	}

	public void toggleFog() {
		sounds[1].Stop();
		isFogging = !isFogging;
		if (isFogging) {
			sounds[0].Stop();
			//Stop other weather effects
			isRaining = false;
			isSnowing = false;
			rainParticleSystem.Stop();
			snowParticleSystem.Stop();
			rainParticleSystem.Clear();
			snowParticleSystem.Clear();

			fogParticleSystem.Play();
		}
		else {
			fogParticleSystem.Stop();
			fogParticleSystem.Clear();
		}
	}

	public void toggleSnow() {
		isSnowing = !isSnowing;
		if (isSnowing) {
			sounds[1].Play();
			sounds[0].Stop();
			//Stop other weather effects
			isFogging = false;
			isRaining = false;
			rainParticleSystem.Stop();
			fogParticleSystem.Stop();
			rainParticleSystem.Clear();
			fogParticleSystem.Clear();

			snowParticleSystem.Play();
		}
		else {
			sounds[1].Stop();
			snowParticleSystem.Stop();
			snowParticleSystem.Clear();
		}
	}

    public void updateRainRate(float newRate)
    {
        rainEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(newRate);
    }
    public void updateSnowRate(float newRate)
    {
        snowEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(newRate);
    }
    public void updateFogRate(float newRate)
    {
        fogEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(newRate);
    }
    public void updateBrightness(float newNum)
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            sprites[i].color = new Color(Math.Min(sprites[i].color.r * newNum, 1.0f), Math.Min(sprites[i].color.g * newNum, 1.0f), Math.Min(sprites[i].color.b * newNum, 1.0f));
        }
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].material.color = new Color(Math.Min(renderers[i].material.color.r * newNum, 1.0f), Math.Min(renderers[i].material.color.g * newNum, 1.0f), Math.Min(renderers[i].material.color.b * newNum, 1.0f));
        }
    } 
}
