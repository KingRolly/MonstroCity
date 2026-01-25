using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public GameObject indicator;
    Vector2 selectPos;
    public static bool locked;
    [SerializeField] Sprite hover;
    [SerializeField] Sprite select;
    public GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 screenPos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        if (!locked)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = hover;
            selectPos = new Vector2(Mathf.Round(worldPos.x), Mathf.Round(worldPos.y));
        } else {
            gameObject.GetComponent<SpriteRenderer>().sprite = select;
        }

        // Keep indicator within bounds
        if (selectPos.x > 15) selectPos.x = 15;
        if (selectPos.x < 0) selectPos.x = 0;
        if (selectPos.y > 8) selectPos.y = 8;
        if (selectPos.y < 0) selectPos.y = 0;
        
        indicator.transform.position = selectPos;

        //Attempt to place tiles
        if (Input.GetMouseButton(0) && gridManager.editingPath)
        {
            gridManager.placePath(new Vector2Int((int)selectPos.x, (int)selectPos.y));
        }
        else if(Input.GetMouseButton(1) && gridManager.editingPath) //right click to delete
        {
            gridManager.deletePath(new Vector2Int((int)selectPos.x, (int)selectPos.y));
        }

        // Click while hovering to "lock" or "unlock" position
        if (Input.GetMouseButtonDown(0) && !gridManager.editingPath)
        {
            if (selectPos == new Vector2(Mathf.Round(worldPos.x), Mathf.Round(worldPos.y)))
            {
                setLock(!locked);
            }
            else
            {
                selectPos = new Vector2(Mathf.Round(worldPos.x), Mathf.Round(worldPos.y));
            }
            Debug.Log(selectPos);
        }
    }

    public void setLock(bool val)
    {
        locked = val;
    }
}