using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SaveController : MonoBehaviour {

	public GameObject player;
    public InventoryManager inventory;
	private static bool saveExists;
	public bool isContinuing = false;

	// Use this for initialization
	void Start () {
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
		try {
			player = FindObjectOfType<PlayerController>().gameObject;
			inventory = FindObjectOfType<InventoryManager>();
			GameObject[] maps = GameObject.FindGameObjectsWithTag("map");
			for (int i = 0; i < maps.Length; ++i) {
				maps[i].AddComponent<MapSaver>();
			}
		}
		catch {}
		if (!saveExists) {
			saveExists = true;
			DontDestroyOnLoad(transform.gameObject);
		} else {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.O)) {
			SaveTo ("default");
		} else if (Input.GetKeyDown (KeyCode.P)) {
			LoadFrom ("default");
		}
	}

	void WriteFromData(SaveData s) {
		player.transform.position = new Vector2 (s.x, s.y);
        inventory.items = s.inventory;
	}

	SaveData WriteToData() {
		SaveData s = new SaveData ();
		s.x = player.transform.position.x;
		s.y = player.transform.position.y;
        s.inventory = inventory.items;
		return s;
	}

	public void SaveTo(String path) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + path + ".dat");
		SaveData dat = WriteToData ();
		bf.Serialize (file, dat);
		file.Close ();
	}

	public void LoadFrom(String path) {
		if (File.Exists (Application.persistentDataPath + path + ".dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + path + ".dat", FileMode.Open);
			SaveData s = (SaveData)bf.Deserialize (file);
			file.Close ();
			WriteFromData (s);
		}
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		Scene currentScene = SceneManager.GetSceneByName(scene.name);
		int buildIndex = currentScene.buildIndex;
		switch (buildIndex) {
		case 0:
			break;
		case 1:
			ScreenFader sf = GameObject.FindGameObjectWithTag("Fader").GetComponent<ScreenFader>();
			player = FindObjectOfType<PlayerController>().gameObject;
			inventory = FindObjectOfType<InventoryManager>();
			GameObject[] maps = GameObject.FindGameObjectsWithTag("map");
			for (int i = 0; i < maps.Length; ++i) {
				maps[i].AddComponent<MapSaver>();
			}
			if (isContinuing) {
				sf.BlackOut();
				StartCoroutine(sf.Wait(1.0f));
				LoadFrom("default");
			}
			break;
		default:
			break;
		}
	}
}

[Serializable]
class SaveData {
	//Any saved fields must be in here. currently just position:
	public float x;
	public float y;
    public List<Item> inventory;
}