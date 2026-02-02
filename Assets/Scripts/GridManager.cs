using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    public static int width = 16;
    public static int height = 9;
    public static Tile[,] grid = new Tile[width, height];
    public static List<Vector2Int> path = new List<Vector2Int>();
    public static List<Vector2Int> placeablePositions = new List<Vector2Int>();
    public static List<GameObject> placeableIndicators = new List<GameObject>();
    public GameObject placeableIndicator;
    public BackgroundTile backgroundTile;
    public PathTile pathTile;
    public TowerTile towerTile;
    public string editing = "None";
    public MouseManager mouseManager;

    // Start is called before the first frame update
    void Start()
    {
        placeTiles(width, height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void placeTiles(int width, int height)
    {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                // Creates prefab at correct pos and adds to grid Array
                grid[i, j] = Instantiate(backgroundTile, new Vector2(i, j), Quaternion.identity);
                // Initializes tile info
                //Tile tileData = grid[i, j].GetComponent<Tile>();
                //tileData.Initialize("something");
            }
        }

        //Set (0, 0) to be the initial placeable position
        placeablePositions.Add(new Vector2Int(0, 0));
        placeableIndicators.Add(Instantiate(placeableIndicator, new Vector2(0, 0), Quaternion.identity));
    }

    /*How path placement works:
     If we are in the path placing mode (edit path pressed etc.), then clicking on/dragging the mouse
     over will call this function to place a path. This function checks if the position we are attempting to place a
     path is on a placeable tile area (checks both valid position using the placeablePositions list
     and if that tile allows placement), and if so it replaces that tile with a path tile, adds this position to the path array,
     and then updates the placeablePositions list.     
     */

    public void placePath(Vector2Int position)
    {
        //Place path tile
        if (IsInBounds(position) && placeablePositions.Contains(position) && grid[position.x, position.y].getPlaceable() == true) { 
            Destroy(grid[position.x, position.y].gameObject);
            grid[position.x, position.y] = Instantiate(pathTile, new Vector2(position.x, position.y), Quaternion.identity);
            path.Add(position);

            //Update placeable grid areas
            placeablePositions.Clear();
            deletePlaceableIndicators();
            updatePlaceablePositions(position);
        }
    }

    public void deletePath(Vector2Int position)
    {
        //Delete path tile at the head of current path
        if (IsInBounds(position) && path.Count > 1 && path[path.Count - 1] == position)
        {
            Destroy(grid[position.x, position.y].gameObject);
            //Default instantiate a background tile for now, in the future can make a deep copy of initial
            //grid array at the start of the level in order to instantiate what was previously there
            grid[position.x, position.y] = Instantiate(backgroundTile, new Vector2(position.x, position.y), Quaternion.identity);
            path.Remove(position);

            //Update placeable grid areas
            placeablePositions.Clear();
            deletePlaceableIndicators();
            updatePlaceablePositions(path[path.Count - 1]);
        }
    }

    public bool placeTower(Vector2Int position, string type)
    {
        // Place tower tile
        // TODO: Add more conditions when other types of path exist
        if (!(grid[position.x, position.y] is PathTile)
            && !(grid[position.x, position.y] is TowerTile)) {
            Destroy(grid[position.x, position.y].gameObject);
            grid[position.x, position.y] = Instantiate(towerTile, new Vector2(position.x, position.y), Quaternion.identity);
            return true;

        } else {
            Debug.Log("Couldn't place tower :( (Is something else there?)");
            return false;
        }
    }

    public bool IsInBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }

    void updatePlaceablePositions(Vector2Int position)
    {
        Vector2Int[] directions = {Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down};

        foreach (var direction in directions) {
            Vector2Int adjacentPos = position + direction;

            if (IsInBounds(adjacentPos) && !path.Contains(adjacentPos)) {
                placeablePositions.Add(adjacentPos);
                GameObject indicator = Instantiate(placeableIndicator, new Vector2(adjacentPos.x, adjacentPos.y), Quaternion.identity);
                indicator.GetComponent<SpriteRenderer>().enabled = true;
                placeableIndicators.Add(indicator);
                
            }
        }
    }

    void updatePathSprites(Boolean placed) //Call this after each placement + deletion
    {
        if (placed)
        {
            //Deal with path array of size 1 first, then deal with 2, then deal with 3
            if (path.Count == 1)
            {
                //Set to generic starter path sprite (?)

            }
            else if (path.Count == 2)
            {
                //Base this off of the previous path location

            }
            else
            {
                //Base this off of the previous path location + check if there was a turn
            }
        }
        else{
            //path deleted case
            //Deal with path array of size 1 first, then deal with 2, then deal with 3
            if (path.Count == 1)
            {
                //Set to generic starter path sprite (?)

            }
            else if (path.Count == 2)
            {
                //Base this off of the previous path location

            }
            else
            {
                //Base this off of the previous path location + check if there was a turn
            }
        }
    }

    void deletePlaceableIndicators()
    {
        for (int i = 0; i < placeableIndicators.Count; i++) {
            Destroy(placeableIndicators[i].gameObject);
        }
        placeableIndicators.Clear();
    }

    public void setEditing(string mode)
    {
        editing = mode;
        if (editing.Equals("Path")) {
            mouseManager.setLock(false);
        }
    }

    public void togglePlaceableIndicatorsVisible()
    {
        foreach (GameObject indicator in placeableIndicators)
        {
            if (indicator != null)
            {
                indicator.GetComponent<SpriteRenderer>().enabled = !indicator.GetComponent<SpriteRenderer>().enabled;
            }
        }
    }
}
