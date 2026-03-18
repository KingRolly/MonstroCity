using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
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
    [SerializeField] private TextMeshProUGUI goblinCounter;
    [SerializeField] private TextMeshProUGUI healthCounter;
    [SerializeField] private GameObject topBar;
    [SerializeField] private GameObject towersPanel;
    [SerializeField] private GameObject towerIconPrefab;
    [SerializeField] private TowerInfoPopup towerInfoPopup;

    [Header("Tower Stats Panel References")]
    [SerializeField] private GameObject towerStatsPanel;
    [SerializeField] private Image towerArt;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI dmgText;
    [SerializeField] private TextMeshProUGUI atkSpdText;
    [SerializeField] private TextMeshProUGUI rangeText;

    [Header("UI Information")]
    [SerializeField] private int money;
    [SerializeField] private int health;
    [SerializeField] private TowerData archerData;
    [SerializeField] private TowerData gnomeData;
    private List<GameObject> towersList;

    [Header("Constants")]
    private readonly int TOWER_PANEL_Y_OFFSET = 225;
    private readonly int TOWER_STATS_PANEL_X_OFFSET = 260;
    private readonly int MAX_TOWER_ICONS = 8;

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
        float duration = 0.5f;
        towersPanel.transform.LeanMoveLocalY(TOWER_PANEL_Y_OFFSET, duration).setEaseInCubic();
        towerInfoPopup.enabled = false;
    }

    /// <summary>
    /// Show the tower panel
    /// </summary>
    public void ShowTowerPanel()
    {
        float duration = 0.5f;
        towersPanel.transform.LeanMoveLocalY(0, duration).setEaseOutCubic();
        towerInfoPopup.enabled = true;
    }

    /// <summary>
    /// Display the stats of given tower
    /// </summary>
    public void DisplayTowerStatsPanel(TowerTile tower)
    {
        // Set stats
        towerArt.sprite = tower.data.sprite;
        towerName.text = tower.data.towerName;
        dmgText.text = "DMG " + tower.data.damage.ToString();
        atkSpdText.text = "SPD " + tower.data.attackSpeed.ToString() + "s";
        rangeText.text = "Range " + tower.data.attackRange.ToString();

        // Slide tower stats panel out
        towerStatsPanel.GetComponent<RectTransform>().LeanMoveX(0, 0.1f).setEaseInCubic();
    }

    /// <summary>
    /// Hide tower stats panel
    /// </summary>
    public void HideTowerStatsPanel()
    {
        // Slide tower stats panel off screen
        towerStatsPanel.GetComponent<RectTransform>().LeanMoveX(-TOWER_STATS_PANEL_X_OFFSET, 0.1f).setEaseOutCubic();
    }

    /// <summary>
    /// Change player money (can be positive or negative)
    /// </summary>
    /// <param name="amt">Amount of money to change by</param>
    public void ChangeMoney(int amt)
    {
        money += amt;
        goblinCounter.text = money.ToString();
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
    }

    #region Basic Getters and Setters
    public int GetMoney()
    {
        return money;
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
