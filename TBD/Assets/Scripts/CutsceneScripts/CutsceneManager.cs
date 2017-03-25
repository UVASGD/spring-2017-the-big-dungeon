using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
	
	private CameraManager cameraManager;
	private PlayerController player;
    private DialogueManager dMan;

    public GameObject cutsceneTop;
    public GameObject cutsceneBottom;
    private RectTransform boxTop;
    private RectTransform boxBottom;

    private Vector2 topPosition;
    private Vector2 bottomPosition;
    private Vector2 topTarget;
    private Vector2 bottomTarget;
    private float startTime;
    private float totalLength;

    private bool cutsceneIsActive = false;

    private bool animateCutsceneBars = false;
    private float cutsceneBarSpeed = 1f;

	// Use this for initialization
	void Start ()
	{
		cameraManager = FindObjectOfType<CameraManager>();
		player = FindObjectOfType<PlayerController> ();
		dMan = FindObjectOfType<DialogueManager>();

        this.boxTop = this.cutsceneTop.GetComponent<RectTransform>();
        this.boxBottom = this.cutsceneBottom.GetComponent<RectTransform>();
    }

	// Update is called once per frame
	void Update () {

        if (this.animateCutsceneBars)
        {
            this.boxBottom.position = new Vector2(this.bottomPosition.x, Mathf.Lerp(this.bottomPosition.y, this.bottomTarget.y, cutsceneBarSpeed * (Time.time - startTime)));
            this.boxTop.position = new Vector2(this.topPosition.x, Mathf.Lerp(this.topPosition.y, this.topTarget.y, cutsceneBarSpeed * (Time.time - startTime)));

            if (Mathf.Approximately(this.topTarget.y, this.boxTop.position.y) && Mathf.Approximately(this.bottomTarget.y, this.boxBottom.position.y)) {
                this.animateCutsceneBars = false;
            }
        }
	}
	
    public bool IsCutsceneActive()
    {
        return this.cutsceneIsActive;
    }

	public void StartCutscene(List<string> dialogueLines, Dictionary<string, int> dialogueLabels, int dialogueState, bool wasStateSet)
	{

        this.cutsceneIsActive = true;

        cameraManager.freeze = true;
		player.frozen = true;
		player.gameObject.GetComponent<Animator>().SetFloat("input_x", 0);
		player.gameObject.GetComponent<Animator>().SetFloat("input_y", -1);

		this.cutsceneBarSpeed = 1f;
        this.startTime = Time.time;
        this.topPosition = this.boxTop.position;
        this.bottomPosition = this.boxBottom.position;
        this.topTarget = this.topPosition + new Vector2(0f, -75f);
        this.bottomTarget = this.bottomPosition + new Vector2(0f, 75f);

        this.animateCutsceneBars = true;

        // The bulk of the work is done by the DialogueManager
        dMan.isCutscene = true;
        dMan.dialogueLines = dialogueLines;
        dMan.dialogueLabels = dialogueLabels;
        dMan.dialogueState = dialogueState;
        dMan.hasDialogueStateBeenSet = wasStateSet;
        dMan.initialFrame = true;

        // Cutscenes are only called once, so don't update our own state after we are finished
        dMan.ShowDialogue(null);
    }

	public void EndCutscene()
	{
        this.startTime = Time.time;
        this.topPosition = this.boxTop.position;
        this.bottomPosition = this.boxBottom.position;
        this.topTarget = this.topPosition + new Vector2(0f, 75f);
        this.bottomTarget = this.bottomPosition + new Vector2(0f, -75f);
        this.cutsceneBarSpeed = 4f;
        this.animateCutsceneBars = true;

        dMan.isCutscene = false;

        // Clean-up and fix DialogueManager
        cameraManager.freeze = false;
		player.frozen = false;
	}

}