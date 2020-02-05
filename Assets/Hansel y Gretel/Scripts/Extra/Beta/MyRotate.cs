using UnityEngine;
using System.Collections;

public class MyRotate : MonoBehaviour {

	public bool global = true;

    public bool OnRotate;

	//[TooltipAttribute("0 for infinite. 1 for one turn. 2 for two... 0.5 for half a turn")]
	//public float loops = 0f;
	public Vector3 rotation = Vector3.up;
	
	//private float m_loopCounter;

	void Update () {

        if (global)
        {
            transform.Rotate(rotation, Space.World);
        }
        else
        {
            transform.Rotate(rotation); //same as transform.Rotate (rotation, Space.Self);
        }
   
	}


}
