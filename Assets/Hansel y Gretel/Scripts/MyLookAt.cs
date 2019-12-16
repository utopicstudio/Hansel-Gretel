using UnityEngine;
using System.Collections;

public class MyLookAt : MonoBehaviour {

	public Transform target;
	public bool verticalLook;
	[RangeAttribute(1f,50f)]
	public float speed = 20f;

	Vector3 dir;

	
	// Update is called once per frame
	void Update () {
		dir = target.position - transform.position;
		if (!verticalLook)
			dir.y = 0;
		Quaternion rotation = Quaternion.LookRotation(dir);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
			Time.deltaTime * speed);
		//transform.LookAt(target, Vector3.up);
	}

}
