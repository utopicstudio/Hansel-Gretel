using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushAnswers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void JPushTheAnswers()
    {
        J.J2.Instance.GetComponent<J.JResourceManager>().PushAnswers();
    }
}
