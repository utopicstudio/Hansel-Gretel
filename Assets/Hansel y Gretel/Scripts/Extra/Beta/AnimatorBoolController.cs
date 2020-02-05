using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorBoolController : MonoBehaviour
{
    //Claudio Inostroza shit script

    public Animator animator;//Animator to control
    public enum AnimationType {Idle, Speak, Walk, Bye, Push, Sit, Fear, Consolate, Caught, Dead, Speak2}

    public AnimationType anim;
    //public bool[] behavior;//List Of bool
    //public string[] BehaviorName;//list of Name of the bool in the animator controller

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        UpdateAnimation();
    }
    //private void OnValidate()
    //{
    //    UpdateAnimation();
    //}
    public void SetAnim(AnimationType animType)
    {
        print("--------Called----");
        anim = animType;
        UpdateAnimation();
    }
    void UpdateAnimation()
    {
        animator.SetTrigger(anim.ToString());

        /*
        if (behavior[0] == true)
        {
            animator.SetTrigger(BehaviorName[0]);
            behavior[0] = false;
        }

        if (behavior[1] == true)
        {
            animator.SetTrigger(BehaviorName[1]);
            behavior[1] = false;
        }

        if (behavior[2] == true)
        {
            animator.SetTrigger(BehaviorName[2]);
            behavior[2] = false;
        }

        if (behavior[3] == true)
        {
            animator.SetTrigger(BehaviorName[3]);
            behavior[3] = false;
        }

        if (behavior[4] == true)
        {
            animator.SetTrigger(BehaviorName[4]);
            behavior[4] = false;
        }

        if (behavior[5] == true)
        {
            animator.SetTrigger(BehaviorName[5]);
            behavior[5] = false;
        }*/
    }

}
