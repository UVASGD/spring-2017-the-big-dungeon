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
	public bool hasDialogueStateBeenSet;
	public bool dialogueEnd;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        dBox.SetActive(dialogueActive);
    }

    void Update()
    {
        if (dialogueActive) {
			if (Input.GetKeyUp(KeyCode.Space) && !initialFrame) {
                dialogueState++;
				if (dialogueState >= dialogueLines.Count || dialogueEnd)
                {
                    dialogueActive = false;
					dialogueEnd = false;
                    dBox.SetActive(false);
                    player.frozen = false;

					// Dialogue is over, see if we need to reset the dialogue state
					if (!hasDialogueStateBeenSet) {
						dialogueLabels.TryGetValue ("start", out dialogueState);
					}
                }
				string output = ParseDialogueLine (dialogueState);
                dText.text = output;
            }
			initialFrame = false;
        }
    }

	public string ParseDialogueLine(int dialogueState) {
		string line = dialogueLines[dialogueState];
		string ret = "";

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
				pos = line.IndexOf ("}", start);
				string tokenstr = line.Substring(start + 1, pos - start - 1);

				// Process the token string
				switch (tokenstr)
				{
				case "end":
					dialogueEnd = true;
					break;
				case "goto":
					dialogueState = Int32.Parse (tokenstr.Split (new Char[] {':'}) [1]);
					break;
				default:
					break;
				}
				Debug.Log (tokenstr);
			}
		}

		return ret;
	}


    public void ShowBox(string dialogue)
    {
        dBox.SetActive(true);
        dialogueActive = true;
        player.frozen = true;
        dText.text = dialogue;
    }

    public void ShowDialogue()
    {
        Debug.Log("Activating now");
        dialogueActive = true;
		dialogueEnd = false;
        dBox.SetActive(true);
        player.frozen = true;
    }
}
