using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using JetBrains.Annotations;
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
    public bool editing;
    public MouseManager mouseManager;
    public Vector2Int startPathPosition;
    public Vector2Int endPathPosition;
    public PhaseManager phaseManager;

    // Start is called before the first frame update
    void Start()
    {
        PlaceTiles(width, height);
        editing = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void PlaceTiles(int width, int height)
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
        UpdatePathSprites();


        //Create the end tile object
        grid[endPathPosition.x, endPathPosition.y] = Instantiate(pathTile, new Vector2(endPathPosition.x, endPathPosition.y), Quaternion.identity);
        grid[endPathPosition.x, endPathPosition.y].SetSpriteType(PathTile.spriteType.End);

        //Update placeable grid areas
        placeablePositions.Clear();
        DeletePlaceableIndicators();
        UpdatePlaceablePositions(startPathPosition);
    }

    /*How path placement works:
     If we are in the path placing mode (edit path pressed etc.), then clicking on/dragging the mouse
     over will call this function to place a path. This function checks if the position we are attempting to place a
     path is on a placeable tile area (checks both valid position using the placeablePositions list
     and if that tile allows placement), and if so it replaces that tile with a path tile, adds this position to the path array,
     and then updates the placeablePositions list.     
     */

    public void PlacePath(Vector2Int position)
    {
        //Place path tile
        if (IsInBounds(position) && placeablePositions.Contains(position) && grid[position.x, position.y].GetPlaceable()) {
            Destroy(grid[position.x, position.y].gameObject);
            grid[position.x, position.y] = Instantiate(pathTile, new Vector2(position.x, position.y), Quaternion.identity);
            path.Add(position);

            if (IsNextToEndTile(position))
            {
                //Add end tile to our path, and connect the sprite up with it properly
                path.Add(endPathPosition);
                UpdatePathSprites(true);
                placeablePositions.Clear();
                DeletePlaceableIndicators();
            }
            else {
                UpdatePathSprites();

                // Print path positions for debugging
                Debug.Log(position.x + ", " + position.y);

                //Update placeable grid areas
                placeablePositions.Clear();
                DeletePlaceableIndicators();
                if (path.Count != 0)
                {
                    UpdatePlaceablePositions(path[path.Count - 1]);
                }
            }
        }
    }

    public void DeletePath(Vector2Int position)
    {
        if (position == endPathPosition) {
            return;
        }

        //Delete path tile at the head of current path (excluding end tile)
        if (IsNextToEndTile(position) && path[path.Count - 2] == position)
        {
            //Remove end tile from path
            path.RemoveAt(path.Count - 1);
        }

        if (IsInBounds(position) && path.Count > 1 && path[path.Count - 1] == position)
        {
            Destroy(grid[position.x, position.y].gameObject);
            //Default instantiate a background tile for now, in the future can make a deep copy of initial
            //grid array at the start of the level in order to instantiate what was previously there
            grid[position.x, position.y] = Instantiate(backgroundTile, new Vector2(position.x, position.y), Quaternion.identity);
            path.Remove(position);
            UpdatePathSprites();

            //Update placeable grid areas
            placeablePositions.Clear();
            DeletePlaceableIndicators();
            if (path.Count != 0)
            {
                UpdatePlaceablePositions(path[path.Count - 1], position);
            }
        }
    }

    public bool PlaceTower(Vector2Int position, TowerData data)
    {
        // Place tower tile
        // TODO: Add more conditions when other types of path exist
        if (grid[position.x, position.y].GetPlaceable()) {
            Destroy(grid[position.x, position.y].gameObject);
            TowerTile tower;
            tower = Instantiate(data.towerType, new Vector2(position.x, position.y), Quaternion.identity);
            tower.GetComponent<SpriteRenderer>().sprite = mouseManager.GetSelectedTowerIcon().GetSprite();
            grid[position.x, position.y] = tower;

            if (!IsPathValid())
            {
                //Update placeable grid areas in case we placed it on a flag tile
                placeablePositions.Clear();
                DeletePlaceableIndicators();
                if (path.Count != 0)
                {
                    UpdatePlaceablePositions(path[path.Count - 1]);
                }
            }
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

    void UpdatePlaceablePositions(Vector2Int position, Vector2Int? deletedPathPos = null)
    {
        Vector2Int[] directions = { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };

        foreach (var direction in directions)
        {
            Vector2Int adjacentPos = position + direction;

            if (IsInBounds(adjacentPos) && !path.Contains(adjacentPos))
            {
                if (grid[adjacentPos.x, adjacentPos.y].GetPlaceable() || adjacentPos.Equals(deletedPathPos))
                {
                    GameObject indicator = Instantiate(placeableIndicator, new Vector2(adjacentPos.x, adjacentPos.y), Quaternion.identity);
                    indicator.GetComponent<SpriteRenderer>().enabled = true;
                    placeableIndicators.Add(indicator);
                }
                placeablePositions.Add(adjacentPos);
            }
        }
    }

    bool IsNextToEndTile(Vector2Int position)
    {
        Vector2Int[] directions = { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };

        foreach (var direction in directions)
        {
            Vector2Int adjacentPos = position + direction;

            if (IsInBounds(adjacentPos) && adjacentPos.x == endPathPosition.x && adjacentPos.y == endPathPosition.y) {
                return true;
            }
        }

        return false;
    }

    enum PathDirection { Up, Down, Left, Right }
    PathDirection GetDirection(Vector2Int previous, Vector2Int next)
    {
        if (next.x > previous.x) return PathDirection.Right;
        if (next.x < previous.x) return PathDirection.Left;
        if (next.y > previous.y) return PathDirection.Up;
        return PathDirection.Down;
    }

    void UpdatePathSprites(bool connectingToEndTile = false) //Call this after each placement + deletion
    {
        //Deal with path array of size 1 first, then deal with 2, then deal with 3
        if (path.Count == 1)
        {
            //Set to generic starter path sprite (?) (this will be changed once we add the start/end tiles)
            grid[path[0].x, path[0].y].SetSpriteType(PathTile.spriteType.Start);
        }
        else if (path.Count == 2)
        {
            //Base this off of the previous path location
            PathDirection direction = GetDirection(path[0], path[1]);

            //DONT NEED TO ALTER THE STARTING TILE 
            //grid[path[0].x, path[0].y].setSpriteType(
            //    (direction == PathDirection.Right || direction == PathDirection.Left) ?
            //    PathTile.spriteType.Horizontal : PathTile.spriteType.Vertical
            //    );


            grid[path[1].x, path[1].y].SetSpriteType(
                direction == PathDirection.Right ?
                PathTile.spriteType.RightEnd : direction == PathDirection.Left ?
                PathTile.spriteType.LeftEnd : direction == PathDirection.Up ?
                PathTile.spriteType.UpEnd : PathTile.spriteType.DownEnd
                );
        }
        else
        {
            //Base this off of the previous path location + check if there was a turn
            PathDirection directionToMid = GetDirection(path[path.Count - 3], path[path.Count - 2]);
            PathDirection directionToHead = GetDirection(path[path.Count - 2], path[path.Count - 1]);

            UpdateMiddleTile(path.Count - 2, directionToMid, directionToHead);

            if (connectingToEndTile && path.Count >= 4)
            {
                PathDirection dirToMid =
                    GetDirection(path[path.Count - 4], path[path.Count - 3]);

                PathDirection dirFromMid =
                    GetDirection(path[path.Count - 3], path[path.Count - 2]);

                UpdateMiddleTile(path.Count - 3, dirToMid, dirFromMid);
            }
            if (!connectingToEndTile)
            {
                // Update head of path
                grid[path[path.Count - 1].x, path[path.Count - 1].y].SetSpriteType(
                    directionToHead == PathDirection.Up ? PathTile.spriteType.UpEnd :
                    directionToHead == PathDirection.Down ? PathTile.spriteType.DownEnd :
                    directionToHead == PathDirection.Left ? PathTile.spriteType.LeftEnd :
                    PathTile.spriteType.RightEnd
                );
            }
        }
    }

    void UpdateMiddleTile(int index, PathDirection directionToMid, PathDirection directionToHead)
    {
        if (directionToHead == directionToMid)
        {
            grid[path[index].x, path[index].y].SetSpriteType(
                (directionToHead == PathDirection.Left || directionToHead == PathDirection.Right) ?
                PathTile.spriteType.Horizontal : PathTile.spriteType.Vertical
            );
        }
        else
        {
            if (directionToMid == PathDirection.Left && directionToHead == PathDirection.Up)
                grid[path[index].x, path[index].y].SetSpriteType(PathTile.spriteType.DownRight);
            if (directionToMid == PathDirection.Left && directionToHead == PathDirection.Down)
                grid[path[index].x, path[index].y].SetSpriteType(PathTile.spriteType.UpRight);
            if (directionToMid == PathDirection.Right && directionToHead == PathDirection.Up)
                grid[path[index].x, path[index].y].SetSpriteType(PathTile.spriteType.DownLeft);
            if (directionToMid == PathDirection.Right && directionToHead == PathDirection.Down)
                grid[path[index].x, path[index].y].SetSpriteType(PathTile.spriteType.UpLeft);

            if (directionToMid == PathDirection.Up && directionToHead == PathDirection.Left)
                grid[path[index].x, path[index].y].SetSpriteType(PathTile.spriteType.UpLeft);
            if (directionToMid == PathDirection.Up && directionToHead == PathDirection.Right)
                grid[path[index].x, path[index].y].SetSpriteType(PathTile.spriteType.UpRight);
            if (directionToMid == PathDirection.Down && directionToHead == PathDirection.Left)
                grid[path[index].x, path[index].y].SetSpriteType(PathTile.spriteType.DownLeft);
            if (directionToMid == PathDirection.Down && directionToHead == PathDirection.Right)
                grid[path[index].x, path[index].y].SetSpriteType(PathTile.spriteType.DownRight);
        }
    }

    public bool IsPathValid()
    {
        return (path[0] == startPathPosition && path[path.Count - 1] == endPathPosition);
    }

    void DeletePlaceableIndicators()
    {
        for (int i = 0; i < placeableIndicators.Count; i++) {
            Destroy(placeableIndicators[i].gameObject);
        }
        placeableIndicators.Clear();
    }

    public void ToggleEditing()
    {
        editing = !editing;
        mouseManager.SetLock(false);
    }

    public bool GetEditing() {
        return editing;
    }

    public List<Vector2Int> GetPath()
    {
        return path;
    }

    public void TogglePlaceableIndicatorsVisible()
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
