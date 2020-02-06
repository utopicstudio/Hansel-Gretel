using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punto : MonoBehaviour
{
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "canasta")
        {
            col.gameObject.GetComponent<Puntaje>().puntos++;
            col.gameObject.GetComponent<Puntaje>().Checker();
            col.gameObject.GetComponent<Puntaje>().PointSound();
            gameObject.SetActive(false);
            //Debug.Log("Toque algo con trigg");

        }
    }
}
