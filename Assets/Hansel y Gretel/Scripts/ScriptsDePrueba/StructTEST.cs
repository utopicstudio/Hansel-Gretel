using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public struct Personaje1
{
    public NavMeshAgent agent;
    public Transform posicion;
}
[System.Serializable]
public struct Personaje2
{
    public NavMeshAgent agent;
    public Transform posicion;
}
[System.Serializable]
public struct Personaje3
{
    public NavMeshAgent agent;
    public Transform posicion;
}




public class StructTEST : MonoBehaviour
{
    public Personaje1 perosnaje1;
    public Personaje2 perosnaje2;
    public Personaje3 perosnaje3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
