using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
using System.Collections.Generic;


[RequireComponent (typeof (MusicManager))]
[RequireComponent (typeof (InventoryManager))]
[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (VolumeManager))]
[RequireComponent (typeof (SaveController))]
public class SaveController : MonoBehaviour {

	private static bool saveExists;

	public GameObject player;
	public InventoryManager inventory;
	public bool isContinuing = false;
	private MusicManager music;
	private VolumeManager volumeMan;
	public bool debugOn = false;
	private BattleManager bm;
	private int currentSlot = 0;
	private string currentName = "";

	// Use this for initialization
	void Start () {
		music = FindObjectOfType<MusicManager> ();
		volumeMan = FindObjectOfType<VolumeManager> ();
		volumeMan.findVCObjects ();
		bm = FindObjectOfType<BattleManager>();
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
		if (player != null) {
			if (Input.GetKeyDown(KeyCode.O)) {
				SaveTo("default");
			}
			else if (Input.GetKeyDown(KeyCode.P)) {
				LoadFrom("default");
			}
			else if (Input.GetKeyDown(KeyCode.Alpha1)) {
				LoadFrom("slot1");
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2)) {
				LoadFrom("slot2");
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3)) {
				LoadFrom("slot3");
			}
		}
	}

	public void setCurrentSlot (int slot) {
		this.currentSlot = slot;
	}

	public int getCurrentSlot() {
		return this.currentSlot;
	}

	public void setCurrentName(string name) {
		this.currentName = name;
	}

	public string getCurrentName() {
		return this.currentName;
	}

	public void WriteFromData(SaveData s) {
		player = GameObject.FindGameObjectWithTag("Player");
		player.GetComponent<PlayerController>().updatePlayerName(s.playerName);
		player.transform.position = new Vector2 (s.x, s.y);
        inventory.items = s.inventory;
		inventory.money = s.money;
	}

	public SaveData WriteToData() {
		SaveData s = new SaveData();
		s.playerName = player.GetComponent<PlayerController>().getPlayerName();
		s.x = player.transform.position.x;
		s.y = player.transform.position.y;
        s.inventory = inventory.items;
        s.money = inventory.money;
		return s;
	}

	public void SaveToSlot(int slot) {
		string slotString = "slot" + currentSlot;
		SaveTo(slotString);
	}

	public void SaveTo(String path) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + path + ".dat");
		SaveData dat = WriteToData ();
		bf.Serialize (file, dat);
		file.Close ();
	}

	public void LoadFromSlot(int slot) {
		string slotString = "slot" + currentSlot;
		LoadFrom(slotString);
	}

	public void LoadFrom(String path) {
		if (File.Exists (Application.persistentDataPath + path + ".dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + path + ".dat", FileMode.Open);
			SaveData s = (SaveData)bf.Deserialize (file);
			file.Close ();
			WriteFromData (s);
			player = FindObjectOfType<PlayerController>().gameObject;
			player.GetComponent<Animator>().SetFloat("input_x", 0);
			player.GetComponent<Animator>().SetFloat("input_y", -1);
		}
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		if (music == null) {
			music = FindObjectOfType<MusicManager> ();
		}
		if (debugOn)
			Debug.Log (scene.name);
		Scene currentScene = SceneManager.GetSceneByName(scene.name);
		int buildIndex = currentScene.buildIndex;
		if (debugOn)
			Debug.Log ("Music from level load " + buildIndex);
		switch (buildIndex) {
		case 0:
			if (bm == null)
				bm = FindObjectOfType<BattleManager>();
			bm.setCanBattle(false);
			music.SwitchTrack (2);
			break;
		case 1:
			music.SwitchTrack (0);
			ScreenFader sf = GameObject.FindGameObjectWithTag("Fader").GetComponent<ScreenFader>();
			player = FindObjectOfType<PlayerController>().gameObject;
			inventory = FindObjectOfType<InventoryManager>();
			GameObject[] maps = GameObject.FindGameObjectsWithTag("map");
			for (int i = 0; i < maps.Length; ++i) {
				maps[i].AddComponent<MapSaver>();
			}
			inventory.addStartItems(isContinuing);
			if (isContinuing) {
				sf.BlackOut();
				StartCoroutine(sf.Wait(1.0f));
				LoadFrom("default");
				inventory.refreshItems();
			} else {
				//just starting
				SaveToSlot(currentSlot);
				player.GetComponent<PlayerController>().updatePlayerName(currentName);
			}
			break;
		case 2:
			music.SwitchTrack (4, 0.6f, 0.8f);
			break;
		default:
			music.SwitchTrack (0);
			break;
		}
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	public void changedLevel(int index){

	}

	public bool getContinuing() {
		return isContinuing;
	}

	
	public void setContinuing(bool set) {
		isContinuing = set;
	}

	public void rememberMusic(int requestedTrack, float requestedFadeOutSpeed = 0.4f, float requestedFadeInSpeed = 0.2f){
		if (music == null) {
			if (debugOn)
				Debug.Log ("Remembering music");
			music = FindObjectOfType<MusicManager> ();
			music.SwitchTrack(requestedTrack, requestedFadeOutSpeed, requestedFadeInSpeed);
		}
	}
}

[Serializable]
public class SaveData {
	//Any saved fields must be in here. currently just position:
	public string playerName;
	public float x;
	public float y;
    public List<Item> inventory;
    public int money;
}