using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathFollow : MonoBehaviour {

	public Transform[] path;
	public float speed = 5.0f;
	public float reachDistance = 1.0f;
	public int currentPoint = 0;
    bool move = true;
    public float waitTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame

	//Por ahora el AI sólo sigue un arreglo de puntos en el espacio, se mueve a una velocidad constante entre ellos
	void Update () {
        //Vector3 dir =  path [currentPoint].position - transform.position;

        if (move)
        {
            float dist = Vector3.Distance(path[currentPoint].position, transform.position);

            transform.position = Vector3.MoveTowards(transform.position, path[currentPoint].position, Time.deltaTime * speed);

            if (dist <= reachDistance) //agregar condiciones
            {
                //currentPoint = Random.Range(1,6); // cambiar por currentPoint +1;
                currentPoint = currentPoint++;
            }
            if (currentPoint == 5) {
                currentPoint = 1;
            }
 
        }

		if (currentPoint >= path.Length) {
            currentPoint = 0;
            //move = false;
        }


	}
		
}
