using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// Class description:
// Used by Tower Icon prefab to implement functionality for purchasing towers and other UI features related to it (i.e display tower info popup as of now)
// - Nicholas Liang (Feb. 2nd, 2026)
public class TowerUI : MonoBehaviour
{
    public static string holding = "None";

    [Header("References")]
    public TextMeshProUGUI priceText;
    public MoneyManager moneyManager;
    public GridManager gridManager;
    public MouseManager mouseManager;
    public TowerInfoPopup towerInfoPopup;

    [Header("Tower Stats")]
    [SerializeField] private string towerType = "Archer";
    [SerializeField] private int price;
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRange;

    // Start is called before the first frame update
    void Start()
    {
        priceText.text = price.ToString() + " Goblins";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H)) {
            Debug.Log("Holding " + holding);
        }
        if (holding != "None") {
            if (Input.GetMouseButtonDown(0)) {
                if (gridManager.placeTower(Vector2Int.RoundToInt(mouseManager.getPos()), towerType)) {
                    moneyManager.addMoney(-price);
                    holding = "None";
                }
            }
            if (Input.GetKeyUp(KeyCode.Escape)) {
                holding = "None";
            }
            mouseManager.setLock(false);
        }
    }

    // Assigns info of tower
    public void setTowerInfo(string type, int price, float dmg, float atkSpd, float atkRg)
    {
        this.towerType = name;
        this.price = price;
        priceText.text = price.ToString() + " Goblins";
        this.damage = dmg;
        this.attackSpeed = atkSpd;
        this.attackRange = atkRg;
    }

    // Make info popup visible, should be called upon hovering over icon
    public void displayTowerInfoPopup()
    {
        // Enable tower info popup and update its stats to reflect currently hovered tower
        towerInfoPopup.displayPopup(this.damage, this.attackSpeed, this.attackRange, gameObject.transform.position.x, gameObject.transform.position.y);
        // Debug.Log("Diplay " + towerType + " Tower's info popup");
    }

    // Hide info popup, should be called upon exiting hover over icon
    public void hideTowerInfoPopup()
    {
        towerInfoPopup.hidePopup();
    }

    // Purchase tower and prompt player to place tower
    public void buyTower()
    {
        if (holding.Equals("None")) { // Check that player isn't already holding a tower
            if (moneyManager.getMoney() >= price) {
                holding = towerType; // Set the held tower to this tower's type
            } else {
                Debug.Log("Not enough goblins!");
            }
        }
    }

    // Basic getters for private variables
    public string getTowerType()
    {
        return this.towerType;
    }
    public int getPrice()
    {
        return this.price;
    }
    public float getDmg()
    {
        return this.damage;
    }
    public float getAtkSpeed()
    {
        return this.attackSpeed;
    }

    public float getAtkRange()
    {
        return this.attackRange;
    }
}
