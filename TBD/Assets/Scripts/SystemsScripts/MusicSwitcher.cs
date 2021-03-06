﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwitcher : MonoBehaviour {

	private MusicManager mc;
	public int newTrack;
	public bool switchOnStart;

	// Use this for initialization
	void Start () {
		mc = FindObjectOfType<MusicManager> ();
		if (switchOnStart) {
			mc = FindObjectOfType<MusicManager>();
			mc.SwitchTrack (newTrack);
			gameObject.SetActive (false);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name == "Player") {
			mc = FindObjectOfType<MusicManager>();
			mc.SwitchTrack(newTrack);
			this.enabled = false;
		}
	}
}
