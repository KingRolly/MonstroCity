using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine.UI;


// Class description:
// Used by Tower Icon prefab to implement functionality for purchasing towers and other UI features related to it (i.e display tower info popup as of now)
// - Nicholas Liang (Feb. 2nd, 2026)
public class TowerIcon : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI nameText;
    public GameObject icon;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private MouseManager mouseManager;
    [SerializeField] private TowerInfoPopup towerInfoPopup;
    [SerializeField] private TowerData data;

    // Start is called before the first frame update
    void Start()
    {
        priceText.text = data.price.ToString();
        nameText.text = data.towerName;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetData(TowerData data)
    {
        this.data = data;
        priceText.text = data.price.ToString();
        nameText.text = data.towerName;
        icon.GetComponent<UnityEngine.UI.Image>().sprite = data.sprite;
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
        if (mouseManager.GetSelectedTowerIcon() == null) // Check that player isn't already holding a tower
        { 
            if (uiManager.getMoney() - data.price >= 0) // check if player can afford tower
            {
                mouseManager.SetSelectedTowerIcon(this); // Set the held tower to this tower
            }
            else
            {
                Debug.Log("Not enough goblins!");
            }
        }
    }

    #region Basic Getters
    public TowerTile GetTowerType()
    {
        return data.towerType;
    }
    public TowerData GetTowerData()
    {
        return data;
    }
    public Sprite GetSprite()
    {
        return data.sprite;
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
    #endregion
}
