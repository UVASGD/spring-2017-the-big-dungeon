using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameObject dBox;
    public Image dFace;
    public Text dName;

    public Text dText;
    private string currentLine;
    enum Speed { Regular, Drag, Slow, Fast, Hyper, Instant };
    private Dictionary<int, Speed> dialogueSpeed = new Dictionary<int, Speed>();
    private Coroutine ulHolder;
    private const double dialogueBaseSpeed = 0.05;

    public PlayerController player;
    public bool dialogueActive = false;
	public List<string> dialogueLines;
	public Dictionary<string, int> dialogueLabels;
	public int dialogueState;
	public bool initialFrame;
	public bool hasDialogueStateBeenSet = false;
	private bool dialogueEnd;
    private bool dialogueDuringOutput;
    private DialogueHolder caller;
    private int x;

    public string characterName;
    public Sprite faceSprite;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        dBox.SetActive(this.dialogueActive);
    }

    void Update()
    {
        if (this.dialogueActive) {
			if (Input.GetKeyUp(KeyCode.Space) && !this.initialFrame) {

                // Check if we're in the process of writing out a dialogue line
                // If so, just finish the current line

                if (this.dialogueDuringOutput)
                {
                    if (this.ulHolder != null)
                    {
                        StopCoroutine(this.ulHolder);
                        this.ulHolder = null;
                    }

                    this.dText.text = currentLine;
                    this.dialogueDuringOutput = false;

                } else if (dialogueState >= dialogueLines.Count || dialogueEnd)
                {
                    this.dialogueActive = false;
                    this.dialogueEnd = false;
                    dBox.SetActive(false);
                    if (this.ulHolder != null)
                    {
                        StopCoroutine(this.ulHolder);
                        this.ulHolder = null;
                    }
                    player.frozen = false;

                    // Update caller information so we can save our state
                    this.caller.UpdateDialogueState(this.dialogueState);
                    this.caller.UpdateHasDialogueStateBeenSet(this.hasDialogueStateBeenSet);
                    this.caller.faceSprite = this.faceSprite;
                    this.caller.characterName = this.characterName;

                    this.dialogueState = 0;
                    this.hasDialogueStateBeenSet = false;
                    this.faceSprite = null;
                    this.dFace.sprite = null;
                    this.dName.text = "";
                    this.characterName = null;
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

        this.dialogueSpeed.Clear();

        this.dialogueDuringOutput = true;

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
				ret = ret + line.Substring (pos, Math.Max(start - pos, 0));
				pos = line.IndexOf ("}", start) + 1;
				string tokenstr = line.Substring(start + 1, pos - start - 2);

                // Process the token string
                string tokencmd = tokenstr.Split(new Char[] { ':' })[0].ToLower();

                switch (tokencmd)
				{
				    case "end":
					    dialogueEnd = true;
					    break;
                    case "goto":
                        this.dialogueState = dialogueLabels[tokenstr.Split(new Char[] { ':' })[1]];
                        shouldIncrementDialogState = false;
					    break;
                    case "fast":
                        this.dialogueSpeed[ret.Length] = Speed.Fast;
                        break;
                    case "hyper":
                        this.dialogueSpeed[ret.Length] = Speed.Hyper;
                        break;
                    case "slow":
                        this.dialogueSpeed[ret.Length] = Speed.Slow;
                        break;
                    case "drag":
                        this.dialogueSpeed[ret.Length] = Speed.Drag;
                        break;
                    case "instant":
                        this.dialogueSpeed[ret.Length] = Speed.Instant;
                        break;
                    case "endspeed":
                        this.dialogueSpeed[ret.Length] = Speed.Regular;
                        break;
                    default:
					    break;
				}
			}
		}

        // Increment the line unless we called goto
        if (shouldIncrementDialogState)
        {
            this.dialogueState++;
        }

        // Update the actual dialogue line
        UpdateDialogueLine(ret);
	}

    IEnumerator DisplayDialogueLine()
    {
        double currentSpeed = dialogueBaseSpeed;
        this.dText.text = "";
        int len = currentLine.Length;
        bool isInstant = false;
        for(int x = 0; x < len; x++) {

            if (this.dialogueSpeed.ContainsKey(x))
            {
                switch(this.dialogueSpeed[x])
                {
                    case Speed.Drag:
                        currentSpeed = dialogueBaseSpeed * 4.0;
                        break;
                    case Speed.Slow:
                        currentSpeed = dialogueBaseSpeed * 2.0;
                        break;
                    case Speed.Regular:
                        currentSpeed = dialogueBaseSpeed * 1.0;
                        break;
                    case Speed.Fast:
                        currentSpeed = dialogueBaseSpeed * 0.5;
                        break;
                    case Speed.Hyper:
                        currentSpeed = dialogueBaseSpeed * 0.25;
                        break;
                    case Speed.Instant:
                        // Find the next speed, if there is one
                        // Otherwise, just print out the rest of the line
                        string buf = "" + currentLine.ElementAt(x);
                        x = x + 1;
                        while (x < len) {
                            if (this.dialogueSpeed.ContainsKey(x))
                            {
                                break;
                            }
                            buf += currentLine.ElementAt(x++);
                        }

                        this.dText.text += buf;

                        if (x < len)
                        {
                            x = x - 1;
                        }
                        isInstant = true;
                        break;
                    default:
                        break;
                }
            }

            yield return new WaitForSeconds ((float) currentSpeed);

            // If it was "instant" speed, we don't want to add the char here
            // because it was already added above
            if (!isInstant)
                this.dText.text += currentLine.ElementAt(x);
            else
                isInstant = false;

        }

        this.dialogueDuringOutput = false;

    }

    public void UpdateDialogueLine(string line)
    {
        if (this.ulHolder != null)
        {
            StopCoroutine(ulHolder);
            this.ulHolder = null;
        }

        this.currentLine = line;
        this.ulHolder = StartCoroutine(DisplayDialogueLine());
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

        // Set the face sprite
        dFace.sprite = this.faceSprite;

        // Set the character name
        dName.text = this.characterName;

        // Process the first line
        ParseDialogueLine(this.dialogueState);

        dBox.SetActive(true);

    }
}
