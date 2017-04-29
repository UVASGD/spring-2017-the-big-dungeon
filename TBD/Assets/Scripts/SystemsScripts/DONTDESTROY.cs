using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DONTDESTROY : MonoBehaviour {

	static GameObject instance;

	void awake() {

	}

	// Use this for initialization
	void Start () {
		if (instance == null) {
			DontDestroyOnLoad (gameObject);
			instance = this.gameObject;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
