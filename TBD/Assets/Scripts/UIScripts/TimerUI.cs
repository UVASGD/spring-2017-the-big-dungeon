using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour {
	Image fillImg;
	float timeAmt = 10f;
	float time;
	public bool timerOn = false;

	// Use this for initialization
	void Start () {
		this.fillImg = this.GetComponent<Image> ();
		this.time = this.timeAmt;
		this.timerOn = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.time > 0 && this.timerOn) {
			this.time -= Time.deltaTime;
			this.fillImg.fillAmount = this.time / this.timeAmt;
		}
	}

	public void startTimer(){
		this.timerOn = true;
	}

	public void resetTimer() {
		this.time = this.timeAmt;
		this.fillImg.fillAmount = this.time / this.timeAmt;
		this.timerOn = false;
	}

	public void changeMaxTime(float newTime){
		this.timeAmt = newTime;
		this.time = this.timeAmt;
	}

	public bool isOutOfTime() {
		return time <= 0;
	}

	public void hideTimer() {
		this.fillImg = this.GetComponent<Image> ();
		Color c = this.fillImg.color;
		c.a = 0;
		this.fillImg.color = c;
	}

	public void showTimer() {
		Color c = this.fillImg.color;
		c.a = 255;
		this.fillImg.color = c;
	}
}
