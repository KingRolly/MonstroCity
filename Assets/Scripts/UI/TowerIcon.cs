using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


// Class description:
// Used by Tower Icon prefab to implement functionality for purchasing towers and other UI features related to it (i.e display tower info popup as of now)
// - Nicholas Liang (Feb. 2nd, 2026)
public class TowerIcon : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI nameText;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private MouseManager mouseManager;
    [SerializeField] private TowerInfoPopup towerInfoPopup;
    [SerializeField] private TowerData data;
    [SerializeField] private TowerData holding = null;

    // Start is called before the first frame update
    void Start()
    {
        priceText.text = data.price.ToString();
        nameText.text = data.towerName;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
        {
            Debug.Log("Holding " + holding.towerName);
        }
        if (holding != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (gridManager.placeTower(Vector2Int.RoundToInt(mouseManager.getPos()), data))
                {
                    Debug.Log($"{data.towerName} purchased for {data.price} goblins");
                    uiManager.addMoney(-data.price);
                    holding = null;
                }
            }
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                holding = null;
            }
            mouseManager.setLock(false);
        }
    }

    public void SetData(TowerData data)
    {
        this.data = data;
        priceText.text = data.price.ToString();
        nameText.text = data.towerName;
    }

    // Used to assign references TowerUI needs upon instantiation
    // UIManager should call this when setting up the top bar
    public void AssignReferences(UIManager uiManager, GridManager gridManager, MouseManager mouseManager, TowerInfoPopup towerInfoPopup)
    {
        this.uiManager = uiManager;
        this.gridManager = gridManager;
        this.mouseManager = mouseManager;
        this.towerInfoPopup = towerInfoPopup;
    }

    // Make info popup visible, should be called upon hovering over icon
    public void DisplayTowerInfoPopup()
    {
        // Enable tower info popup and update its stats to reflect currently hovered tower
        towerInfoPopup.DisplayPopup(data.damage, data.attackSpeed, data.attackRange, gameObject.transform.position.x, gameObject.transform.position.y);
        //Debug.Log("Diplay " + data.towerName + " Tower's info popup");
    }

    // Hide info popup, should be called upon exiting hover over icon
    public void HideTowerInfoPopup()
    {
        towerInfoPopup.HidePopup();
    }

    // Purchase tower and prompt player to place tower
    public void BuyTower()
    {
        if (holding == null) // Check that player isn't already holding a tower
        { 
            if (uiManager.getMoney() - data.price >= 0) // check if player can afford tower
            {
                holding = data; // Set the held tower to this tower's data
            }
            else
            {
                Debug.Log("Not enough goblins!");
            }
        }
    }

    // Basic getters for private variables
    public TowerTile GetTowerType()
    {
        return data.towerType;
    }
    public string GetTowerName()
    {
        return data.towerName;
    }
    public int GetPrice()
    {
        return data.price;
    }
    public float GetDmg()
    {
        return data.damage;
    }
    public float GetAtkSpeed()
    {
        return data.attackSpeed;
    }

    public float GetAtkRange()
    {
        return data.attackRange;
    }

    public TowerData GetHolding()
    {
        return holding;
    }
}
