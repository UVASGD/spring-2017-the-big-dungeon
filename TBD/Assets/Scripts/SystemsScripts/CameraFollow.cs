using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float m_speed = 0.1f;
    Camera mycam;

    public float minPosition = -10f;
    public float maxPosition = 10f;


	// Use this for initialization
	void Start () {
        mycam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        float scalefactor = 3f;
        mycam.orthographicSize = (Screen.height / 100f) / scalefactor;
        float halfWidth = Camera.main.orthographicSize * Screen.width / Screen.height;
        if (transform.position.x > 2.12 - halfWidth)
        {
            Debug.Log(transform.position);
        }
        
        if (target) {
            transform.position = Vector3.Lerp(transform.position, target.position, m_speed) + new Vector3(0, 0, -10);
        }

	}
}
