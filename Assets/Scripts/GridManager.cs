using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int width = 16;
    public static int height = 9;
    public static GameObject[,] grid = new GameObject[width, height];
    public GameObject tile;

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
                grid[i, j] = Instantiate(tile, new Vector2(i, j), Quaternion.identity);
                // Initializes tile info
                //Tile tileData = grid[i, j].GetComponent<Tile>();
                //tileData.Initialize("something");
            }
        }

    }
}
