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
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject icon;
    [SerializeField] private GameObject goblinIcon;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private MouseManager mouseManager;
    [SerializeField] private TowerInfoPopup towerInfoPopup;
    [SerializeField] private TowerData data;

    private bool isEmpty;
    private readonly Color EMPTY_ICON_COLOUR = new Color32(150, 150, 150, 200);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// Assign TowerData to tower and sets its information
    /// </summary>
    /// <param name="data">Scriptable object that provides tower data for tower icon</param>
    public void SetData(TowerData data)
    {
        this.data = data;
        this.isEmpty = false;
        priceText.text = data.price.ToString();
        nameText.text = data.towerName;
        icon.GetComponent<UnityEngine.UI.Image>().sprite = data.sprite;
    }

    /// <summary>
    /// Make this tower icon an empty tower
    /// </summary>
    public void MakeEmpty()
    {
        // Remove all images and text (tower name is empty for debugging purposes for now)
        this.data = null;
        this.isEmpty = true;
        priceText.text = "";
        nameText.text = "Empty";
        icon.SetActive(false);
        goblinIcon.SetActive(false);
        // Removes button functionality from this tower icon, then greys it out
        Destroy(gameObject.GetComponent<Button>());
        gameObject.GetComponent<UnityEngine.UI.Image>().color = EMPTY_ICON_COLOUR;
    }

    /// <summary>
    /// Used to assign references TowerUI needs upon instantiation <br/> 
    /// UIManager should call this when setting up the top bar
    /// </summary>
    /// <param name="uiManager"></param>
    /// <param name="gridManager"></param>
    /// <param name="mouseManager"></param>
    /// <param name="towerInfoPopup"></param>
    public void AssignReferences(UIManager uiManager, GridManager gridManager, MouseManager mouseManager, TowerInfoPopup towerInfoPopup)
    {
        this.uiManager = uiManager;
        this.gridManager = gridManager;
        this.mouseManager = mouseManager;
        this.towerInfoPopup = towerInfoPopup;
    }

    /// <summary>
    /// Make info popup visible, should be called upon hovering over icon
    /// </summary>
    public void DisplayTowerInfoPopup()
    {
        // Enable tower info popup and update its stats to reflect currently hovered tower
        if (!isEmpty) // Check this isn't an empty tower icon
        {
            towerInfoPopup.DisplayPopup(data.damage, data.attackSpeed, data.attackRange, gameObject.transform.position.x, gameObject.transform.position.y);
        }
        //Debug.Log("Diplay " + data.towerName + " Tower's info popup");
    }

    /// <summary>
    /// Hide info popup, should be called upon exiting hover over icon
    /// </summary>
    public void HideTowerInfoPopup()
    {
        towerInfoPopup.HidePopup();
    }

    /// <summary>
    /// Purchase tower and prompt player to place tower
    /// </summary>
    public void BuyTower()
    {
        if (mouseManager.GetSelectedTowerIcon() != this && !isEmpty) // Check that player isn't already holding this tower and tower isn't empty
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
