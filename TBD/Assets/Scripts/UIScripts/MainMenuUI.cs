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
	public StartMenuUI[] saveFiles;
	public GameObject deleteMenu;
	public GameObject warningMenu;

	private int curDelete;

	public GameObject continueMenu;
	public GameObject contArrow;
	private int contIndex = 1;
	private Vector2 contPosition1;
	private Vector2 contPosition2;
	private Vector2 contPosition3;
	private bool contWait = false;
	public ContinueMenuUI[] contFiles;
	public GameObject warningMenu2;

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
		saveArrow.GetComponent<RectTransform>().anchoredPosition = startPosition1;
		refreshSaveFilesUI();		

		contPosition3 = contArrow.GetComponent<RectTransform>().anchoredPosition;
		contPosition2 = contPosition3 + new Vector2(0f, contArrow.transform.parent.GetComponent<RectTransform>().rect.height * 1.09f);
		contPosition1 = contPosition2 + new Vector2(0f, contArrow.transform.parent.GetComponent<RectTransform>().rect.height * 1.09f);
		contArrow.GetComponent<RectTransform>().anchoredPosition = contPosition1;
		refreshContFilesUI();
		continueMenu.SetActive(false);
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
		if (contWait)
			contWait = false;
	}

	public void refreshSaveFilesUI() {
		int i = 1;
		foreach (StartMenuUI s in saveFiles) {
			s.setThisSlot(i);
			s.updateSaveFiles();
			++i;
		}
	}

	public void refreshContFilesUI() {
		int i = 1;
		foreach (ContinueMenuUI c in contFiles) {
			c.setThisSlot(i);
			c.updateContFiles();
			++i;
		}
	}

	public void StartPressed() {
		if (startPosition2 == startPosition3) {
			startPosition2 = startPosition3 + new Vector2(0f, saveArrow.transform.parent.GetComponent<RectTransform>().rect.height * 1.09f);
		}
		if (startPosition1 == startPosition3) {
			startPosition1 = startPosition2 + new Vector2(0f, saveArrow.transform.parent.GetComponent<RectTransform>().rect.height * 1.09f);
			saveArrow.GetComponent<RectTransform>().anchoredPosition = startPosition1;
		}
		saveArrow.GetComponent<RectTransform>().anchoredPosition = startPosition1;
		mainMenuOff();
		startMenu.enabled = true;
		saveWait = true;
	}

	public void StartExitPressed() {
		startMenu.enabled = false;
		Debug.Log("pls");
	
		inputField.transform.FindChild("Placeholder").gameObject.GetComponent<Text>().enabled = true;
		inputField.GetComponent<InputField>().text = "";
		inputField.GetComponent<InputField>().customCaretColor = true;
		mainMenuOn();
	}

	public void ContExitPressed() {
		continueMenu.SetActive(false);
		mainMenuOn();
	}

	public void continuePressed() {
		if (startPosition2 == startPosition3) {
			startPosition2 = startPosition3 + new Vector2(0f, saveArrow.transform.parent.GetComponent<RectTransform>().rect.height * 1.09f);
		}
		if (startPosition1 == startPosition3) {
			startPosition1 = startPosition2 + new Vector2(0f, saveArrow.transform.parent.GetComponent<RectTransform>().rect.height * 1.09f);
			saveArrow.GetComponent<RectTransform>().anchoredPosition = startPosition1;
		}
		saveArrow.GetComponent<RectTransform>().anchoredPosition = startPosition1;
		mainMenuOff();
		continueMenu.SetActive(true);
		contWait = true;
	}

	public void deletePressed() {
		string name = EventSystem.current.currentSelectedGameObject.name;
		int num = int.Parse(name.Substring(name.Length - 1));
		switchSaveButton(num);
		curDelete = saveIndex;
		bool isNewGame = sc.isNewGame(curDelete);
		if (!isNewGame) {
			deleteMenu.SetActive(true);
		}
	}

	public void confirmDelete() {
		sc.deleteSlot(curDelete);
		refreshSaveFilesUI();
		refreshContFilesUI();
		deleteMenu.SetActive(false);
		curDelete = 0;
	}

	public void cancelDelete() {
		deleteMenu.SetActive(false);
		curDelete = 0;
	}

	public void popWarning() {
		warningMenu.SetActive(true);
	}

	public void closeWarning() {
		warningMenu.SetActive(false);
	}

	public void popWarning2() {
		warningMenu2.SetActive(true);
	}

	public void closeWarning2() {
		warningMenu2.SetActive(false);
	}

	public void refreshCursor() {
		inputField.transform.FindChild("Text").gameObject.GetComponent<RectTransform>().pivot = new Vector2 (0.51f, 0.51f);
		inputField.gameObject.transform.FindChild("Text").gameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
		inputField.GetComponent<InputField>().customCaretColor = false;
	}

	public void switchSaveButton(int num) {
		switch (num) {
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

	public void switchContButton(int num) {
		switch (num) {
			case 1:
				contArrow.GetComponent<RectTransform>().anchoredPosition = contPosition1;
				contArrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				contIndex = 1;
				break;
			case 2:
				contArrow.GetComponent<RectTransform>().anchoredPosition = contPosition2;
				contArrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				contIndex = 2;
				break;
			case 3:
				contArrow.GetComponent<RectTransform>().anchoredPosition = contPosition3;
				contArrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				contIndex = 3;
				break;
		}
	}

	public void buttonStartPressed() {
		if (startPosition2 == startPosition3) {
			startPosition2 = startPosition3 + new Vector2(0f, saveArrow.transform.parent.GetComponent<RectTransform>().rect.height * 1.09f);
		}
		if (startPosition1 == startPosition3) {
			startPosition1 = startPosition2 + new Vector2(0f, saveArrow.transform.parent.GetComponent<RectTransform>().rect.height * 1.09f);
			saveArrow.GetComponent<RectTransform>().anchoredPosition = startPosition1;
		}
		if (!saveWait) {
			int name = int.Parse(EventSystem.current.currentSelectedGameObject.name);
			switchSaveButton(name);
		}
	}

	public void buttonContPressed() {
		if (contPosition2 == contPosition3) {
			contPosition2 = contPosition3 + new Vector2(0f, contArrow.transform.parent.GetComponent<RectTransform>().rect.height * 1.09f);
		}
		if (contPosition1 == contPosition3) {
			contPosition1 = contPosition2 + new Vector2(0f, contArrow.transform.parent.GetComponent<RectTransform>().rect.height * 1.09f);
			contArrow.GetComponent<RectTransform>().anchoredPosition = contPosition1;
		}
		if (!contWait) {
			int name = int.Parse(EventSystem.current.currentSelectedGameObject.name);
			switchContButton(name);
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
		bool isNew = sc.isNewGame(contIndex);
		if (!isNew) {
			sc.setContinuing(true);
			sc.setCurrentSlot(contIndex);
			bm.setCanBattle(true);
			SceneManager.LoadScene(1);
		} else {
			popWarning2();
		}
	}

	public void StartGame() {
		bool isNew = sc.isNewGame(saveIndex);
		if (isNew) {
			sc.setContinuing(false);
			bm.setCanBattle(true);
			string saveName = inputField.GetComponent<InputField>().text;
			sc.setCurrentSlot(saveIndex);
			sc.setCurrentName(saveName);
			sc.setNewGame(false);
			SceneManager.LoadScene(1);
		} else {
			popWarning();
		}
		
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

	public void deleteSaves() {
		sc.deleteSave("slot1");
		sc.deleteSave("slot2");
		sc.deleteSave("slot3");
		refreshSaveFilesUI();
		refreshContFilesUI();
	}
}
