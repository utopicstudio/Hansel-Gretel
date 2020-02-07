using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LODManager : MonoBehaviour
{
    public static LODManager Instance { get; private set; }
    public delegate void LODChange();
    public static event LODChange LODrefresh;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {

            Destroy(gameObject);
        }

        Instance = this;

    }

    public void UpdateLOD()
    {
        LODrefresh();

    }
}