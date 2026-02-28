using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages day and night cycle within a level
/// </summary>
public class PhaseManager : MonoBehaviour
{
    [SerializeField] private bool daytime;

    // Start is called before the first frame update
    void Start()
    {
        BeginLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BeginLevel()
    {
        daytime = false;
    }
}
