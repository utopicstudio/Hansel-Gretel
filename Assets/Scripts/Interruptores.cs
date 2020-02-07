using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interruptores : MonoBehaviour
{
    public Animator animator;
    public int IDNumber;
    public bool switcheado;
    public GameObject Manager;
    private InterruptorManager interruptorManager;
    public Material[] materiales;
    public Renderer[] render;

    public void Start()
    {
        interruptorManager =  Manager.gameObject.GetComponent<InterruptorManager>();
    }

    public void selected (){

        animator.SetTrigger("switch");

        if (switcheado == false)
        {
            switcheado = true;
            
            switch (IDNumber)
            {
                case 1:
                    interruptorManager.booleanos[1] = true;
                    render[1].material = materiales[0];
                    break;
                case 2:
                    interruptorManager.booleanos[0] = false;
                    interruptorManager.booleanos[1] = true;
                    interruptorManager.booleanos[2] = true;
                    render[0].material = materiales[1];
                    render[1].material = materiales[0];
                    render[2].material = materiales[0];
                    break;
                case 3:
                    interruptorManager.booleanos[0] = true;
                    interruptorManager.booleanos[1] = false;
                    interruptorManager.booleanos[2] = true;
                    render[0].material = materiales[0];
                    render[1].material = materiales[1];
                    render[2].material = materiales[0];
                    break;
                case 4:     
                    /*interruptorManager.booleanos[0] = false;
                    interruptorManager.booleanos[1] = false;
                    interruptorManager.booleanos[3] = true;
                    render[0].material = materiales[1];
                    render[1].material = materiales[1];
                    render[3].material = materiales[0];
                    break;*/
                case 5:
                    /*interruptorManager.booleanos[1] = false;
                    interruptorManager.booleanos[3] = true;
                    render[1].material = materiales[1];
                    render[3].material = materiales[0];
                    break; */
                case 6:
                    
                    interruptorManager.booleanos[1] = true;            
                    render[1].material = materiales[0];

                    break;
                case 7:
                    
                    interruptorManager.booleanos[1] = false;
                    interruptorManager.booleanos[3] = true;
                   
                    render[1].material = materiales[1];
                    render[3].material = materiales[0];
                    break;
                case 8:
                    interruptorManager.booleanos[2] = true;
                    interruptorManager.booleanos[3] = true;
                    render[2].material = materiales[0];
                    render[3].material = materiales[0];
                    break;
                default:
                    print("no le pusiste ningun numero");
                    break;
            }
            IDNumber = IDNumber*-1;
            interruptorManager.CheckComplete();
        }
        else {
            switcheado = false;
            switch (IDNumber)
            {
                case -1:
                    interruptorManager.booleanos[1] = true;
                    render[1].material = materiales[0];
                    break;
                case -2:
                    interruptorManager.booleanos[0] = true;
                    interruptorManager.booleanos[1] = false;
                    interruptorManager.booleanos[2] = false;
                    render[0].material = materiales[0];
                    render[1].material = materiales[1];
                    render[2].material = materiales[1];
                    break;
                case -3:
                    interruptorManager.booleanos[0] = false;
                    interruptorManager.booleanos[1] = true;
                    interruptorManager.booleanos[2] = false;
                    render[0].material = materiales[1];
                    render[1].material = materiales[0];
                    render[2].material = materiales[1];
                    break;
                case -4:
                    /*interruptorManager.booleanos[3] = false;
                    interruptorManager.booleanos[0] = true;
                    interruptorManager.booleanos[1] = true;
                    render[3].material = materiales[1];
                    render[0].material = materiales[0];
                    render[1].material = materiales[0];
                    break;*/
                case -5:
                    /*interruptorManager.booleanos[1] = true;
                    interruptorManager.booleanos[3] = false;
                    render[1].material = materiales[0];
                    render[3].material = materiales[1];
                    break; */
                case -6:
                    interruptorManager.booleanos[1] = true;
                    render[1].material = materiales[0];
                    break;
                case -7:
                   
                    interruptorManager.booleanos[1] = true;
                    interruptorManager.booleanos[3] = false;
                    
                    render[1].material = materiales[0];
                    render[3].material = materiales[1];
                    break;
                case -8:
                    interruptorManager.booleanos[2] = false;
                    interruptorManager.booleanos[3] = false;
                    render[2].material = materiales[1];
                    render[3].material = materiales[1];
                    break;
                default:
                    print("no le pusiste ningun numero");
                    break;
            }
            IDNumber = IDNumber * -1;
            interruptorManager.CheckComplete();
        }
    }
}
