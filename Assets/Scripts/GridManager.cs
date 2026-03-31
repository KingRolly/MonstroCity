using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [Header("Grid Info")]
    public static int width = 16;
    public static int height = 9;
    public Vector2Int startPathPosition;
    public Vector2Int endPathPosition;
    public Tile[,] grid;
    public List<Vector2Int> path;
    public List<Vector2Int> placeablePositions;
    public List<GameObject> placeableIndicators;
    public bool editing;

    [Header("References")]
    public GameObject placeableIndicator;
    public BackgroundTile backgroundTile;
    public ObstacleTile obstacleTile;
    public PathTile pathTile;
    
    [Header("Managers")]
    public MouseManager mouseManager;
    public PhaseManager phaseManager;
    public UIManager uiManager;

    //defines the initial layout of tiles, 0 for background tile, 1 for obstacle tile
    public int[,] initialLayout = {
        {1, 1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0},
        {1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1},
        {0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1},
        {1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
        {0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };

    // Start is called before the first frame update
    void Start()
    {
        grid = new Tile[width, height];
        path = new List<Vector2Int>();
        placeablePositions = new List<Vector2Int>();
        placeableIndicators = new List<GameObject>();

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
                if (initialLayout[height - j - 1, i] == 1)
                {
                    grid[i, j] = Instantiate(obstacleTile, new Vector2(i, j), Quaternion.identity);
                }
                else
                {
                    grid[i, j] = Instantiate(backgroundTile, new Vector2(i, j), Quaternion.identity);
                }
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

    public bool PlacePath(Vector2Int position)
    {
        //Place path tile
        if (IsPosPlaceable(position)) // Check position is valid
        {
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
            return true;
        }
        return false;
    }

    /// <summary>
    /// Check if the given position is a placeable tile
    /// </summary>
    /// <param name="position">position to check</param>
    /// <returns>true if tile at given position is placeable, false otherwise</returns>
    public bool IsPosPlaceable(Vector2Int position)
    {
        return (IsInBounds(position) && placeablePositions.Contains(position) && grid[position.x, position.y].GetPlaceable());
    }

    public bool DeletePath(Vector2Int position)
    {
        if (position == endPathPosition) {
            return false;
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
            return true;
        }
        return false;
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
            uiManager.DisplayGameAnnouncement("A tower cannot be built here!", 2);
            return false;
        }
    }

    public bool DestroyTower(Vector2Int position)
    {
        if (IsInBounds(position))
        {
            Destroy(grid[position.x, position.y].gameObject);
            //Default instantiate a background tile for now, in the future can make a deep copy of initial
            //grid array at the start of the level in order to instantiate what was previously there
            grid[position.x, position.y] = Instantiate(backgroundTile, new Vector2(position.x, position.y), Quaternion.identity);

            if (!IsPathValid())
            {
                //Update placeable grid areas in case it was next to the path head
                placeablePositions.Clear();
                DeletePlaceableIndicators();
                if (path.Count != 0)
                {
                    UpdatePlaceablePositions(path[path.Count - 1]);
                }
            }
            return true;
        }
        return false;
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

            // Change start tile sprite
            grid[path[0].x, path[0].y].SetSpriteType(
                (direction == PathDirection.Right || direction == PathDirection.Left) ?
                PathTile.spriteType.Horizontal : PathTile.spriteType.DownLeft
                );

            // Change second tile sprite
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
                // Update middle tile sprite
                PathDirection dirToMid =
                    GetDirection(path[path.Count - 4], path[path.Count - 3]);

                PathDirection dirFromMid =
                    GetDirection(path[path.Count - 3], path[path.Count - 2]);

                UpdateMiddleTile(path.Count - 3, dirToMid, dirFromMid);

                // Update end tile sprite
                grid[width - 1, height - 1].SetSpriteType(
                    directionToHead == PathDirection.Up ? PathTile.spriteType.Vertical : PathTile.spriteType.DownLeft);
            }
            if (!connectingToEndTile)
            {
                // Revert end of path to default sprite if the path isn't connected to it anymore
                grid[width - 1, height - 1].SetSpriteType(
                    PathTile.spriteType.DownEnd);

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
        mouseManager.SetSelectedTowerIcon(null);
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
