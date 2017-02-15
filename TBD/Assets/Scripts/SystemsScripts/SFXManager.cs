using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {

	public AudioSource chopSFX;
	public AudioSource clothSFX;
	public AudioSource creakSFX;
	public AudioSource footstepSFX;
	public AudioSource coinsSFX;

	public List<GroundType> myGroundTypes = new List<GroundType>();
	public PlayerController player;
	public string currentground;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();
		if (myGroundTypes.Count > 0) {
			setGroundType(myGroundTypes[0]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
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

