using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickering : MonoBehaviour
{

    
    public MeshRenderer mesh;
    public GameObject flare;
    public GameObject luz;
    float minFlickerSpeed = 0.1f;
    float maxFlickerSpeed = 1.0f;


    private void Start()
    {
        InvokeRepeating("Wait", 2.0f, 0.3f);
       
    }


    void Wait() {
        //Debug.Log("Funciona");

        mesh.enabled = false;
        flare.SetActive(false);
        luz.SetActive(false);
        StartCoroutine("Seconds");

    }

     IEnumerator Seconds() {
        yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
        mesh.enabled = true;
        flare.SetActive(true);
        luz.SetActive(true);
    }
}


