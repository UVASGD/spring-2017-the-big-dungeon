using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameObject dBox;
    public Text dText;
    public PlayerMovement player;
    public bool dialogueActive = false;
	public List<string> dialogueLines;
	public Dictionary<string, int> dialogueLabels;
	public int dialogueState;
	public bool initialFrame;
	public bool hasDialogueStateBeenSet = false;
	private bool dialogueEnd;
    private DialogueHolder caller;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        dBox.SetActive(this.dialogueActive);
    }

    void Update()
    {
        if (this.dialogueActive) {
			if (Input.GetKeyUp(KeyCode.Space) && !this.initialFrame) {
				if (dialogueState >= dialogueLines.Count || dialogueEnd)
                {
                    this.dialogueActive = false;
                    this.dialogueEnd = false;
                    dBox.SetActive(false);
                    player.frozen = false;

                    // Update caller information so we can save our state
                    this.caller.dialogueState = this.dialogueState;
                    this.caller.hasDialogueStateBeenSet = this.hasDialogueStateBeenSet;

                    this.dialogueState = 0;
                    this.hasDialogueStateBeenSet = false;
                    this.caller = null;

                } else
                {
                    ParseDialogueLine(this.dialogueState);
                }
            }
            this.initialFrame = false;
        }
    }

	public void ParseDialogueLine(int dialogueState) {
        string line = dialogueLines[dialogueState];
        string ret = "";
        bool shouldIncrementDialogState = true;

		int pos = 0;
		while (pos < line.Length) {
			int start = line.IndexOf ("{", pos);
			if (start == -1) {
				if (pos < line.Length) {
					ret = ret + line.Substring (pos);
					pos = line.Length;
				}
			} else {
                // Get the current token and act on it
				ret = ret + line.Substring (pos, Math.Max(start - pos - 1, 0));
				pos = line.IndexOf ("}", start) + 1;
				string tokenstr = line.Substring(start + 1, pos - start - 2);

                // Process the token string
                string tokencmd = tokenstr.Split(new Char[] { ':' })[0];

                switch (tokencmd)
				{
				    case "end":
					    dialogueEnd = true;
					    break;
				    case "goto":
                        this.dialogueState = dialogueLabels[tokenstr.Split(new Char[] { ':' })[1]];
                        shouldIncrementDialogState = false;
					    break;
				    default:
					    break;
				}
			}
		}

        if (shouldIncrementDialogState)
        {
            this.dialogueState++;
        }

        dText.text = ret;
	}

    public void ShowDialogue(DialogueHolder caller)
    {
        this.dialogueActive = true;
        this.dialogueEnd = false;
        this.caller = caller;
        player.frozen = true;

        // Check if we've given any dialogue before
        if (!this.hasDialogueStateBeenSet)
        {
            dialogueLabels.TryGetValue("start", out this.dialogueState);
            this.hasDialogueStateBeenSet = true;
        }

        // Check if we need to reset the dialogue state
        if (dialogueState >= dialogueLines.Count)
        {
            this.dialogueState = 0;
        }

        // Process the first line
        ParseDialogueLine(this.dialogueState);

        dBox.SetActive(true);

    }
}
