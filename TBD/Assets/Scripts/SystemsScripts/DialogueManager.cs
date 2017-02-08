using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    public GameObject dBox;
    public Text dText;
    public PlayerMovement player;
    public bool dialogueActive = false;
    public string[] dialogueLines;
    public int currentLine;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        dBox.SetActive(dialogueActive);
    }

    void Update()
    {
        if (dialogueActive) {
            dText.text = dialogueLines[currentLine];
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("Pressed Space");
                currentLine++;
                if (currentLine >= dialogueLines.Length)
                {
                    dialogueActive = false;
                    dBox.SetActive(false);
                    player.frozen = false;
                    currentLine = 0;
                }
                dText.text = dialogueLines[currentLine];
            }
        }
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
        dBox.SetActive(true);
        player.frozen = true;
        currentLine = 0;
    }
}
