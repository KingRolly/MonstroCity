using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public class PathTile : Tile
{
    private float enemySpeed;
    private float enemyDamage;
    [SerializeField] Sprite verticalPath;
    [SerializeField] Sprite horizontalPath;
    [SerializeField] Sprite upperEndPath;
    [SerializeField] Sprite lowerEndPath;
    [SerializeField] Sprite rightEndPath; //flip for left
    [SerializeField] Sprite ceilingTurnPath; //rotate this when necessary
    [SerializeField] Sprite floorTurnPath; //rotate this when necessary

    public enum spriteType //add the flipped types into here and handle that in the sprite type function
    {
        Vertical,
        Horizontal,
        UpperEnd,
        LowerEnd,
        RightEnd,
        CeilingTurn,
        FloorTurn
    }

    protected override void Start()
    {
        base.Start();
        setPlaceable(false);
        enemySpeed = 0;
        enemyDamage = 0;
    }

    public void setSpriteType(spriteType type)
    {
        switch (type){
            case spriteType.Vertical:
                gameObject.GetComponent<SpriteRenderer>().sprite = verticalPath;
                return;
            case spriteType.Horizontal:
                gameObject.GetComponent<SpriteRenderer>().sprite = horizontalPath;
                return;
            case spriteType.UpperEnd:
                gameObject.GetComponent<SpriteRenderer>().sprite = upperEndPath;
                return;
            case spriteType.LowerEnd:
                gameObject.GetComponent<SpriteRenderer>().sprite = lowerEndPath;
                return;
            case spriteType.RightEnd:
                gameObject.GetComponent<SpriteRenderer>().sprite = rightEndPath;
                return;
            case spriteType.CeilingTurn:
                gameObject.GetComponent<SpriteRenderer>().sprite = ceilingTurnPath;
                return;
            case spriteType.FloorTurn:
                gameObject.GetComponent<SpriteRenderer>().sprite = floorTurnPath;
                return;
        }
    }
}
