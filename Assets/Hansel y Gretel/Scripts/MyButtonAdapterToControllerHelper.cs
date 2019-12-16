using UnityEngine;
using System.Collections;

public class MyButtonAdapterToControllerHelper : MonoBehaviour {

	public bool doOnceOnEnter;
	public bool doOnceOnExit;

	void Start() {
		GetComponent<MyButtonAdapterToController> ().doOnceOnEnter = this.doOnceOnEnter;
		GetComponent<MyButtonAdapterToController> ().doOnceOnExit = this.doOnceOnExit;
	}

}
