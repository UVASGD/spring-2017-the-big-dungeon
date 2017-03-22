using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour {

	public Canvas quitMenu;
	public Button startText;
	public Button continueText;
	public Button optionsText;
	public Button creditsText;
	public Button exitText;
	public Canvas optionsMenu;
	public GameObject creditsBackground;
	private Canvas mainMenu;
	private float timer = 0f;
	private float maxTimer = 6.3f;
	private bool timerOn = false;
	private SaveController sc;
	private BattleManager bm;
	public Canvas startMenu;
	public GameObject saveArrow;
	public GameObject inputField;
	private int saveIndex = 1;
	private Vector2 startPosition1;
	private Vector2 startPosition2;
	private Vector2 startPosition3;

	private bool saveWait = false;
	private StartMenuUI[] saveFiles;


	// Use this for initialization
	void Start () {
		sc = FindObjectOfType<SaveController>();
		mainMenu = GetComponent<Canvas>();
		quitMenu = quitMenu.GetComponent<Canvas>();
		startText = startText.GetComponent<Button>();
		continueText = continueText.GetComponent<Button>();
		optionsText = optionsText.GetComponent<Button>();
		creditsText = creditsText.GetComponent<Button>();
		exitText = exitText.GetComponent<Button>();
		optionsMenu = optionsMenu.GetComponent<Canvas>();
		quitMenu.enabled = false;
		startMenu.enabled = false;
		optionsMenu.enabled = false;
		creditsBackground.SetActive(false);
		bm = FindObjectOfType<BattleManager>();
		bm.setCanBattle(false);

		startPosition3 = saveArrow.GetComponent<RectTransform>().anchoredPosition;
		startPosition2 = startPosition3 + new Vector2(0f, saveArrow.transform.parent.GetComponent<RectTransform>().rect.height * 1.09f);
		startPosition1 = startPosition2 + new Vector2(0f, saveArrow.transform.parent.GetComponent<RectTransform>().rect.height * 1.09f);

		saveFiles = startMenu.GetComponentsInChildren<StartMenuUI>();
		int i = 1;
		foreach (StartMenuUI s in saveFiles) {
			s.setThisSlot(i);
			s.updateSaveFiles();
			++i;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (timerOn) {
			timer += Time.deltaTime;
			if (timer > maxTimer) {
				timer = 0f;
				creditsBackground.SetActive(false);
				creditsBackground.GetComponentInChildren<CreditScrollUI>().resetPos();
				Color colorAlpha = new Color(0f, 0f, 0f, 255f);
				foreach (Text t in creditsBackground.GetComponentsInChildren<Text>()) {
					t.color += colorAlpha;
				}
			}
		}
		if (saveWait)
			saveWait = false;
	}

	public void StartPressed() {
		saveArrow.GetComponent<RectTransform>().anchoredPosition = startPosition1;
		mainMenuOff();
		startMenu.enabled = true;
		saveWait = true;
	}

	public void StartExitPressed() {
		startMenu.enabled = false;
		mainMenuOn();
	}

	public void deletePressed() {
		string name = EventSystem.current.currentSelectedGameObject.name;
		int num = int.Parse(name.Substring(name.Length - 1));
		Debug.Log(num);
		switch (num) {
			case 1:
				Debug.Log("Are you sure you want to delete save file 1?");
				break;
			case 2:
				Debug.Log("Are you sure you want to delete save file 2?");
				break;
			case 3:
				Debug.Log("Are you sure you want to delete save file 3?");
				break;
		}
	}

	public void buttonPressed() {
		if (startPosition2 == startPosition3) {
			startPosition2 = startPosition3 + new Vector2(0f, saveArrow.transform.parent.GetComponent<RectTransform>().rect.height * 1.09f);
		}
		if (startPosition1 == startPosition3) {
			startPosition1 = startPosition2 + new Vector2(0f, saveArrow.transform.parent.GetComponent<RectTransform>().rect.height * 1.09f);
			saveArrow.GetComponent<RectTransform>().anchoredPosition = startPosition1;
		}
		if (!saveWait) {
			int name = int.Parse(EventSystem.current.currentSelectedGameObject.name);
			switch (name) {
				case 1:
					saveArrow.GetComponent<RectTransform>().anchoredPosition = startPosition1;
					saveArrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
					saveIndex = 1;
					break;
				case 2:
					saveArrow.GetComponent<RectTransform>().anchoredPosition = startPosition2;
					saveArrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
					saveIndex = 2;
					break;
				case 3:
					saveArrow.GetComponent<RectTransform>().anchoredPosition = startPosition3;
					saveArrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
					saveIndex = 3;
					break;
			}
		}
	}

	public void ExitPressed() {
		quitMenu.enabled = true;
		mainMenuOff();
	}

	public void NoExitPressed() {
		quitMenu.enabled = false;
		mainMenuOn();
	}

	public void ContinueGame(){
		sc.setContinuing(true);
		bm.setCanBattle(true);
		SceneManager.LoadScene(1);
	}

	public void StartGame() {
		sc.setContinuing(false);
		bm.setCanBattle(true);
		string saveName = inputField.GetComponent<InputField>().text;
		sc.setCurrentSlot(saveIndex);
		sc.setCurrentName(saveName);
		SceneManager.LoadScene(1);
	}

	public void ExitGame() {
		Application.Quit();
	}

	public void OptionsPressed() {
		optionsMenu.enabled = true;
		mainMenu.enabled = false;
	}

	public void CloseOptions() {
		optionsMenu.enabled = false;
		mainMenu.enabled = true;
	}

	public void CreditsPressed() {
		timerOn = false;
		timer = 0.0f;
		creditsBackground.SetActive(true);
		mainMenuOff();
		creditsBackground.GetComponentInChildren<CreditScrollUI>().startCredits();
	}

	public void endOfCredits() {
		Color colorFade = new Color(0f, 0f, 0f, 0f);
		foreach (Text t in creditsBackground.GetComponentsInChildren<Text>()) {
			t.GetComponent<CanvasRenderer>().SetAlpha(1f);
			t.CrossFadeColor(colorFade, maxTimer, true, true);
			timerOn = true;
		}
		mainMenuOn();
	}

	public void mainMenuOn() {
		startText.enabled = true;
		continueText.enabled = true;
		optionsText.enabled = true;
		creditsText.enabled = true;
		exitText.enabled = true;
	}

	public void mainMenuOff() {
		startText.enabled = false;
		continueText.enabled = false;
		optionsText.enabled = false;
		creditsText.enabled = false;
		exitText.enabled = false;
	}
}
