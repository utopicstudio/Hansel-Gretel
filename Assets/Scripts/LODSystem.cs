using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LODSystem : MonoBehaviour
{

    public MeshRenderer[] lods;
    public float[] distances;
    GameObject SceneCamera;
    private float distance;

    private void OnEnable()
    {
        LODManager.LODrefresh += DistanceCheck;

    }
    private void OnDisable()
    {
        LODManager.LODrefresh -= DistanceCheck;
    }

    void Start()
    {
        SceneCamera = GameObject.FindGameObjectWithTag("MainCamera");
        DistanceCheck();


    }

    public void DistanceCheck()
    {
        
           distance = Vector3.Distance(SceneCamera.transform.position, transform.position);

        int n = 0;
        for (int i = 0; i < lods.Length; i++)
        {
            lods[i].enabled = false;
            if (distance > distances[i])
                n = i;

        }

        lods[n].enabled = true;

    }
}


