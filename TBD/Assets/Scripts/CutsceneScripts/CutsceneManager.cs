using UnityEngine;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{

	private Camera camera;

	// Use this for initialization
	void Start ()
	{
		camera = GetComponent<Camera>();
	}
	
	public void StartCutscene()
	{
		// The bulk of the work is done by the DialogueManager
	}

	public void EndCutscene()
	{
		// Clean-up and fix DialogueManager
	}

}