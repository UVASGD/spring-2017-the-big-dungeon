using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
		optionsMenu.enabled = false;
		creditsBackground.SetActive(false);
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
		SceneManager.LoadScene(1);
	}

	public void StartGame() {
		sc.setContinuing(false);
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
		/*
		Color colorBlack = new Color(0f, 0f, 0f, 255f);
		foreach (Image i in creditsBackground.GetComponentsInChildren<Image>()) {
			i.GetComponent<CanvasRenderer>().SetColor(colorBlack);
		}*/
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
