using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 16;
    public int height = 9;

    public GameObject tile;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Instantiate(tile, new Vector2(i, j), Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
