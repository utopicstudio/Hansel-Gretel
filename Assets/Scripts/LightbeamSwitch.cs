using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightbeamSwitch : MonoBehaviour
{
    public bool OnSwitch;
    
   
    public Color colorluz;
    float intensity;
    public float speed = 10;
    public GameObject[] luzarray;
    private void Start()
    {
        luzarray = new GameObject[transform.childCount];
        
        for (int i = 0; i < transform.childCount;i++)
        {
            luzarray[i] = transform.GetChild(i).gameObject;

        }
    }


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < luzarray.Length; i++)
        {
            luzarray[i].GetComponent<Renderer>().material.SetColor("_TintColor", colorluz);
        }

        colorluz.a = intensity;
        colorluz.a = Mathf.Clamp(colorluz.a, 0, 3);

        intensity = Mathf.Clamp(intensity, 0, 1);

        if (OnSwitch)
        {

            intensity += Time.deltaTime * speed;

        }
        else
        {
            intensity -= Time.deltaTime * speed;
        }

    }
    // Turn ON /OFF the light for events
    public void OnLight()
    {
        if (OnSwitch)
        {
            OnSwitch = false;
        }
        else
        {
            OnSwitch = true;
        }

    }
}
