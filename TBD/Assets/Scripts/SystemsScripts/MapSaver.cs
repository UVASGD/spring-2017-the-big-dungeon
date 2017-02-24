using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSaver : MonoBehaviour {

	private Tiled2Unity.TiledMap map;
	private new BoxCollider2D collider;
	private GameObject obj;
	private float height;
	private float width;

	// Use this for initialization
	void Start () {
		map = GetComponent<Tiled2Unity.TiledMap>();
		collider = this.gameObject.AddComponent<BoxCollider2D>();
		Rect r = map.GetMapRectInPixelsScaled();
		width = r.width;
		height = r.height;
		collider.isTrigger = true;
		collider.size = new Vector3(width,height,0);
		collider.offset = new Vector3(width / 2, - height / 2, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
