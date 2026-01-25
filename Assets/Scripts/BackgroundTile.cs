using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : Tile
{
    protected override void Start()
    {
        base.Start();
        setPlaceable(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}