using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerUI : MonoBehaviour
{
    public TextMeshProUGUI priceText;
    public MoneyManager moneyManager;
    public GridManager gridManager;
    public MouseManager mouseManager;

    [Header("Tower Stats")]
    public string towerType = "Archer";
    public int price;
    public float damage;
    public float attackSpeed;
    public float attackRange;

    // Start is called before the first frame update
    void Start()
    {
        priceText.text = price.ToString() + " Goblins";
    }

    // Update is called once per frame
    void Update()
    {
        
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
        // TODO: Enable tower info popup and update its stats to reflect currently hovered tower
        Debug.Log("Diplay " + towerType + " Tower's info popup"); // stub
    }

    // Purchase tower and prompt player to place tower
    public void buyTower()
    {
        if (mouseManager.getLock()) {
            if (moneyManager.getMoney() >= price) {
                Debug.Log("Placing...");
                if (gridManager.placeTower(Vector2Int.RoundToInt(mouseManager.getPos()), towerType)) {
                    moneyManager.addMoney(-price);
                    Debug.Log("Placed an " + towerType);
                }
            } else {
                // TODO: Show that the tower can't be placed in some way
                Debug.Log("Not enough goblins!!!");
            }
        }
    }
}
