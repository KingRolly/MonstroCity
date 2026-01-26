using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TowerUI : MonoBehaviour
{
    public TextMeshProUGUI priceText;
    public MoneyManager moneyManager;

    [Header("Tower Stats")]
    public string towerName = "Archer";
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
    public void setTowerInfo(string name, int price, float dmg, float atkSpd, float atkRg)
    {
        this.towerName = name;
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
        Debug.Log("display " + towerName + "'s tower info popup"); // stub
    }

    // Purchase tower and prompt player to place tower
    public void buyTower()
    {
        // TODO: Intiate tower placement
        moneyManager.addMoney(-price);
        Debug.Log("purchased " + towerName); // stub
    }
}
