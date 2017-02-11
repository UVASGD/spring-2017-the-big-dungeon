using System.Collections;
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

	// Use this for initialization
	void Start () {
        dMan = FindObjectOfType<DialogueManager>();
		loadDialogue (dialogueFile);
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
				dialogueLabels.Add(line.Trim(), pos);
			}
			line = r.ReadLine ();
		}

		dialogueLabels.TryGetValue ("start", out dialogueState);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
		if(other.gameObject.name == "Player" && Input.GetKeyUp(KeyCode.Space))
        {
            if (!dMan.dialogueActive)
            {
                Debug.Log("Calling into Dialogue Manager");
                dMan.dialogueLines = dialogueLines;
                dMan.dialogueState = dialogueState;
				dMan.dialogueLabels = dialogueLabels;
				dMan.initialFrame = true;
				dMan.hasDialogueStateBeenSet = hasDialogueStateBeenSet;
                dMan.ShowDialogue();
            }
        }
    }
}
