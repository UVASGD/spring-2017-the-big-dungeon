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
	private string currentName;
	private bool newGame;

	public SaveData curData;
	private int currentLevel;



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
		curData = new SaveData();
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null) {
			/*
			if (Input.GetKeyDown(KeyCode.O)) {
				SaveTo("default", false);
			}
			else if (Input.GetKeyDown(KeyCode.P)) {
				LoadFrom("default");
			}
			else if (Input.GetKeyDown(KeyCode.Alpha1)) {
				SaveTo("slot1", false);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2)) {
				SaveTo("slot2", false);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3)) {
				SaveTo("slot3", false);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha8)) {
				LoadFrom("slot1");
			}
			else if (Input.GetKeyDown(KeyCode.Alpha9)) {
				LoadFrom("slot2");
			}
			else if (Input.GetKeyDown(KeyCode.Alpha0)) {
				LoadFrom("slot3");
			}*/
		}
	}

	public void setCurrentSlot (int slot) {
		this.currentSlot = slot;
	}

	public int getCurrentSlot() {
		return this.currentSlot;
	}

	public void setNewGame(bool b) {
		this.newGame = b;
	}

	public bool getNewGame() {
		return this.newGame;
	}

	public void setCurrentName(string name) {
		this.currentName = name;
		this.curData.playerName = name;
	}

	public string getCurrentName() {
		return this.currentName;
	}

	public void deleteSlot(int slot) {
		string slotString;
		slotString = "slot" + slot;
		if (slot == 0) {
			slotString = "default";
		}
		deleteSave(slotString);
		SaveTo(slotString, true);
	}

	public bool deleteSave(string saveName) {
		try {
			File.Delete(getSavePath(saveName));
		}
		catch (Exception) {
			return false;
		}
		return true;
	}

	private string getSavePath(string name) {
		return Path.Combine(Application.persistentDataPath, name + ".sav");
	}

	public bool doesSaveExist(string name) {
		return File.Exists(getSavePath(name));
	}

	public void WriteFromData(SaveData s) {
		player = GameObject.FindGameObjectWithTag("Player");
		curData = SaveData.deepCopy(s);
		setCurrentName(s.playerName);
		if (!s.newGame) {
			debug("This is not a new game");
			if (player != null) {
				player.GetComponent<PlayerController>().updatePlayerName(currentName);
				PlayerStatsUI ps = FindObjectOfType<PlayerStatsUI>();
				if (ps != null)
					ps.updateName(currentName);
				player.GetComponent<PlayerController> ().stats = s.playerStats;
				player.transform.position = new Vector2(s.x, s.y);
				inventory.items = s.inventory;
				inventory.money = s.money;
				inventory.setHat (curData.hat);
				inventory.setBody (curData.body);
				inventory.setWeapon (curData.weapon);
				player.GetComponent<PlayerController>().level = s.level;
				setNewGame(false);
			}
		}
	}

	public SaveData WriteToData(bool isNew) {
		SaveData s = new SaveData();
		if (!isNew) {
			s.playerName = currentName;
			s.playerStats = player.GetComponent<PlayerController>().stats;
			s.x = player.transform.position.x;
			s.y = player.transform.position.y;
			s.inventory = inventory.items;
			s.hat = inventory.getCurHat ();
			s.body = inventory.getCurBody ();
			s.weapon = inventory.getCurWeapon ();
			s.money = inventory.money;
			s.level = player.GetComponent<PlayerController>().level;
			//s.newGame = getNewGame();
			s.newGame = false;
		} else {
			s.playerName = "NEW SAVE FILE";
			s.x = 0;
			s.y = 0;
			s.inventory = null;
			s.money = 0;
			s.level = 0;
			s.newGame = true;
			s.playerStats = null;
		}
		return s;
	}
	
	public bool isNewGame(int slot) {
		string slotString;
		slotString = "slot" + slot;
		if (slot == 0) {
			slotString = "default";
		}
		LoadFrom(slotString);
		setNewGame(curData.newGame);
		return getNewGame();
	}

	public void SaveToSlot(int slot) {
		string slotString;
		slotString = "slot" + slot;
		if (slot == 0) {
			slotString = "default";
		}
		SaveTo(slotString, false);
	}

	public void SaveTo(string path, bool isNew) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (getSavePath(path));
		SaveData dat = WriteToData (isNew);
		bf.Serialize (file, dat);
		file.Close ();
	}

	public void LoadFromSlot(int slot) {
		string slotString;
		slotString = "slot" + slot;
		if (slot == 0) {
			slotString = "default";
		}
		LoadFrom(slotString);
		setCurrentSlot(slot);
	}

	public void LoadFrom(string path) {
		if (doesSaveExist(path)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (getSavePath(path), FileMode.Open);
			SaveData load = bf.Deserialize (file) as SaveData;
			file.Close ();
			WriteFromData (load);
			PlayerController pc = FindObjectOfType<PlayerController>();
			if (pc != null) {
				player = pc.gameObject;
				player.GetComponent<Animator>().SetFloat("input_x", 0);
				player.GetComponent<Animator>().SetFloat("input_y", -1);
			}
		} else {
			debug("doesn't exist, create new save");
			SaveTo(path, true);
			if (currentLevel != 1)
				// Don't load from new saves when in game
				LoadFrom(path);
		}
	}

	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		if (music == null) {
			music = FindObjectOfType<MusicManager> ();
		}
		debug(scene.name);
		Scene currentScene = SceneManager.GetSceneByName(scene.name);
		int buildIndex = currentScene.buildIndex;
		debug ("Music from level load " + buildIndex);
		switch (buildIndex) {
		case 0:
			if (bm == null)
				bm = FindObjectOfType<BattleManager>();
			bm.setCanBattle(false);
			music.SwitchTrack (0);
			currentLevel = 0;
			break;
		case 1:
			currentLevel = 1;
			music.SwitchTrack (6);
			ScreenFader sf = GameObject.FindGameObjectWithTag ("Fader").GetComponent<ScreenFader> ();
			player = FindObjectOfType<PlayerController> ().gameObject;
			inventory = FindObjectOfType<InventoryManager> ();
			GameObject[] maps = GameObject.FindGameObjectsWithTag ("map");
			for (int i = 0; i < maps.Length; ++i) {
				maps [i].AddComponent<MapSaver> ();
			}
			inventory.addStartItems (isContinuing);
			if (isContinuing) {
				sf.BlackOut ();
				StartCoroutine (sf.Wait (1.0f));
				LoadFromSlot (currentSlot);
				inventory.refreshItems ();
				//player.GetComponent<PlayerController> ().refreshStats ();
			} else {
				//just starting
				SaveToSlot (currentSlot);
				player.GetComponent<PlayerController> ().updatePlayerName (currentName);
				player.GetComponent<PlayerController> ().startStats ();
				setNewGame (false);
			}
			SaveToSlot (currentSlot);
			break;
		case 2:
			currentLevel = 2;
			music.SwitchTrack (4, 0.6f, 0.8f);
			break;
		default:
			currentLevel = 0;
			music.SwitchTrack (6);
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
			debug ("Remembering music");
			music = FindObjectOfType<MusicManager> ();
			music.SwitchTrack(requestedTrack, requestedFadeOutSpeed, requestedFadeInSpeed);
		}
	}

	void debug(string line) {
		if (debugOn) {
			Debug.Log(line);
		}
	}
}

[Serializable]
public class SaveData {
	//Any saved fields must be in here. currently just position:
	
	public float x;
	public float y;
    public List<Item> inventory;
	public Item hat, body, weapon;
    public int money;
	public int level;
	[SerializeField]
	public string playerName;
	public bool newGame;
	public List<BaseStat> playerStats;

	public static SaveData deepCopy(SaveData s) {
		SaveData temp = new SaveData();
		temp.playerName = s.playerName;
		temp.x = s.x;
		temp.y = s.y;
		temp.inventory = s.inventory;
		temp.hat = s.hat;
		temp.body = s.body;
		temp.weapon = s.weapon;
		temp.money = s.money;
		temp.level = s.level;
		temp.newGame = s.newGame;
		temp.playerStats = s.playerStats;
		return temp;
	}

	public void printData() {
		Debug.Log(this.playerName + " " +  this.x + " " + this.y + " " + this.money + " " + this.level);
	}

}