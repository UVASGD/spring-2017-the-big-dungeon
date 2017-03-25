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
	// Use this for initialization
	void Start () {
        //Get Needed Objects
        sounds = new List<AudioSource>();
        player = FindObjectOfType<PlayerController>();
        sounds.AddRange(GetComponentsInChildren<AudioSource>());
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
        for (int i = 0; i < sounds.Count; i++)
        {
            sounds[i].Stop();
        }

    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = player.transform.position + new Vector3(xOffset, yOffset, 0.0f);
        if (Input.GetKeyDown(KeyCode.R))
        {
            isRaining = !isRaining;
            sounds[1].Stop();
            if (isRaining)
            {
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
            else
            {
                sounds[0].Stop();
                rainParticleSystem.Stop();
                rainParticleSystem.Clear();
            }
            
        } else if (Input.GetKeyDown(KeyCode.F))
        {
            sounds[1].Stop();
            isFogging = !isFogging;
            if (isFogging)
            {
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
            else
            {
                fogParticleSystem.Stop();
                fogParticleSystem.Clear();
            }
        } else if (Input.GetKeyDown(KeyCode.C))
        {
            isSnowing = !isSnowing;
            if (isSnowing)
            {
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
            else
            {
                sounds[1].Stop();
                snowParticleSystem.Stop();
                snowParticleSystem.Clear();
            }
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
}
