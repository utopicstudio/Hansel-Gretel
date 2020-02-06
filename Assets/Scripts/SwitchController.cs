using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{

    public Animator animator;
    public bool OnOff;
    // Start is called before the first frame update
    void Start()
    {
        animator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OnOff)
        {
            animator.SetBool("On", true);
        }
        else
        {
            animator.SetBool("On", false);
        }

    }
    public void OnSwitch()//Via EventSystem
    {
        if (OnOff)
        {
            OnOff = false;
        }
        else
        {
            OnOff = true;
        }
    }
}
