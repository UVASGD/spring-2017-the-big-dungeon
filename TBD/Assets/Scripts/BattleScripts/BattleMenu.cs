using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleMenu : MonoBehaviour
{
    public BattleManager battleManager;
    public Canvas quitMenu;
    public Button attackText;
    public Button itemText;
    public Button defendText;
    public Button runText;
    public Text hpText;
	public Text hpMaxText;
	public Text nameText;

	public Image enemySprite;

    private Canvas mainMenu;
	private PlayerController player;

	public Text keyText;
	public Image keyBack;

	private int xIndex = 0;
	private int yIndex = 0;
	private int totalX = 2;
	private int totalY = 2;
	public GameObject arrow;
	private Vector2 yOffset = new Vector3(0f, 100f);
	private Vector2 xOffset = new Vector3(400f, 0f);
	private Vector2 startPosition;
	private bool isOdd = false;
	private bool canClick = false;
	private bool isActive = true;

    // Use this for initialization
    void Awake()
    {
        mainMenu = GetComponent<Canvas>();
        quitMenu = quitMenu.GetComponent<Canvas>();
        attackText = attackText.GetComponent<Button>();
        itemText = itemText.GetComponent<Button>();
        defendText = defendText.GetComponent<Button>();
        runText = runText.GetComponent<Button>();
        hpText = hpText.GetComponent<Text>();
		hpMaxText = hpMaxText.GetComponent<Text>();
		nameText = nameText.GetComponent<Text> ();

		enemySprite = enemySprite.GetComponent<Image> ();

		player = FindObjectOfType<PlayerController> ();
		hpText.text = "" + player.getCurrentStatValue ("HP");
		hpMaxText.text = "" + player.getBaseStatValue ("HP");
		nameText.text = player.getPlayerName ();

        mainMenu.enabled = true;
        quitMenu.enabled = false;

		attackText.enabled = false;
		defendText.enabled = false;
		itemText.enabled = false;
		runText.enabled = false;

		startPosition = arrow.GetComponent<RectTransform>().anchoredPosition;

        // Make sure menu is disabled by default
        DisableMenu();

        SceneManager.sceneLoaded += OnSceneWasLoaded;
    }

	void Update() {
		if (isActive) {
			if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
				if (yIndex > 0) {
					yIndex--;
					Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
					arrowPosition += yOffset;
					arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
					arrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				}
			}
			if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
				if ((yIndex < totalY - 1)) {
					if (!isOdd || !(xIndex == 1) || yIndex < totalY - 2) {
						yIndex++;
						Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
						arrowPosition -= yOffset;
						arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
						arrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
					}
				}
			}
			if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
				if (xIndex > 0) {
					xIndex--;
					Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
					arrowPosition -= xOffset;
					arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
					arrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				}
			}
			if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
				if ((xIndex < totalX - 1) && (!isOdd || !(yIndex == totalY - 1))) {
					xIndex++;
					Vector2 arrowPosition = arrow.GetComponent<RectTransform>().anchoredPosition;
					arrowPosition += xOffset;
					arrow.GetComponent<RectTransform>().anchoredPosition = arrowPosition;
					arrow.GetComponent<Animator>().SetTrigger("ArrowRestart");
				}
			}
			if (Input.GetKeyDown(KeyCode.Space) && canClick) {
				if (xIndex == 0 && yIndex == 0) {
					AttackPressed ();
				} else if (xIndex == 0 && yIndex == 1) {
					DefendPressed ();
				} else if (xIndex == 1 && yIndex == 0) {
					ItemPressed ();
				} else if (xIndex == 1 && yIndex == 1) {
					RunPressed ();
				}
				canClick = false;
				isActive = false;
			}
		}
		canClick = true;
	}

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneWasLoaded;
    }

    public void EnableMenu()
    {
		/*
        attackText.enabled = true;
        defendText.enabled = true;
        itemText.enabled = true;
        runText.enabled = true;*/
		arrow.GetComponent<RectTransform>().anchoredPosition = startPosition;
		xIndex = 0;
		yIndex = 0;
		isActive = true;

        attackText.GetComponentInParent<Text>().text = "Attack";
        defendText.GetComponentInParent<Text>().text = "Defend";
        itemText.GetComponentInParent<Text>().text = "Item";
        runText.GetComponentInParent<Text>().text = "Run!";
		arrow.GetComponentInParent<Text>().text = ">";
    }

    public void DisableMenu()
    {
        
        attackText.GetComponentInParent<Text>().text = "";
        defendText.GetComponentInParent<Text>().text = "";
        itemText.GetComponentInParent<Text>().text = "";
        runText.GetComponentInParent<Text>().text = "";
		arrow.GetComponentInParent<Text>().text = "";

    }

    public void RunPressed()
    {
        this.battleManager.addState(new TextState("You tried to run... and was successful!"));
        this.battleManager.addState(new TextState("{end}"));
        this.DisableMenu();
        this.battleManager.ProcessState();
    }

    public void AttackPressed()
    {
		this.DisableMenu();
		this.battleManager.playerAttack ();
    }

    public void ItemPressed()
    {
        this.battleManager.addState(new TextState("You tried to use an item, but was unable..."));
        this.battleManager.addState(new EnemyState());
        this.DisableMenu();
        this.battleManager.ProcessState();
    }

    public void DefendPressed()
    {
        this.battleManager.addState(new TextState("You brace for the next attack..."));
        this.battleManager.addState(new EnemyState());
        this.DisableMenu();
        this.battleManager.ProcessState();
    }

    // Make sure the BattleMenu is loaded before we start any battle logic
    private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        // We only care about the Battle Scene
        if (scene.buildIndex != 2)
            return;
        this.battleManager = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();
        this.battleManager.LoadBattleMenu(this);
        this.battleManager.ProcessState();
    }
}
