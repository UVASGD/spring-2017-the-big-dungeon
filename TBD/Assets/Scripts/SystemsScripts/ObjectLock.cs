using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLock : MonoBehaviour {

	public GameObject target;

	bool locked;

	public static ObjectLock instance = null;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);

		locked = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(!locked && ((Vector2)gameObject.transform.position - (Vector2)target.transform.position).magnitude < 0.05) {
			Debug.Log ("FREEZE");
			locked = true;
			gameObject.transform.position = target.transform.position;
			gameObject.GetComponent<Rigidbody2D> ().constraints = RigidbodyConstraints2D.FreezeAll;
		}
	}

}
