using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float m_speed = 0.1f;
    Camera mycam;

	public GameObject currentRoom;
	Rect currentBounds;

    public float minPosition = -10f;
    public float maxPosition = 10f;


	// Use this for initialization
	void Start () {
        mycam = GetComponent<Camera>();

		currentBounds = currentRoom.GetComponent<Tiled2Unity.TiledMap> ().GetMapRectInPixelsScaled ();
	}
	
	// Update is called once per frame
	void Update () {
        float scalefactor = 3f;
        mycam.orthographicSize = (Screen.height / 100f) / scalefactor;
		if ((target != null) && (currentRoom != null)) {
			Vector3 targetPosition = new Vector3 (0, 0, 0);
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
				targetPosition.x = target.transform.position.x;
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
			//currentBounds = null;
		}
	}
}
