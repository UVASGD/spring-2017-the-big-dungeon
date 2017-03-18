using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherRainScript : MonoBehaviour {
    public bool isActive;
	private ParticleSystem pSys;
	private ParticleSystem.EmissionModule myEmissionModule;
	public float myRate = 100f;

	// Use this for initialization
	void Start () {
        isActive = false;
        pSys = this.GetComponent<ParticleSystem>();
        pSys.Stop();
        pSys.Clear();
		myEmissionModule = pSys.emission;
		myEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(myRate);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R))
        {
            isActive = !isActive;
            if (isActive)
            {
                pSys.Play();
            }
            else
            {
                pSys.Stop();
                pSys.Clear();
            }
        }
	}

	public void updateRate(float newRate){
		myEmissionModule.rateOverTime = new ParticleSystem.MinMaxCurve(newRate);
	}

}
