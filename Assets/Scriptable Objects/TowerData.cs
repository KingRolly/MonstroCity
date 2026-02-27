using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data container for a tower type's information. Used to assign general tower data <br/>
/// - Jack Peters (Feb. 27th, 2026)
/// </summary>

[CreateAssetMenu(fileName = "New TowerData", menuName = "TowerData")]
public class TowerData : ScriptableObject
{
    public TowerTile towerType;

    public string towerName;
    public Sprite sprite;
    public int price;
    public float damage;
    public float attackSpeed;
    public float attackRange;
}
