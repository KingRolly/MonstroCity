using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class MouseManager : MonoBehaviour
{
    public GameObject indicator;
    public GameObject heldTower;
    Vector2 selectPos;
    static bool locked;
    [SerializeField] Sprite hover;
    [SerializeField] Sprite select;
    [SerializeField] Sprite hold;
    [SerializeField] TowerIcon selectedTowerIcon = null;
    [SerializeField] GridManager gridManager;
    [SerializeField] UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 screenPos = Input.mousePosition;
        Vector2Int worldPos = Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(screenPos));

        #region Updates indicator sprite
        // Indicator is locked
        if (locked) { 
            gameObject.GetComponent<SpriteRenderer>().sprite = select;
        }
        // Tower has been purchased and being held
        else if (selectedTowerIcon != null) { 
            selectPos = new Vector2(Mathf.Round(worldPos.x), Mathf.Round(worldPos.y));
            gameObject.GetComponent<SpriteRenderer>().sprite = hold;
        }
        // Default state
        else
        { 
            selectPos = new Vector2(Mathf.Round(worldPos.x), Mathf.Round(worldPos.y));
            gameObject.GetComponent<SpriteRenderer>().sprite = hover;
        }

        // Set sprite of child object representing held tower
        SpriteRenderer heldTowerRenderer = heldTower.GetComponent<SpriteRenderer>();
        if (selectedTowerIcon == null)
        {
            heldTower.SetActive(false);
        } else
        {
            heldTower.SetActive(true);
            heldTowerRenderer = heldTower.GetComponent<SpriteRenderer>();
            heldTowerRenderer.sprite = selectedTowerIcon.GetSprite();
            heldTowerRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        }
        #endregion

        // Keep indicator within bounds
        if (selectPos.x > 15) selectPos.x = 15;
        if (selectPos.x < 0) selectPos.x = 0;
        if (selectPos.y > 8) selectPos.y = 8;
        if (selectPos.y < 0) selectPos.y = 0;
        
        indicator.transform.position = selectPos;


        #region Attempt to place something
        // Check that player is in editing mode
        if (gridManager.getEditing()) 
        {

            // Check if player is placing a tower
            // 1. No selected tower - place path
            if (selectedTowerIcon == null)
            {
                if (Input.GetMouseButton(0))
                { // Left click places
                    gridManager.placePath(new Vector2Int((int)selectPos.x, (int)selectPos.y));
                }
                else if (Input.GetMouseButton(1))
                { // Right click deletes
                    gridManager.deletePath(new Vector2Int((int)selectPos.x, (int)selectPos.y));
                }
            }

            // 2. There is a selected tower - place tower
            else
            {
                if (Input.GetMouseButtonDown(0) && gridManager.IsInBounds(worldPos))
                {
                    if (gridManager.placeTower(Vector2Int.RoundToInt(selectPos), selectedTowerIcon.GetTowerData()))
                    { // Call GridManager to place the tower, update money, unselect tower icon
                        Debug.Log($"{selectedTowerIcon.GetTowerName()} purchased for {selectedTowerIcon.GetPrice()} goblins");
                        uiManager.ChangeMoney(-selectedTowerIcon.GetPrice());
                        if (uiManager.GetMoney() < selectedTowerIcon.GetPrice())
                        {
                            selectedTowerIcon = null;
                        }
                    }
                }

                if (Input.GetKeyUp(KeyCode.Escape))
                { // Cancel placing of tower
                    selectedTowerIcon = null;
                }
                locked = false;
            }
        }
        #endregion


        // Click while hovering to "lock" or "unlock" position
        if (Input.GetMouseButtonDown(0) && !gridManager.getEditing() && gridManager.IsInBounds(worldPos))
        {
            if (selectPos == new Vector2(Mathf.Round(worldPos.x), Mathf.Round(worldPos.y)))
            {
                setLock(!locked);
            }
            else
            {
                selectPos = new Vector2Int((int)Mathf.Round(worldPos.x), (int)Mathf.Round(worldPos.y));
            }
            //Debug.Log(selectPos);
        }
    }

    public void setLock(bool val)
    {
        locked = val;
    }

    public bool getLock() {
        return locked;
    }

    public Vector2 getPos() {
        return selectPos;
    }

    /// <summary>
    /// Assign the TowerIcon that's been selected
    /// </summary>
    /// <param name="towerIcon">TowerIcon to be assigned</param>
    public void SetSelectedTowerIcon(TowerIcon towerIcon)
    {
        selectedTowerIcon = towerIcon;
    }

    /// <summary>
    /// Get TowerIcon that's currently being selected
    /// </summary>
    public TowerIcon GetSelectedTowerIcon()
    {
        return selectedTowerIcon;
    }
}