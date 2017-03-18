using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DontDestroyObject : MonoBehaviour {

	private static bool audioExists;


	// Use this for initialization
	void Start () {
		if (!audioExists) {
			audioExists = true;
			DontDestroyOnLoad(transform.gameObject);
		} else {
			Destroy(gameObject);
		}

	}
}
