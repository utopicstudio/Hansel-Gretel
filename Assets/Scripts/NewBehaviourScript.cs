using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void Test(string name)
    {
        Animator anim = GetComponent<Animator>();

        if(anim)
        {
            anim.SetBool(name, true);
        }
    }
}
