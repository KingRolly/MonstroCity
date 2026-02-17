using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

// Class description:
// Manager for setting up and changing anything UI related
// Note: Used to be MoneyManager, but decided a general purpose UIManager should oversee all UI management
// - Nicholas Liang (Feb. 9th, 2026)
public class UIManager : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI goblinCounter;
    public GameObject topBar;
    public GameObject towersPanel;
    public GameObject towerIconPrefab;
    public UIManager uiManager;
    public GridManager gridManager;
    public MouseManager mouseManager;
    public TowerInfoPopup towerInfoPopup;

    [Header("UI Information")]
    [SerializeField] private int money;
    private List<GameObject> towersList;

    // Start is called before the first frame update
    void Start()
    {
        goblinCounter.text = money.ToString();
        setupTopBarTowersUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method for setting up all the purchasable tower UI in the top bar
    // Should be called upon start for setup
    private void setupTopBarTowersUI()
    {
        // Setup top bar by instantiating TowerUIs, setting their stats, and assign references they need
        // Initialize towersList and add instantiated towerUIs to towersList to keep track of them for later use
        towersList = new List<GameObject>();

        // Placeholder towers for now
        // Orc: 10 price, 1 dmg, 0.8s spd, 10 range
        GameObject orc = Instantiate(towerIconPrefab, towersPanel.transform);
        orc.GetComponent<TowerUI>().setTowerInfo("Orc", 10, 1, 0.8f, 10);
        orc.GetComponent<TowerUI>().assignReferences(uiManager, gridManager, mouseManager, towerInfoPopup);
        towersList.Add(orc);

        // Slime: 15 price, 10 dmg, 2s spd, 25 range
        GameObject slime = Instantiate(towerIconPrefab, towersPanel.transform);
        slime.GetComponent<TowerUI>().setTowerInfo("Slime", 15, 10, 2f, 25);
        slime.GetComponent<TowerUI>().assignReferences(uiManager, gridManager, mouseManager, towerInfoPopup);
        towersList.Add(slime);

        // Gnome Shooter: 8 price, 4 dmg, 0.5s spd, 5 range
        GameObject gnomeShooter = Instantiate(towerIconPrefab, towersPanel.transform);
        gnomeShooter.GetComponent<TowerUI>().setTowerInfo("Gnome Shooter", 8, 4, 0.5f, 5);
        gnomeShooter.GetComponent<TowerUI>().assignReferences(uiManager, gridManager, mouseManager, towerInfoPopup);
        towersList.Add(gnomeShooter);

        // Orc: 10 price, 1 dmg, 0.8s spd, 10 range
        GameObject archer = Instantiate(towerIconPrefab, towersPanel.transform);
        archer.GetComponent<TowerUI>().setTowerInfo("Archer", 2, 2, 0.1f, 8);
        archer.GetComponent<TowerUI>().assignReferences(uiManager, gridManager, mouseManager, towerInfoPopup);
        towersList.Add(archer);

        // Slime: 15 price, 10 dmg, 2s spd, 25 range
        GameObject dragon = Instantiate(towerIconPrefab, towersPanel.transform);
        dragon.GetComponent<TowerUI>().setTowerInfo("Dragon", 50, 25, 1f, 15);
        dragon.GetComponent<TowerUI>().assignReferences(uiManager, gridManager, mouseManager, towerInfoPopup);
        towersList.Add(dragon);

        // Gnome Shooter: 8 price, 4 dmg, 0.5s spd, 5 range
        GameObject golem = Instantiate(towerIconPrefab, towersPanel.transform);
        golem.GetComponent<TowerUI>().setTowerInfo("Golem", 20, 50, 3f, 2);
        golem.GetComponent<TowerUI>().assignReferences(uiManager, gridManager, mouseManager, towerInfoPopup);
        towersList.Add(golem);

        // Slime: 15 price, 10 dmg, 2s spd, 25 range
        GameObject cannon = Instantiate(towerIconPrefab, towersPanel.transform);
        cannon.GetComponent<TowerUI>().setTowerInfo("Cannon", 100, 20, 3f, 50);
        cannon.GetComponent<TowerUI>().assignReferences(uiManager, gridManager, mouseManager, towerInfoPopup);
        towersList.Add(slime);

        // Gnome Shooter: 8 price, 4 dmg, 0.5s spd, 5 range
        GameObject hwacha = Instantiate(towerIconPrefab, towersPanel.transform);
        hwacha.GetComponent<TowerUI>().setTowerInfo("Hwacha", 50, 4, 0.1f, 20);
        hwacha.GetComponent<TowerUI>().assignReferences(uiManager, gridManager, mouseManager, towerInfoPopup);
        towersList.Add(hwacha);

    }

    public int getMoney() {
        return money;
    }

    // set current money to amt
    public void setMoney(int amt)
    {
        money = amt;
        goblinCounter.text = money.ToString();
    }

    // add money by amt (can be positive or negative)
    public void addMoney(int amt)
    {
        money += amt;
        goblinCounter.text = money.ToString();
    }
}
