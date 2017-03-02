using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rainScript : MonoBehaviour {
    public bool isActive;
    private ParticleSystem pSys;
	// Use this for initialization
	void Start () {
        isActive = false;
        pSys = this.GetComponent<ParticleSystem>();
        pSys.Stop();
        pSys.Clear();
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
}
