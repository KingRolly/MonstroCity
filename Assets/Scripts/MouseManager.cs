using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public GameObject indicator;
    public GameObject heldTower;
    Vector2 selectPos;
    static bool locked;
    [SerializeField] Sprite hover;
    [SerializeField] Sprite select;
    [SerializeField] Sprite hold;
    [SerializeField] GameObject rangePreview;
    [SerializeField] TowerIcon selectedTowerIcon = null;
    [SerializeField] GridManager gridManager;
    [SerializeField] UIManager uiManager;

    [Header("SFX")]
    [SerializeField] private AudioClip placePathSound;
    [SerializeField] private AudioClip removePathSound;
    private float announcementCooldown = 0;

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

            // Show preview for tower range
            rangePreview.transform.localScale = new Vector3(selectedTowerIcon.GetAtkRange() * 2, selectedTowerIcon.GetAtkRange() * 2, 1);
            // It's 2 times the attack range because the range is the radius of the circle, and scale sets the diameter
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
        if (gridManager.GetEditing()) 
        {

            // Check if player is placing a tower
            // 1. No selected tower - place path
            if (selectedTowerIcon == null)
            {
                HandlePathEditing();
            }
            // 2. There is a selected tower - place tower
            else
            {
                HandleTowerPlacement(worldPos);
            }
        }
        #endregion

        // Update selected pos
        selectPos = new Vector2Int((int)Mathf.Round(worldPos.x), (int)Mathf.Round(worldPos.y));
    }

    /// <summary>
    /// Private helper to handle path ediitng
    /// </summary>
    private void HandlePathEditing()
    {
        if (Input.GetMouseButton(0))
        { // Left click places
            HandlePlacePath();
        }
        else if (Input.GetMouseButton(1))
        { // Right click deletes
            HandlePathDeletion();
        }
    }

    /// <summary>
    /// Private helper to handle attempting to place a path
    /// </summary>
    private void HandlePlacePath()
    {
        Vector2Int placePosition = new Vector2Int((int)selectPos.x, (int)selectPos.y);

        // Check position is placeable
        if (gridManager.IsPosPlaceable(placePosition))
        {
            // Check player has enough money                        
            if (uiManager.GetMoney() >= uiManager.GetPathPrice())
            {
                gridManager.PlacePath(placePosition);
                AudioManager.instance.PlaySoundFX(placePathSound, transform, 1f);
                uiManager.ChangeMoney(-uiManager.GetPathPrice());
            }
            // Can't afford path
            else if (uiManager.GetMoney() < uiManager.GetPathPrice())
            {
                if (announcementCooldown == 0) // Check announcement cooldown to prevent mouse manager spamming game announcements
                {
                    LeanTween.value(gameObject, 1, 0, 1).setOnUpdate((float value) => announcementCooldown = value); // Intiative cooldown
                    uiManager.DisplayGameAnnouncement("Not enough goblins!", 2);
                }
            }
        }
    }

    /// <summary>
    /// Private helper to handle path deletion
    /// </summary>
    private void HandlePathDeletion()
    {
        if (gridManager.DeletePath(new Vector2Int((int)selectPos.x, (int)selectPos.y)))
        {
            AudioManager.instance.PlaySoundFX(removePathSound, transform, 0.3f);
            uiManager.ChangeMoney(uiManager.GetPathPrice());
        }
    }

    /// <summary>
    /// Private helper to handle tower placement
    /// </summary>
    private void HandleTowerPlacement(Vector2Int worldPos)
    {
        if (Input.GetMouseButtonDown(0) && gridManager.IsInBounds(worldPos))
        {
            if (gridManager.PlaceTower(Vector2Int.RoundToInt(selectPos), selectedTowerIcon.GetTowerData()))
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

    public bool GetLock() {
        return locked;
    }

    public Vector2 GetPos() {
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