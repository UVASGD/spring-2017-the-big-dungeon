using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueHolder : MonoBehaviour {

    private int dialogueState;
	private bool hasDialogueStateBeenSet = false;
    private DialogueManager dMan;
	public string dialogueFile;
	private List<string> dialogueLines = new List<string>();
	private Dictionary<string, int> dialogueLabels = new Dictionary<string, int>();

    // These variables will be set in the UI
    public string characterName;
    public Sprite faceSprite;

	private bool withinTalkingRange = false;

	private PlayerController player;

    // Use this for initialization
    void Start () {
        dMan = FindObjectOfType<DialogueManager>();
		loadDialogue (dialogueFile);
		player = FindObjectOfType<PlayerController>();
	}

	void loadDialogue(string file) {
		StreamReader r = new StreamReader (file);
		string line = r.ReadLine();
		int pos = 0;
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
				dMan.dialogueLines = dialogueLines;
				dMan.dialogueLabels = dialogueLabels;
				dMan.dialogueState = dialogueState;
				dMan.faceSprite = faceSprite;
				dMan.characterName = characterName;
				dMan.hasDialogueStateBeenSet = hasDialogueStateBeenSet;
				dMan.initialFrame = true;
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
