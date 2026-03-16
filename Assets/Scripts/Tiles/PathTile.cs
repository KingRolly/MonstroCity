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
    [SerializeField] Sprite horizontal;
    [SerializeField] Sprite vertical;
    [SerializeField] Sprite downLeft;
    [SerializeField] Sprite downRight;
    [SerializeField] Sprite upLeft;
    [SerializeField] Sprite upRight;
    [SerializeField] Sprite downEnd;
    [SerializeField] Sprite upEnd;
    [SerializeField] Sprite leftEnd;
    [SerializeField] Sprite rightEnd;
    [SerializeField] Sprite start;
    [SerializeField] Sprite end;

    public enum spriteType //add the flipped types into here and handle that in the sprite type function
    {
        Horizontal,
        Vertical,
        DownLeft,
        DownRight,
        UpLeft,
        UpRight,
        DownEnd,
        UpEnd,
        LeftEnd,
        RightEnd,
        Start,
        End
    }

    private void Awake()
    {
        SetSpriteType(spriteType.Vertical);
    }

    protected override void Start()
    {
        base.Start();
        SetPlaceable(false);
        enemySpeed = 0;
        enemyDamage = 0;
    }

    public override void SetSpriteType(spriteType type)
    {
        switch (type){
            case spriteType.Horizontal:
                gameObject.GetComponent<SpriteRenderer>().sprite = horizontal;
                return;
            case spriteType.Vertical:
                gameObject.GetComponent<SpriteRenderer>().sprite = vertical;
                return;
            case spriteType.DownLeft:
                gameObject.GetComponent<SpriteRenderer>().sprite = downLeft;
                return;
            case spriteType.DownRight:
                gameObject.GetComponent<SpriteRenderer>().sprite = downRight;
                return;
            case spriteType.UpLeft:
                gameObject.GetComponent<SpriteRenderer>().sprite = upLeft;
                return;
            case spriteType.UpRight:
                gameObject.GetComponent<SpriteRenderer>().sprite = upRight;
                return;
            case spriteType.DownEnd:
                gameObject.GetComponent<SpriteRenderer>().sprite = downEnd;
                return;
            case spriteType.UpEnd:
                gameObject.GetComponent<SpriteRenderer>().sprite = upEnd;
                return;
            case spriteType.LeftEnd:
                gameObject.GetComponent<SpriteRenderer>().sprite = leftEnd;
                return;
            case spriteType.RightEnd:
                gameObject.GetComponent<SpriteRenderer>().sprite = rightEnd;
                return;
            case spriteType.Start:
                gameObject.GetComponent<SpriteRenderer>().sprite = start;
                return;
            case spriteType.End:
                gameObject.GetComponent<SpriteRenderer>().sprite = end;
                return;
        }
    }
}
