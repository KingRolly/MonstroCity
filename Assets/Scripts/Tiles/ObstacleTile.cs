using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTile : Tile
{
    protected override void Start()
    {
        base.Start();
        setPlaceable(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Function used to clear the obstacle (in the future, we can use this to set
    // the sprite to a cleared sprite and subtract some amount of money or something
    // once we actually have obstacles we want to put in)
    public void clearObstacle() {
        setPlaceable(false);
    }
}
