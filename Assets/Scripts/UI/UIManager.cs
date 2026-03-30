using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Manager for setting up and changing anything UI related
/// Note: Used to be MoneyManager, but decided a general purpose UIManager should oversee all UI management
/// <br/> - Nicholas Liang (Feb. 9th, 2026)
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Manager References")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private MouseManager mouseManager;
    [SerializeField] private GameManager gameManager;

    [Header("References")]
    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private TextMeshProUGUI goblinCounter;
    [SerializeField] private TextMeshProUGUI healthCounter;
    [SerializeField] private GameObject moneyChangeTextPrefab;
    [SerializeField] private GameObject healthChangeTextPrefab;
    [SerializeField] private GameObject topBar;
    [SerializeField] private GameObject towersPanel;
    [SerializeField] private GameObject towerIconPrefab;
    [SerializeField] private TowerInfoPopup towerInfoPopup;
    [SerializeField] private GameObject announcementMessagePrefab;

    [Header("SFX")]
    [SerializeField] private AudioClip towersPanelSound;
    [SerializeField] private AudioClip sellSound;
    [SerializeField] private AudioClip invalidSound1;
    [SerializeField] private AudioClip invalidSound2;

    [Header("Tower Stats Panel References")]
    [SerializeField] private GameObject towerStatsPanel;
    [SerializeField] private TowerTile currentlySelectedTower;
    [SerializeField] private Image towerArt;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI dmgText;
    [SerializeField] private TextMeshProUGUI atkSpdText;
    [SerializeField] private TextMeshProUGUI rangeText;

    [Header("UI Information")]
    [SerializeField] private int money;
    [SerializeField] private int health;
    [SerializeField] private int pathPrice;
    [SerializeField] private TowerData archerData;
    [SerializeField] private TowerData gnomeData;
    private List<GameObject> towersList;
    private int announcementsNum = 0;

    [Header("Constants")]
    private readonly int MAX_ANNOUNCEMENTS = 10;
    private readonly int TOWER_PANEL_Y_OFFSET = 225;
    private readonly int TOWER_STATS_PANEL_X_OFFSET = 260;
    private readonly int MAX_TOWER_ICONS = 8;
    private readonly Vector2 MONEY_CHANGE_ORIGINAL_POS = new Vector2(-34, 0);
    private readonly Vector2 HEALTH_CHANGE_ORIGINAL_POS = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        goblinCounter.text = money.ToString();
        SetupTopBarTowersUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method for setting up all the purchasable tower UI in the top bar
    // Should be called upon start for setup
    private void SetupTopBarTowersUI()
    {
        // Setup top bar by instantiating TowerUIs, setting their stats, and assign references they need
        // Initialize towersList and add instantiated towerUIs to towersList to keep track of them for later use
        towersList = new List<GameObject>();

        // TODO: Refactor this to work with a list of tower data instead of 2 hard coded references
        // Placeholder towers for now
        // Archer: 2 price, 1 dmg, 0.1s spd, 8 range
        GameObject archer = Instantiate(towerIconPrefab, towersPanel.transform);
        archer.GetComponent<TowerIcon>().AssignReferences(uiManager, gridManager, mouseManager, towerInfoPopup);
        archer.GetComponent<TowerIcon>().SetData(archerData);
        towersList.Add(archer);

        // Gnome Shooter: 10 price, 2 dmg, 1s spd, 10 range
        GameObject gnome = Instantiate(towerIconPrefab, towersPanel.transform);
        gnome.GetComponent<TowerIcon>().AssignReferences(uiManager, gridManager, mouseManager, towerInfoPopup);
        gnome.GetComponent<TowerIcon>().SetData(gnomeData);
        towersList.Add(gnome);

        // Fill remaining slots with greyed out (empty) tower icons
        for (int i = 0; towersList.Count < MAX_TOWER_ICONS; i++)
        {
            GameObject emptyIcon = Instantiate(towerIconPrefab, towersPanel.transform);
            emptyIcon.GetComponent<TowerIcon>().AssignReferences(uiManager, gridManager, mouseManager, towerInfoPopup);
            emptyIcon.GetComponent<TowerIcon>().MakeEmpty();
            towersList.Add(emptyIcon);
        }
    }

    /// <summary>
    /// Hide the tower panel
    /// </summary>
    public void HideTowerPanel()
    {
        AudioManager.instance.PlaySoundFX(towersPanelSound, transform, 0.3f);
        float duration = 0.5f;
        towersPanel.transform.LeanMoveLocalY(TOWER_PANEL_Y_OFFSET, duration).setEaseInCubic();
        towerInfoPopup.enabled = false;
    }

    /// <summary>
    /// Show the tower panel
    /// </summary>
    public void ShowTowerPanel()
    {
        AudioManager.instance.PlaySoundFX(towersPanelSound, transform, 0.6f);
        float duration = 0.5f;
        towersPanel.transform.LeanMoveLocalY(0, duration).setEaseOutCubic();
        towerInfoPopup.enabled = true;
    }

    /// <summary>
    /// Display the stats of given tower
    /// </summary>
    public void DisplayTowerStatsPanel(TowerTile tower)
    {
        // Stop outlining previously selected tower if it exists
        if (currentlySelectedTower != null) currentlySelectedTower.HideSelectionOutline();

        // Update currently selected tower
        currentlySelectedTower = tower;
        currentlySelectedTower.ShowSelectionOutline();

        float slideOutDuration = 0.05f;
        float slideInDuration = 0.1f;

        // Slide tower stats panel off-screen
        towerStatsPanel.GetComponent<RectTransform>().LeanMoveX(-TOWER_STATS_PANEL_X_OFFSET, slideOutDuration)
            .setEaseInCubic()
            // Then seamlessly switch stats once off-screen
            .setOnComplete(() => UpdateTowerStatsPanel(currentlySelectedTower));

        // Before sliding tower panel back out with new stats
        towerStatsPanel.GetComponent<RectTransform>().LeanMoveX(0, slideInDuration)
            .setEaseOutCubic()
            .setDelay(2* slideOutDuration); // Delay until slide out animation is done (with a bit of a pause)
    }

    /// <summary>
    /// Private helper for updating the tower stats panel
    /// </summary>
    /// <param name="tower">Tower to update stats to</param>
    private void UpdateTowerStatsPanel(TowerTile tower)
    {
        towerArt.sprite = tower.data.sprite;
        towerName.text = tower.data.towerName;
        dmgText.text = "DMG " + tower.data.damage.ToString();
        atkSpdText.text = "SPD " + tower.data.attackSpeed.ToString() + "s";
        rangeText.text = "Range " + tower.data.attackRange.ToString();
    }

    /// <summary>
    /// Briefly display a message in the middle of the screen as an announcement to the player
    /// </summary>
    public void DisplayGameAnnouncement(string message)
    {

        if (announcementsNum >= MAX_ANNOUNCEMENTS) // Check if max number of announcements has been exceeded
        {
            return;
        }
        announcementsNum++;

        // Play error sound
        AudioManager.instance.PlaySoundFX(invalidSound1, transform, 0.4f);
        AudioManager.instance.PlaySoundFX(invalidSound2, transform, 0.4f);

        // Create message popup with given string
        GameObject announcement = Instantiate(announcementMessagePrefab, uiCanvas.transform);
        announcement.transform.SetSiblingIndex(0);
        TextMeshProUGUI messageText = announcement.GetComponent<TextMeshProUGUI>();
        CanvasGroup announcementCG = announcement.GetComponent<CanvasGroup>();

        messageText.text = message;
        
        // Animation
        float duration = 2f;
        // Fade in
        announcementCG.LeanAlpha(1, duration / 2)
            .setEaseOutSine();
        // Slide up a bit
        announcement.transform.LeanMoveLocalY(30, duration)
            .setEaseOutExpo()
            .setOnComplete
            // Fade out
            (() => announcementCG.LeanAlpha(0, duration / 2).setEaseInSine()
            .setOnComplete(() => OnGameAnnouncementComplete(announcement)));
    }

    /// <summary>
    /// Private helper that is evoked to handle completion of displaying an announcement
    /// </summary>
    private void OnGameAnnouncementComplete(GameObject announcement)
    {
        announcementsNum--;
        Destroy(announcement);
    }

    /// <summary>
    /// Sells a tower, called by tower stats panel sell button
    /// </summary>
    public void SellTower()
    {
        if (gridManager.DestroyTower(new Vector2Int((int) currentlySelectedTower.GetX(), (int)currentlySelectedTower.GetY()))){
            AudioManager.instance.PlaySoundFX(sellSound, transform, 1f);
            ChangeMoney(currentlySelectedTower.data.price);
            HideTowerStatsPanel();
        }
    }

    /// <summary>
    /// Hide tower stats panel
    /// </summary>
    public void HideTowerStatsPanel()
    {
        // Slide tower stats panel off screen
        towerStatsPanel.GetComponent<RectTransform>().LeanMoveX(-TOWER_STATS_PANEL_X_OFFSET, 0.1f).setEaseInCubic();

        // Stop outlining selected tower
        currentlySelectedTower.HideSelectionOutline();
    }

    /// <summary>
    /// Change player money (can be positive or negative)
    /// </summary>
    /// <param name="amt">Amount of money to change by</param>
    public void ChangeMoney(int amt)
    {
        money += amt;
        goblinCounter.text = money.ToString();
        
        // Create money change text
        GameObject moneyChangeText = Instantiate(moneyChangeTextPrefab, goblinCounter.gameObject.transform);
        if (amt >= 0)
        {
            moneyChangeText.GetComponent<TextMeshProUGUI>().color = Color.green;
            moneyChangeText.GetComponent<TextMeshProUGUI>().text = $"+{amt}";
        }
        else
        {
            moneyChangeText.GetComponent<TextMeshProUGUI>().color = Color.red;
            moneyChangeText.GetComponent<TextMeshProUGUI>().text = $"{amt}";
        }

        // Animate money change text
        CanvasGroup moneyChangeCG = moneyChangeText.GetComponent<CanvasGroup>();

        float duration = 0.5f;
        moneyChangeText.transform.localPosition = MONEY_CHANGE_ORIGINAL_POS;
        // Fade in
        moneyChangeCG.LeanAlpha(1, duration / 2)
            .setEaseOutSine();
        // Slide down
        moneyChangeText.transform.LeanMoveLocalY(MONEY_CHANGE_ORIGINAL_POS.y - 70, 0.5f)
            .setEaseOutExpo()
            .setOnComplete
            // Fade out
            (() => moneyChangeCG.LeanAlpha(0, duration / 2).setEaseInSine()
            .setOnComplete(() => Destroy(moneyChangeText)));
    }

    /// <summary>
    /// Change player health (can be positive or negative)
    /// </summary>
    /// <param name="amt">Amount of health to change by</param>
    public void ChangeHealth(int amt)
    {
        health += amt;
        healthCounter.text = health.ToString();

        // Check if player has lost
        if (health <= 0)
        {
            health = 0;
            healthCounter.text = health.ToString();
            gameManager.TriggerGameOver();
        }

        // Create health change text
        GameObject healthChangeText = Instantiate(healthChangeTextPrefab, healthCounter.gameObject.transform);
        healthChangeText.GetComponent<TextMeshProUGUI>().text = $"{amt}";

        // Animate health change text
        CanvasGroup healthChangeCG = healthChangeText.GetComponent<CanvasGroup>();

        float duration = 0.5f;
        healthChangeText.transform.localPosition = HEALTH_CHANGE_ORIGINAL_POS;
        // Fade in
        healthChangeCG.LeanAlpha(1, duration / 2)
            .setEaseOutSine();
        // Slide down
        healthChangeText.transform.LeanMoveLocalY(HEALTH_CHANGE_ORIGINAL_POS.y - 55, 0.5f)
            .setEaseOutExpo()
            .setOnComplete
            // Fade out
            (() => healthChangeCG.LeanAlpha(0, duration / 2).setEaseInSine()
            .setOnComplete(() => Destroy(healthChangeText)));
    }

    #region Basic Getters and Setters
    public int GetMoney()
    {
        return money;
    }
    public int GetPathPrice()
    {
        return pathPrice;
    }
    public void SetMoney(int amt)
    {
        money = amt;
    }

    public int GetHealth()
    {
        return money;
    }

    public void SetHealth(int amt)
    {
        health = amt;
    }
    #endregion
}
