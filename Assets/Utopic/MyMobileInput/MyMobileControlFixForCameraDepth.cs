using UnityEngine;
using System.Collections;

public class MyMobileControlFixForCameraDepth : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("fix", 0.2f);
	}

	void fix () {
		GetComponent<Camera> ().depth = 0f;
	}
}
