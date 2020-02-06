using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activador : MonoBehaviour
{
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Trigg")
        {
            col.gameObject.GetComponent<SliderFill>().looked = true;
            //Debug.Log("Toque algo con trigg");

        }

        if (col.gameObject.tag == "Meta")
        {
            col.gameObject.GetComponent<Meta>().looked = true;
            //Debug.Log("Toque algo con meta");

        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Trigg")
        {
            col.gameObject.GetComponent<SliderFill>().looked = false;
            //Debug.Log("Toque algo con trigg");

        }

        if (col.gameObject.tag == "Meta")
        {
            col.gameObject.GetComponent<Meta>().looked = false;
            //Debug.Log("Toque algo con meta");

        }

    }


}
