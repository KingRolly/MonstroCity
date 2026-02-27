using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

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
    [SerializeField] private TowerData archerData;

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
        // Archer: 2 price, 1 dmg, 0.1s spd, 8 range
        GameObject archer = Instantiate(towerIconPrefab, towersPanel.transform);
        archer.GetComponent<TowerIcon>().SetData(archerData);
        archer.GetComponent<TowerIcon>().AssignReferences(uiManager, gridManager, mouseManager, towerInfoPopup);
        towersList.Add(archer);

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

    public Boolean isHolding(){
        foreach (GameObject obj in towersList)
        {
            if (obj.GetComponent<TowerIcon>().holding != "None") {
                return true;
            }
        }
        return false;
    }
}
