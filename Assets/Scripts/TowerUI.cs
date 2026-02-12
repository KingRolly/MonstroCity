using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// Class description:
// Used by Tower Icon prefab to implement functionality for purchasing towers and other UI features related to it (i.e display tower info popup as of now)
// - Nicholas Liang (Feb. 2nd, 2026)
public class TowerUI : MonoBehaviour
{
    // TODO: See if this needs to be static
    // Previously was static and seemingly was the cause of the purchase price bug
    // Bug was fixed when static was removed because holding being static caused every tower UI to share holding
    // So when trying to place a tower, every tower UI thought it was being placed, tower highest up in hierachy kept being placed
    [SerializeField] public string holding = "None";

    [Header("References")]
    public TextMeshProUGUI priceText;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private MouseManager mouseManager;
    [SerializeField] private TowerInfoPopup towerInfoPopup;

    [Header("Tower Stats")]
    [SerializeField] private string towerType;
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
        if (Input.GetKeyUp(KeyCode.H))
        {
            Debug.Log("Holding " + holding);
        }
        if (holding != "None")
        {
            if (Input.GetMouseButtonDown(0))
            {
                // TODO: Resolve an error originates here when calling placeTower() on line 57
                // Error occurs on line 109 of GridManager.cs, apparently it's an out of bounds error or smt

                // Placeholder for tower placement testing until we get the above error fixed
                Debug.Log($"{towerType} purchased for {price} goblins");
                uiManager.addMoney(-this.price);
                holding = "None";

                if (gridManager.placeTower(Vector2Int.RoundToInt(mouseManager.getPos()), towerType))
                {
                    //Debug.Log($"{towerType} purchased for {price} goblins");
                    //uiManager.addMoney(-this.price);
                    //holding = "None";
                }
            }
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                holding = "None";
            }
            mouseManager.setLock(false);
        }
    }

    // Assigns info of tower upon instantiation
    // UIManager should call this when setting up the top bar
    public void setTowerInfo(string type, int price, float dmg, float atkSpd, float atkRg)
    {
        this.towerType = type;
        this.price = price;
        priceText.text = price.ToString() + " Goblins";
        this.damage = dmg;
        this.attackSpeed = atkSpd;
        this.attackRange = atkRg;
    }

    // Used to assign references TowerUI needs upon instantiation
    // UIManager should call this when setting up the top bar
    public void assignReferences(UIManager uiManager, GridManager gridManager, MouseManager mouseManager, TowerInfoPopup towerInfoPopup)
    {
        this.uiManager = uiManager;
        this.gridManager = gridManager;
        this.mouseManager = mouseManager;
        this.towerInfoPopup = towerInfoPopup;
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
        if (holding.Equals("None")) // Check that player isn't already holding a tower
        { 
            if (uiManager.getMoney() - this.price >= 0) // check if player can afford tower
            {
                holding = towerType; // Set the held tower to this tower's type
            }
            else
            {
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
