using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TpHanselGretelCisne : MonoBehaviour
{

    public Vector3 warpPosition;
    //PRIMERA FASE
    [Header("PRIMERA FASE")]
    public GameObject hansel;
    public GameObject gretel;
    public GameObject hanselPos1;
    public GameObject gretelPos1;
    public GameObject cisnePos1;
    public GameObject movedor;
    
    //SEGUNDA FASE
    [Header("SEGUNDA FASE")]
    public GameObject cam;
    public GameObject camPos;
    public GameObject hanselPos2;
    public GameObject gretelPos2;
    public GameObject father;
    public GameObject cisnePos2;

    [Header("Final")]
    public GameObject camPos3;

    public void Teleportation() {
        hansel.transform.position = hanselPos1.transform.position;
        hansel.transform.rotation = cisnePos1.transform.rotation;
        gretel.transform.position = gretelPos1.transform.position;
        gretel.transform.rotation = cisnePos1.transform.rotation;
     
        //   StartCoroutine("DelayMovimiento");
        //   StartCoroutine("StopMovimiento");
    }


    public void OnArriveCamMove() {       
        cam.transform.position = camPos.transform.position;
        cam.transform.rotation = camPos.transform.rotation;
    }

    public void OnArriveBack()
    {
        hansel.transform.position = hanselPos2.transform.position;
        hansel.transform.rotation = hanselPos2.transform.rotation;
        gretel.transform.position = gretelPos2.transform.position;
        gretel.transform.rotation = gretelPos2.transform.rotation;

    }

    public void FinalTp() {
        cam.transform.position = camPos3.transform.position;
        cam.transform.rotation= camPos3.transform.rotation;
    }

}

