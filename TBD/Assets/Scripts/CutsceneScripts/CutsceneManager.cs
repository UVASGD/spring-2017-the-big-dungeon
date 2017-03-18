using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
	
	private Camera mainCamera;
	private CameraManager cameraManager;
	private PlayerController player;

	// Use this for initialization
	void Start ()
	{
		mainCamera = Camera.main;
		cameraManager = FindObjectOfType<CameraManager>();
		Debug.Log (cameraManager);
		player = FindObjectOfType<PlayerController> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.L))
		{ 
			StartCutscene ();
		}
	}
	
	public void StartCutscene()
	{
		cameraManager.freeze = true;
		player.frozen = true;

		// The bulk of the work is done by the DialogueManager
	}

	public void EndCutscene()
	{

		mainCamera.ResetProjectionMatrix ();

		// Clean-up and fix DialogueManager
		cameraManager.freeze = false;
		player.frozen = false;
	}

}