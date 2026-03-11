using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private float posX;
    private float posY;
    private bool placeable;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        posX = transform.position.x;
        posY = transform.position.y;
        placeable = false; //default to false
    }

    // Called from GridManager to preset certain information?
    public void Initialize()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Pressing "I" prints tile info when hovering
    void OnMouseOver()
    {
        if (Input.GetKeyUp(KeyCode.I)) {
            Debug.Log("[" + posX + ", " + posY + "]");
        }
    }

    public float GetX()
    {
        return posX;
    }

    public float GetY()
    {
        return posY;
    }

    public bool GetPlaceable()
    {
        return placeable;
    }

    public void SetPlaceable(bool val)
    {
        placeable = val;
    }

    public virtual void SetSpriteType(PathTile.spriteType sprite) {
        //this is kind of a hack job but it's okay probably
    }
}