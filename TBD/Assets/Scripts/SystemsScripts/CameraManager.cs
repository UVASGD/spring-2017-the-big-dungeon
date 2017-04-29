using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour {

    public Transform target;
    public float m_speed = 0.1f;
    Camera mycam;

	public GameObject currentRoom;
	Rect currentBounds;

    public float minPosition = -10f;
    public float maxPosition = 10f;

	public bool freeze { get; set; }

	public float scalefactor;


	public GameObject outskirts;
	public GameObject town;

	// Use this for initialization
	void Start () {
		this.freeze = false;
        mycam = GetComponent<Camera>();
		target = FindObjectOfType<PlayerController> ().transform;
	}
	
	// Update is called once per frame
	void Update () {
		mycam.orthographicSize = (Screen.height / 100f) / scalefactor;
		if(target == null) {
			target = FindObjectOfType<PlayerController> ().transform;
		}
		if(target != null && !freeze) {
			Vector3 targetPosition = new Vector3 (0, 0, 0);
			if (currentRoom) {
				currentBounds = currentRoom.GetComponent<Tiled2Unity.TiledMap> ().GetMapRectInPixelsScaled ();
				if (currentBounds.height < mycam.orthographicSize * 2) {
					targetPosition.y = (currentBounds.yMin + currentBounds.yMax) / 2;
				} else if (target.transform.position.y < currentBounds.yMin + mycam.orthographicSize) {
					targetPosition.y = currentBounds.yMin + mycam.orthographicSize;
				} else if (target.transform.position.y > currentBounds.yMax - mycam.orthographicSize) {
					targetPosition.y = currentBounds.yMax - mycam.orthographicSize;
				} else {
					targetPosition.y = target.transform.position.y;
				}
				if (currentBounds.width < mycam.orthographicSize * 2 * mycam.aspect) {
					targetPosition.x = (currentBounds.xMin + currentBounds.xMax) / 2;
				} else if (target.transform.position.x < currentBounds.xMin + (mycam.orthographicSize * mycam.aspect)) {
					targetPosition.x = currentBounds.xMin + (mycam.orthographicSize * mycam.aspect);
				} else if (target.transform.position.x > currentBounds.xMax - (mycam.orthographicSize * mycam.aspect)) {
					targetPosition.x = currentBounds.xMax - (mycam.orthographicSize * mycam.aspect);
				} else {
					targetPosition.x = target.position.x;
				}
			} else {
				targetPosition.x = target.position.x;
				targetPosition.y = target.position.y;
			}
			transform.position = Vector3.Lerp (transform.position, targetPosition, m_speed) + new Vector3 (0, 0, -10);
		}

	}

	public void setCurrentRoom(GameObject newRoom) {
		if (newRoom) {
			currentRoom = newRoom;
			currentBounds = currentRoom.GetComponent<Tiled2Unity.TiledMap> ().GetMapRectInPixelsScaled ();
		} else {
			currentRoom = null;
		}
	}

	public void instantMove() {
		mycam = GetComponent<Camera>();
		target = FindObjectOfType<PlayerController> ().transform;
		transform.position = target.position;
	}
}
