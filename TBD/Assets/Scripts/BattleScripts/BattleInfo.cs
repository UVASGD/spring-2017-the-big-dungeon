using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleInfo : MonoBehaviour
{

    public Text dText;
    private string currentLine;
    private Coroutine ulHolder;
    private const double dialogueBaseSpeed = 0.05;

    private bool dialogueActive = false;
    private List<string> dialogueLines;
    public Dictionary<string, int> dialogueLabels;
    private int dialogueState = 0;
    private bool initialFrame = true;
    private bool dialogueEnd;
    private bool dialogueDuringOutput;
    private int x;

    private BattleManager battleManager;

    void Start()    
    {
        this.dText = dText.GetComponent<Text>();
    }

    void Update()
    {
        if (this.dialogueActive)
        {
            if (Input.GetKeyUp(KeyCode.Space) && !this.initialFrame)
            {
                // Check if we're in the process of writing out a dialogue line
                // If so, just finish the current line

                if (this.dialogueDuringOutput)
                {
                    if (this.ulHolder != null)
                    {
                        StopCoroutine(this.ulHolder);
                        this.ulHolder = null;
                    }

                    this.dText.text = "<color=#D4CA41FF>></color> " + currentLine;
                    this.dialogueDuringOutput = false;

                }
                else if (dialogueState >= dialogueLines.Count || dialogueEnd)
                {
                    // End the dialogue
                    this.dialogueActive = false;
                    this.dialogueEnd = false;
                    if (this.ulHolder != null)
                    {
                        StopCoroutine(this.ulHolder);
                        this.ulHolder = null;
                    }

                    this.dialogueState = 0;
                    this.dText.text = "";
                    this.battleManager.ProcessState();
                }
                else
                {
                    ParseDialogueLine(this.dialogueState);
                }
            }
            this.initialFrame = false;
        }
    }

    public void ParseDialogueLine(int dialogueState)
    {
        string line = dialogueLines[dialogueState];

        if (line.Equals("{end}"))
        {
            this.battleManager.EndBattle();
        }
        else
        {
            this.dialogueDuringOutput = true;
            UpdateDialogueLine(line);
            this.dialogueState++;
        }
    }

    IEnumerator DisplayDialogueLine()
    {
        double currentSpeed = dialogueBaseSpeed * 0.25;
        this.dText.text = "<color=#D4CA41FF>></color> ";
        int len = currentLine.Length;
        for (int x = 0; x < len; x++)
        {

            yield return new WaitForSeconds((float)currentSpeed);

            this.dText.text += currentLine[x];

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

    public void ShowDialogue(List<string> lines)
    {
        this.dialogueEnd = false;

        this.dialogueLines = lines;
        this.dialogueState = 0;
        this.dialogueActive = true;

        // Process the first line
        ParseDialogueLine(this.dialogueState);
    }

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneWasLoaded;
    }

    void Destroy()
    {
        SceneManager.sceneLoaded -= OnSceneWasLoaded;
    }

    private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        // We only care about the Battle Scene
        if (scene.buildIndex != 2)
            return;
        this.battleManager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
        this.battleManager.LoadBattleInfo(this);
    }
}
