using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int width = 16;
    public static int height = 9;
    public static Tile[,] grid = new Tile[width, height];
    public static List<Vector2> path = new List<Vector2>();
    public Tile backgroundTile;
    public PathTile pathTile;
    public bool editingPath = false;

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

        //Setting start of path to (0, 0)
        grid[0, 0].setPlaceable(true);
    }

    /*How path placement works:
     If we are in the path placing mode (edit path pressed etc.), then clicking on/dragging the mouse
     over will call this function to place a path. This function checks if the position we are attempting to place a
     path is on a placeable tile, and if so it replaces that tile with a path tile, adds this position to the path array,
     and then updates the placeable value of all relevant tiles.     
     */

    public void placePath(Vector2 position)
    {
        Vector2Int intPosition = new Vector2Int((int)position.x, (int)position.y);

        //Place path tile
        if (IsInBounds(intPosition) && grid[intPosition.x, intPosition.y].getPlaceable() == true) { 
            Destroy(grid[intPosition.x, intPosition.y]);
            grid[intPosition.x, intPosition.y] = Instantiate(pathTile, position, Quaternion.identity);
            path.Add(position);

            //Update placeable status for previous placeable tiles
            if (path.Count > 1) {
                Vector2 prevPos = path[path.Count - 2];
                updatePlaceableStatus(prevPos, false);
                
            }

            //Update placeable status for new placeable tiles
            updatePlaceableStatus(position, true);
        }
    }

    bool IsInBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < grid.GetLength(0) && position.y >= 0 && position.y < grid.GetLength(1);
    }

    void updatePlaceableStatus(Vector2 position, bool val)
    {
        Vector2Int[] directions = {Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down};
        Vector2Int intPosition = new Vector2Int((int)position.x, (int)position.y);

        foreach (var direction in directions) {
            Vector2Int adjacentPos = intPosition + direction;

            if (IsInBounds(adjacentPos) && !path.Contains(adjacentPos)) {
                grid[adjacentPos.x, adjacentPos.y].setPlaceable(val);
            }
        }
    }
    public void toggleEditingPath()
    {
        editingPath = !editingPath;
    }
}
