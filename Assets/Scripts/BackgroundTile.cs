using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : Tile
{   

    [SerializeField] Sprite grass;
    [SerializeField] Sprite brick;
    [SerializeField] Sprite stone;

    [SerializeField] Sprite dirt;
    protected override void Start()
    {
        base.Start();
        setPlaceable(true);
    }

    // Update is called once per frame
    // void Update()
    // {

    // }

    public enum TextureType
    {
        Grass, 
        Brick, 
        Stone, 
        Dirt
    }

    public void setTileTexture(TextureType type)
    {
        switch (type){
            case TextureType.Grass:
                gameObject.GetComponent<SpriteRenderer>().sprite = grass;
                return;
            case TextureType.Brick:
                gameObject.GetComponent<SpriteRenderer>().sprite = brick;
                return;
            case TextureType.Stone:
                gameObject.GetComponent<SpriteRenderer>().sprite = stone;
                return;
            
            default: 
                gameObject.GetComponent<SpriteRenderer>().sprite = dirt;
                return;

           
        }
    }
}