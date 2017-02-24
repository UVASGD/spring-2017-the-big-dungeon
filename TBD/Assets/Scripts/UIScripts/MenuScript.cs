using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public Canvas quitMenu;
	public Button startText;
	public Button continueText;
	public Button optionsText;
	public Button creditsText;
	public Button exitText;

	public Canvas optionsMenu;

	private SaveController saveCont;

	// Use this for initialization
	void Start () {
		quitMenu = quitMenu.GetComponent<Canvas>();
		startText = startText.GetComponent<Button>();
		continueText = continueText.GetComponent<Button>();
		optionsText = optionsText.GetComponent<Button>();
		creditsText = creditsText.GetComponent<Button>();
		exitText = exitText.GetComponent<Button>();
		optionsMenu = optionsMenu.GetComponent<Canvas>();
		quitMenu.enabled = false;
		optionsMenu.enabled = false;
		saveCont = FindObjectOfType<SaveController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ExitPressed() {
		quitMenu.enabled = true;
		startText.enabled = false;
		continueText.enabled = false;
		optionsText.enabled = false;
		creditsText.enabled = false;
		exitText.enabled = false;
	}

	public void NoExitPressed() {
		quitMenu.enabled = false;
		startText.enabled = true;
		continueText.enabled = true;
		optionsText.enabled = true;
		creditsText.enabled = true;
		exitText.enabled = true;
	}

	public void ContinueGame(){
		saveCont.isContinuing = true;
		SceneManager.LoadScene(1);
	}

	public void StartGame() {
		saveCont.isContinuing = false;
		SceneManager.LoadScene(1);
		
	}

	public void ExitGame() {
		Application.Quit();
	}

	public void OptionsPressed() {
		optionsMenu.enabled = true;
		this.GetComponent<Canvas>().enabled = false;
		startText.enabled = false;
		continueText.enabled = false;
		optionsText.enabled = false;
		creditsText.enabled = false;
		exitText.enabled = false;
	}

	public void CloseOptions() {
		optionsMenu.enabled = false;
		this.GetComponent<Canvas>().enabled = true;
		startText.enabled = true;
		continueText.enabled = true;
		optionsText.enabled = true;
		creditsText.enabled = true;
		exitText.enabled = true;
	}
}
