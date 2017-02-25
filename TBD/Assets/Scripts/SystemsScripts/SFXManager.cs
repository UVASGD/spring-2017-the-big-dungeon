using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {

	public PlayerController player;

	public List<AudioSource> soundEffects = new List<AudioSource>();
	public List<GroundType> myGroundTypes = new List<GroundType>();
	public string currentground;

	// Use this for initialization
	void Start () {
		soundEffects.AddRange(GetComponentsInChildren<AudioSource>());
		player = FindObjectOfType<PlayerController>();
		if (myGroundTypes.Count > 0 && player != null) {
			setGroundType(myGroundTypes[0]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = FindObjectOfType<PlayerController>();
		}
		if (myGroundTypes.Count > 0 && player != null) {
			setGroundType(myGroundTypes[0]);
		}
	}

    public void StopSFX(AudioSource sfx)
    {
        sfx.Stop();
    }

    public void PlaySFX(AudioSource sfx)
    {
        sfx.Play();
    }

	public void GroundChange(string change) {
		if (change == "path") {
			setGroundType(myGroundTypes[1]);
		}
		else if (change == "grass") {
			setGroundType(myGroundTypes[2]);
		}
		else
			setGroundType(myGroundTypes[0]);
	}

	public void setGroundType(GroundType ground) {
		if (currentground != ground.name) {
			player.UpdateGround(ground.stepSounds);
			currentground = ground.name;
		}
	}
}

[System.Serializable]
public class GroundType {
	public string name;
	public AudioSource[] stepSounds;
}

