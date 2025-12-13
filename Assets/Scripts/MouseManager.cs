using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public GameObject indicator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPos = Input.mousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector2 selectPos = new Vector2(Mathf.Round(worldPos.x), Mathf.Round(worldPos.y));
        if (selectPos.x > 15) selectPos.x = 15;
        if (selectPos.x < 0) selectPos.x = 0;
        if (selectPos.y > 8) selectPos.y = 8;
        if (selectPos.y < 0) selectPos.y = 0;
        indicator.transform.position = selectPos;
        Debug.Log("[" + selectPos.x + ", " + selectPos.y + "]");
    }
}
