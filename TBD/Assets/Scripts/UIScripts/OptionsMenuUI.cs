using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;
using System;

public class OptionsMenuUI : MonoBehaviour {

	public Toggle fullscreenToggle;
	public Dropdown resolutionDropdown;
	public Dropdown textureQualityDropdown;
	public Slider musicVolumeSlider;
	public Slider sfxVolumeSlider;
	public Button confirmButton;
	public Button cancelButton;

	public Resolution[] resolutions;
	public List<string> resolutionList = new List<string>();
	private GameSettings gameSettings;

	public VolumeManager volMan;
	public MainMenuUI menu;
	public PauseMenuUI pause;

	private bool isActive = false;

	// Use this for initialization
	void Start () {
		menu = FindObjectOfType<MainMenuUI>();
		volMan = FindObjectOfType<VolumeManager>();
		pause = FindObjectOfType<PauseMenuUI>();
		musicVolumeSlider.value = volMan.getCurrentMusicVolumeLevel();
		sfxVolumeSlider.value = volMan.getCurrentSFXVolumeLevel();

		if (this.isActiveAndEnabled) {
			onEnable();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) && isActive) {
			OnCancelButtonClick();
		}
	}

	public void toggleMenu() {
		isActive = !isActive;
		this.gameObject.SetActive(isActive);
	}

	public void setActive(bool set) {
		this.isActive = set;
	}

	public bool getActive() {
		return this.isActive;
	}

	void onEnable() {
		gameSettings = this.gameObject.AddComponent<GameSettings>();

		fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
		resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
		textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
		musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
		sfxVolumeSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChange(); });
		confirmButton.onClick.AddListener(delegate { OnConfirmButtonClick(); });
		cancelButton.onClick.AddListener(delegate { OnCancelButtonClick(); });

		resolutions = Screen.resolutions;
		foreach(Resolution resolution in resolutions) {
			string name = resolution.width + " x " + resolution.height;
			if (!resolutionList.Contains(name)) {
				resolutionDropdown.options.Add(new Dropdown.OptionData(name));
				resolutionList.Add(name);
			}
			resolutionDropdown.RefreshShownValue();
		}
		LoadSettings();
	}

	public void OnFullscreenToggle() {
		gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
	}

	public void OnResolutionChange() {
		Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
		gameSettings.resolutionIndex = resolutionDropdown.value;
	}

	public void OnTextureQualityChange() {
		QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
	}

	public void OnMusicVolumeChange() {
		gameSettings.musicVolume = musicVolumeSlider.value;
		volMan.updateMusicVolume(musicVolumeSlider.value);
	}

	public void OnSFXVolumeChange() {
		gameSettings.sfxVolume = sfxVolumeSlider.value;
		volMan.updateSFXVolume(sfxVolumeSlider.value);
	}

	public void OnConfirmButtonClick() {
		if (menu != null) {
			menu.CloseOptions();
		}
		else if (pause != null) {
			pause.OptionsClose();
		}
		else {
			Debug.Log("Something went wrong");
		}
		SaveSettings();
	}

	public void OnCancelButtonClick() {
		if (menu != null) {
			menu.CloseOptions();
		}
		else if (pause != null) {
			pause.OptionsClose();
		}
		else {
			Debug.Log("Something went wrong");
		}
		LoadSettings();
	}

	public void SaveSettings() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/gamesettings.json");
		SettingData dat = WriteToData();
		bf.Serialize(file, dat);
		file.Close();
	}

	public void SaveCurrentSettings() {

	}

	SettingData WriteToData() {
		SettingData s = new SettingData();
		s.fullscreen = gameSettings.fullscreen;
		s.resolutionIndex = gameSettings.resolutionIndex;
		s.textureQuality = gameSettings.textureQuality;
		s.musicVolume = gameSettings.musicVolume;
		s.sfxVolume = gameSettings.sfxVolume;
		return s;
	}

	public void LoadSettings() {
		if (File.Exists(Application.persistentDataPath + "/gamesettings.json")) {
			try {
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(Application.persistentDataPath + "/gamesettings.json", FileMode.Open);
				SettingData s = (SettingData)bf.Deserialize(file);
				file.Close();
				WriteFromData(s);
			}
			catch {
				setDefault();
			}
		} else {
			setDefault();
		}
		fullscreenToggle.isOn = gameSettings.fullscreen;
		resolutionDropdown.value = gameSettings.resolutionIndex;
		textureQualityDropdown.value = gameSettings.textureQuality;
		musicVolumeSlider.value = gameSettings.musicVolume;
		sfxVolumeSlider.value = gameSettings.sfxVolume;
		Screen.fullScreen = gameSettings.fullscreen;
		resolutionDropdown.RefreshShownValue();
	}

	void WriteFromData(SettingData s) {
		gameSettings.fullscreen = s.fullscreen;
		gameSettings.resolutionIndex = s.resolutionIndex;
		gameSettings.textureQuality = s.textureQuality;
		gameSettings.musicVolume = s.musicVolume;
		gameSettings.sfxVolume = s.sfxVolume;
	}

	void setDefault() {
		gameSettings.fullscreen = false;
		gameSettings.resolutionIndex = 0;
		gameSettings.textureQuality = 0;
		gameSettings.musicVolume = 1.0f;
		gameSettings.sfxVolume = 1.0f;
	}

}

[Serializable]
class SettingData {
	//Any saved fields must be in here. currently just position:
	public bool fullscreen;
	public int resolutionIndex;
	public int textureQuality;
	public float musicVolume;
	public float sfxVolume;
}