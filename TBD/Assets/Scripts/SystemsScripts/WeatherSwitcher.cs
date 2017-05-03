using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSwitcher : MonoBehaviour {

	private WeatherScript weather;
	public int type;


	// Use this for initialization
	void Start() {
		weather = FindObjectOfType<WeatherScript>();


	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name == "Player") {
			weather = FindObjectOfType<WeatherScript>();
			this.enabled = false;
			switch (type) {
				case 0:
					weather.toggleRain();
					break;
				case 1:
					weather.toggleFog();
					break;
				case 2:
					weather.toggleSnow();
					break;
				case 3:
					weather.allWeatherOff();
					break;
			}
				
		}
	}
}
