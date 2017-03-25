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
		hpMaxText.text = "" + player.getCurrentStatValue ("HP");
		nameText.text = player.getPlayerName ();

        mainMenu.enabled = true;
        quitMenu.enabled = false;

        // Make sure menu is disabled by default
        DisableMenu();

        SceneManager.sceneLoaded += OnSceneWasLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneWasLoaded;
    }

    public void EnableMenu()
    {
        attackText.enabled = true;
        defendText.enabled = true;
        itemText.enabled = true;
        runText.enabled = true;

        attackText.GetComponentInParent<Text>().text = "Attack";
        defendText.GetComponentInParent<Text>().text = "Defend";
        itemText.GetComponentInParent<Text>().text = "Item";
        runText.GetComponentInParent<Text>().text = "Run!";
    }

    public void DisableMenu()
    {
        attackText.enabled = false;
        defendText.enabled = false;
        itemText.enabled = false;
        runText.enabled = false;

        attackText.GetComponentInParent<Text>().text = "";
        defendText.GetComponentInParent<Text>().text = "";
        itemText.GetComponentInParent<Text>().text = "";
        runText.GetComponentInParent<Text>().text = "";
    }

    public void RunPressed()
    {
        this.battleManager.addState(new TextState("Player tried to run... and was successful!"));
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
        this.battleManager.addState(new TextState("Player tried to use an item, but was unable..."));
        this.battleManager.addState(new EnemyState());
        this.DisableMenu();
        this.battleManager.ProcessState();
    }

    public void DefendPressed()
    {
        this.battleManager.addState(new TextState("Player tried to defend, but was unable..."));
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
