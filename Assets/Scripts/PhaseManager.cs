using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages day and night cycle within a level
/// - Jack Peters (Mar. 1st, 2026)
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
        SetDaytime(false);
    }

    public bool GetDaytime()
    {
        return daytime;
    }

    public void SetDaytime(bool state)
    {
        
    }
}
