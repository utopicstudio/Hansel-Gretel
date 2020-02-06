using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed;
    public GameObject objeto;
    public bool rotar;

    public void StartRotation()
    {
        

        rotar = true;
            InvokeRepeating("Rotation", 1, 1);
        
        
    }
    public void Rotation (){
        if (rotar)
        {
            objeto.transform.Rotate(0, 0, 45);
        }
       // if (!rotar) { 
        

    }

    public void StopRotation() {
       
        rotar = false;
        CancelInvoke("Rotation");
    }


}
   
  

