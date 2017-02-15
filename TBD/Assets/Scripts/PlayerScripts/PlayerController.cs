using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	private SFXManager sfxMan;
	private Animator anim;
	private Rigidbody2D rbody;

	public bool frozen = false;

	public AudioSource[] playerStepSounds;
	
	// How often the step sound occurs
	private float stepInterval = 0.25f;
	private float timer = 0.0f;
	private AudioSource currentStep;

	private bool stepsOn;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody2D>();
		sfxMan = FindObjectOfType<SFXManager>();

		if (playerStepSounds.Length > 0) {
			currentStep = playerStepSounds[Random.Range(0, playerStepSounds.Length)];
			stepsOn = true;
			sfxMan.GroundChange("default");
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 movement_vector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * 2.5f;

		/*
		if (stepsOn) {
			if (currentStep.time > 0.2f) {
				currentStep.Stop();
			}
		}
		*/

		if (movement_vector != Vector2.zero && !frozen) {
			anim.SetBool("is_walking", true);
			anim.SetFloat("input_x", movement_vector.x);
			anim.SetFloat("input_y", movement_vector.y);
			if (stepsOn)
				timer += Time.deltaTime;

			if (timer > stepInterval) {
				timer = 0;
				PlayNextSound();
			}
		}
		else {
			anim.SetBool("is_walking", false);
			timer = 0;
		}
		if (!frozen) {
			rbody.MovePosition(rbody.position + movement_vector * Time.deltaTime);
		}

		
	}

	void PlayNextSound() {
		AudioSource lastStep = currentStep;
		while (currentStep == lastStep && playerStepSounds.Length > 1) {
			currentStep = playerStepSounds[Random.Range(0, playerStepSounds.Length)];
		}
		if (lastStep != null)
            sfxMan.StopSFX(lastStep);
        if (currentStep != null)
            sfxMan.PlaySFX(currentStep);

    }

	void OnTriggerEnter2D(Collider2D other) {
		if (stepsOn) {
			if (other.transform.tag == "path") {
				sfxMan.GroundChange("path");
			}
			else if (other.transform.tag == "grass") {
				sfxMan.GroundChange("grass");
			}
			else
				sfxMan.GroundChange("default");
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (stepsOn) {
			sfxMan.GroundChange("default");
		}
	}

	public void UpdateGround(AudioSource[] stepSounds) {
		if (playerStepSounds != null) {
			playerStepSounds = stepSounds;
			stepsOn = true;
			if (currentStep != null)
				currentStep.Stop();
			currentStep = playerStepSounds[Random.Range(0, playerStepSounds.Length)];
		}
	}

}
