using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public float posX;
    public float posY;
    public GameObject tile;

    // Start is called before the first frame update
    void Start()
    {
        posX = transform.position.x;
        posY = transform.position.y;
    }

    // Called from GridManager to preset certain information?
    public void Initialize()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Pressing "P" prints tile info when hovering
    void OnMouseOver()
    {
        if (Input.GetKeyUp(KeyCode.P)) {
            Debug.Log("[" + posX + ", " + posY + "]");
        }
    }
}