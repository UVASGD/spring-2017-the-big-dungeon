using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// The DialogueHolder represents a single dialogue trigger and all associated information
/// to make the dialogue work. The most important information here is simply the file to load
/// the dialogue from -- everything will be parsed from this file. This object simply passes
/// this information to the DialogueManager when it is triggered.
/// </summary>

public class DialogueHolder : MonoBehaviour {

	/*
	 * Dialogue can start from somewhere that isn't the beginning
	 * if you have already talked to the NPC once, so we track whether
	 * we have already triggered this once and reached somewhere else
	*/

    private int dialogueState;
	private bool hasDialogueStateBeenSet = false;


    private DialogueManager dMan;

	// Dialogue script file and parsed information
	public string dialogueFile;
	private List<string> dialogueLines = new List<string>();
	private Dictionary<string, int> dialogueLabels = new Dictionary<string, int>();

    // These variables will be set in the UI
    public string characterName;
    public Sprite faceSprite;

	private bool withinTalkingRange = false;

	private PlayerController player;

    void Start () {
        dMan = FindObjectOfType<DialogueManager>();
		loadDialogue (dialogueFile);
		player = FindObjectOfType<PlayerController>();
	}

	// Loads the script from the file and does some preprocessing
	void loadDialogue(string file) {
		StreamReader r = new StreamReader (file);
		string line = r.ReadLine();
		int pos = 0;

		// We want to keep track of labels so we can jump to them later
		while (line != null) {

			// Check if this is a label or a line
			if (line [0] == '\t') {
				// Regular line
				dialogueLines.Add(line.Trim());
				pos++;
			} else {
                // Label
                line = line.Trim();
                line = line.Substring(0, line.Length - 1);
                dialogueLabels.Add(line, pos);
			}
			line = r.ReadLine ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (withinTalkingRange && Input.GetKeyDown(KeyCode.Space) && !player.inMenu) {
			if (!dMan.dialogueActive) {

				/*
				 * We just directly set our properties on the dialogue manager
				 * so it can use them
				*/
				dMan.dialogueLines = dialogueLines;
				dMan.dialogueLabels = dialogueLabels;
				dMan.dialogueState = dialogueState;
				dMan.faceSprite = faceSprite;
				dMan.characterName = characterName;
				dMan.hasDialogueStateBeenSet = hasDialogueStateBeenSet;
				dMan.initialFrame = true;

				// Trigger the dialogue
				dMan.ShowDialogue(this);
			}
		}
	}

    public void UpdateHasDialogueStateBeenSet(bool hasDialogueStateBeenSet)
    {
        this.hasDialogueStateBeenSet = hasDialogueStateBeenSet;
    }

    public void UpdateDialogueState(int dialogueState)
    {
        this.dialogueState = dialogueState;
    }

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name == "Player") {
			withinTalkingRange = true;
			player.talking = true;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.name == "Player") {
			withinTalkingRange = false;
			player.talking = false;
		}
	}
}
