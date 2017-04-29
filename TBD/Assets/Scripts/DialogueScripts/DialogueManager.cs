using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	// Links to our objects
    public GameObject dBox;
    public Image dFace;
    public Text dName;

	public PlayerController player;
	private NPCManager npcManager;
	private InventoryManager inventoryManager;

	// Dialogue text
    public Text dText;

	private Coroutine ulHolder;

	// Buffer for the current printed text
    private string currentLine;

	// Possible dialogue speeds (these are modifiers on the base speed
    enum Speed { Regular, Drag, Slow, Fast, Hyper, Instant };
	private const double dialogueBaseSpeed = 0.05;

	// This lets us know when we should switch the speed
    private Dictionary<int, Speed> dialogueSpeed = new Dictionary<int, Speed>();
   

	// These variables come from the DialogueHolder
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

	// Used for NPC face sprite and name
    public string characterName;
    public Sprite faceSprite;

	/*
	 * If we're in a cutscene, we should have access to some special
	 * commands, like moving NPCS, moving the player, and moving the camera
	*/
    public bool isCutscene = false;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
		inventoryManager = FindObjectOfType<InventoryManager> ();
        dBox.SetActive(this.dialogueActive);
        npcManager = FindObjectOfType<NPCManager>();
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
					// End the dialogue
                    this.dialogueActive = false;
                    this.dialogueEnd = false;
                    dBox.SetActive(false);
                    if (this.ulHolder != null)
                    {
                        StopCoroutine(this.ulHolder);
                        this.ulHolder = null;
                    }

                    // Update caller information so we can save our state
                    if (this.caller != null)
                    {
                        this.caller.UpdateDialogueState(this.dialogueState);
                        this.caller.UpdateHasDialogueStateBeenSet(this.hasDialogueStateBeenSet);
                        this.caller.faceSprite = this.faceSprite;
                        this.caller.characterName = this.characterName;
                        this.caller = null;
                    }

					// Null out all our objects
                    this.dFace.sprite = null;
                    this.dName.text = "";
                    this.faceSprite = null;
                    this.characterName = null;
                    this.dialogueState = 0;
                    this.hasDialogueStateBeenSet = false;

                    // Make sure we properly end a cutscene vs regular dialogue
                    if (this.isCutscene)
                    {
                        FindObjectOfType<CutsceneManager>().EndCutscene();
                    } else
                    {
                        player.frozen = false;
                    }
                    

                } else
                {
					// Parse the current line of dialogue
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

		// Iterate through our input line to parse script commands
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

				// These are the various dialogue commands
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
					case "hasitem":
						/*
						 * This has a two part argument -- first part is the item name,
						 * and the second part is the label we jump to if the player has it
						*/

						if (this.inventoryManager.hasItem (tokenstr.Split (new Char[] { ':' }) [1])) {
							this.dialogueState = dialogueLabels[tokenstr.Split(new Char[] { ':' })[2]];
							shouldIncrementDialogState = false;
						}
						
						break;

                    case "setnpc":
						/* Changes the NPC we are talking to -- could be useful if you want
						 * to talk to multiple people at once in a cutscene
						*/
                        NPC npc = npcManager.getNPC(tokenstr.Split(new Char[] { ':' })[1]);
                        dFace.sprite = npc.npcSprite;
                        dName.text = npc.npcName;
                        break;
                    default:
					    break;
				}
			}
		}

        // Increment the line, unless we are jumping to another line
        if (shouldIncrementDialogState)
        {
            this.dialogueState++;
        }

        // Update the actual dialogue line onscreen
        UpdateDialogueLine(ret);
	}

	// This is called by the coroutine
    IEnumerator DisplayDialogueLine()
    {
        double currentSpeed = dialogueBaseSpeed;
        this.dText.text = "";
        int len = currentLine.Length;
        bool isInstant = false;
        for(int x = 0; x < len; x++) {

            if (this.dialogueSpeed.ContainsKey(x))
            {
				// Dialogue speed modifiers
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

	// Just call the coroutine to update the line
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

	/*
	 * Trigger the start of dialogue -- if this is called, we already
	 * have a lot of local fields set by the DialogueHolder object
	*/
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

		dBox.SetActive(true);

        // Set the face sprite
        dFace.sprite = this.faceSprite;

        // Set the character name
        dName.text = this.characterName;

        // Process the first line
        ParseDialogueLine(this.dialogueState);


    }
}
