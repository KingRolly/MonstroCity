using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Class description:
// UI element that displays information of a tower
// - Nicholas Liang (Feb. 2nd, 2026)
public class TowerInfoPopup : MonoBehaviour
{
    [Header("TMP Text")]
    [SerializeField] private TextMeshProUGUI dmgText;
    [SerializeField] private TextMeshProUGUI atkSpeedText;
    [SerializeField] private TextMeshProUGUI atkRangeText;

    [Header("Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRange;

    // private readonly float Y_POSITION = 600;
    private readonly float Y_OFFSET = 230;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Updates popup with information of currently hovered tower and makes popup visible
    // Provide stats to be updated (TODO: method might need to support more stats in the future)
    public void DisplayPopup(float dmg, float spd, float range, float xPos, float yPos)
    {
        dmgText.text = "DMG " + dmg.ToString();
        atkSpeedText.text = "SPD " + spd.ToString() + "s";
        atkRangeText.text = "Range " + range.ToString();
        Vector2 newPos = new Vector2(xPos, yPos - Y_OFFSET);
        gameObject.transform.position = newPos;
        gameObject.SetActive(true);
    }

    // Hides popups (should occur upon exiting hover)
    public void HidePopup()
    {
        gameObject.SetActive(false);
    }

    // Basic getters for private variables
    public float GetDmg()
    {
        return this.damage;
    }
    public float GetAtkSpeed()
    {
        return this.attackSpeed;
    }

    public float GetAtkRange()
    {
        return this.attackRange;
    }
}
