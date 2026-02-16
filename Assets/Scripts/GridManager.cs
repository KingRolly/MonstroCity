using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    public bool editing;
    public MouseManager mouseManager;
    public Vector2Int startPathPosition;
    public Vector2Int endPathPosition;

    // Start is called before the first frame update
    void Start()
    {
        placeTiles(width, height);
        editing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPathValid()) {
            print("PATH VALID NOW");
        }
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

        //Create the initial starting path tile
        grid[startPathPosition.x, startPathPosition.y] = Instantiate(pathTile, new Vector2(startPathPosition.x, startPathPosition.y), Quaternion.identity);
        path.Add(startPathPosition);
        updatePathSprites();

        //Create the end tile object
        grid[endPathPosition.x, endPathPosition.y] = Instantiate(pathTile, new Vector2(endPathPosition.x, endPathPosition.y), Quaternion.identity);

        //Update placeable grid areas
        placeablePositions.Clear();
        deletePlaceableIndicators();
        updatePlaceablePositions(startPathPosition, false);
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
        if (IsInBounds(position) && placeablePositions.Contains(position) && grid[position.x, position.y].getPlaceable()) { 
            Destroy(grid[position.x, position.y].gameObject);
            grid[position.x, position.y] = Instantiate(pathTile, new Vector2(position.x, position.y), Quaternion.identity);
            path.Add(position);

            updatePathSprites();

            // Print path positions for debugging
            Debug.Log(position.x + ", " + position.y);

            //Update placeable grid areas
            placeablePositions.Clear();
            deletePlaceableIndicators();
            updatePlaceablePositions(position, true);
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
            updatePathSprites();

            //Update placeable grid areas
            placeablePositions.Clear();
            deletePlaceableIndicators();
            updatePlaceablePositions(path[path.Count - 1], false);
        }
    }

    public bool placeTower(Vector2Int position, string type)
    {
        // Place tower tile
        // TODO: Add more conditions when other types of path exist
        if (grid[position.x, position.y].getPlaceable()) {
            Destroy(grid[position.x, position.y].gameObject);
            grid[position.x, position.y] = Instantiate(towerTile, new Vector2(position.x, position.y), Quaternion.identity);

            //Update placeable grid areas in case we placed it on a flag tile
            placeablePositions.Clear();
            deletePlaceableIndicators();
            updatePlaceablePositions(path[path.Count - 1], true); // TODO: Resolve bug that occurs here (out of bounds error)
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

    void updatePlaceablePositions(Vector2Int position, Boolean placingPath)
    {
        Vector2Int[] directions = {Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down};

        foreach (var direction in directions) {
            Vector2Int adjacentPos = position + direction;

            if (IsInBounds(adjacentPos) && !path.Contains(adjacentPos)) {
                if (grid[adjacentPos.x, adjacentPos.y].getPlaceable() || !placingPath)
                {
                    GameObject indicator = Instantiate(placeableIndicator, new Vector2(adjacentPos.x, adjacentPos.y), Quaternion.identity);
                    indicator.GetComponent<SpriteRenderer>().enabled = true;
                    placeableIndicators.Add(indicator);
                }
                placeablePositions.Add(adjacentPos);
            }
        }
    }

    void isNextToEndTile(Vector2Int position)
    {
        
    }


    enum PathDirection { Up, Down, Left, Right }
    PathDirection getDirection(Vector2Int previous, Vector2Int next)
    {
        if (next.x > previous.x) return PathDirection.Right;
        if (next.x < previous.x) return PathDirection.Left;
        if (next.y > previous.y) return PathDirection.Up;
        return PathDirection.Down;
    }

    void updatePathSprites() //Call this after each placement + deletion
    {
        //Deal with path array of size 1 first, then deal with 2, then deal with 3
        if (path.Count == 1)
        {
            //Set to generic starter path sprite (?) (this will be changed once we add the start/end tiles)
            grid[path[0].x, path[0].y].setSpriteType(PathTile.spriteType.Vertical);
        }
        else if (path.Count == 2)
        {
            //Base this off of the previous path location
            PathDirection direction = getDirection(path[0], path[1]);
            grid[path[0].x, path[0].y].setSpriteType(
                (direction == PathDirection.Right || direction == PathDirection.Left) ?
                PathTile.spriteType.Horizontal : PathTile.spriteType.Vertical
                );
            grid[path[1].x, path[1].y].setSpriteType(
                direction == PathDirection.Right ?
                PathTile.spriteType.RightEnd : direction == PathDirection.Left ?
                PathTile.spriteType.LeftEnd : direction == PathDirection.Up ?
                PathTile.spriteType.UpEnd : PathTile.spriteType.DownEnd
                );
        }
        else
        {
            //Base this off of the previous path location + check if there was a turn
            PathDirection directionToHead = getDirection(path[path.Count - 2], path[path.Count - 1]);
            PathDirection directionToMid = getDirection(path[path.Count - 3], path[path.Count - 2]);

            if (directionToHead == directionToMid)
            {
                grid[path[path.Count - 2].x, path[path.Count - 2].y].setSpriteType(
                    (directionToHead == PathDirection.Left || directionToHead == PathDirection.Right) ?
                    PathTile.spriteType.Horizontal : PathTile.spriteType.Vertical
                );
            }
            else
            {
                if (directionToMid == PathDirection.Left && directionToHead == PathDirection.Up)
                    grid[path[path.Count - 2].x, path[path.Count - 2].y].setSpriteType(PathTile.spriteType.DownRight);
                if (directionToMid == PathDirection.Left && directionToHead == PathDirection.Down)
                    grid[path[path.Count - 2].x, path[path.Count - 2].y].setSpriteType(PathTile.spriteType.UpRight);
                if (directionToMid == PathDirection.Right && directionToHead == PathDirection.Up)
                    grid[path[path.Count - 2].x, path[path.Count - 2].y].setSpriteType(PathTile.spriteType.DownLeft);
                if (directionToMid == PathDirection.Right && directionToHead == PathDirection.Down)
                    grid[path[path.Count - 2].x, path[path.Count - 2].y].setSpriteType(PathTile.spriteType.UpLeft);

                if (directionToMid == PathDirection.Up && directionToHead == PathDirection.Left)
                    grid[path[path.Count - 2].x, path[path.Count - 2].y].setSpriteType(PathTile.spriteType.UpLeft);
                if (directionToMid == PathDirection.Up && directionToHead == PathDirection.Right)
                    grid[path[path.Count - 2].x, path[path.Count - 2].y].setSpriteType(PathTile.spriteType.UpRight);
                if (directionToMid == PathDirection.Down && directionToHead == PathDirection.Left)
                    grid[path[path.Count - 2].x, path[path.Count - 2].y].setSpriteType(PathTile.spriteType.DownLeft);
                if (directionToMid == PathDirection.Down && directionToHead == PathDirection.Right)
                    grid[path[path.Count - 2].x, path[path.Count - 2].y].setSpriteType(PathTile.spriteType.DownRight);
            }

            // Update head of path
            grid[path[path.Count - 1].x, path[path.Count - 1].y].setSpriteType(
                directionToHead == PathDirection.Up ? PathTile.spriteType.UpEnd :
                directionToHead == PathDirection.Down ? PathTile.spriteType.DownEnd :
                directionToHead == PathDirection.Left ? PathTile.spriteType.LeftEnd :
                PathTile.spriteType.RightEnd
            );
        }
    }

    public bool isPathValid()
    {
        return (path[0] == startPathPosition && path[path.Count - 1] == endPathPosition);
    }

    void deletePlaceableIndicators()
    {
        for (int i = 0; i < placeableIndicators.Count; i++) {
            Destroy(placeableIndicators[i].gameObject);
        }
        placeableIndicators.Clear();
    }

    public void toggleEditing()
    {
        editing = !editing;
        mouseManager.setLock(false);
    }

    public bool getEditing() {
        return editing;
    }

    public List<Vector2Int> getPath()
    {
        return path;
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
