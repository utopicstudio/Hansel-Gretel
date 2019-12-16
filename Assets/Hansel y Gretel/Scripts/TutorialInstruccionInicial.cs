using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIAlpha))]
public class TutorialInstruccionInicial : MonoBehaviour {

	public UIAlpha firstInstruction;
	public float durationOfInstruction = 3f;

	// Use this for initialization
	void Start () {
		Invoke ("doThis", durationOfInstruction);
	}
	
	// Update is called once per frame
	void doThis () {
		firstInstruction.Show ();
		this.GetComponent<UIAlpha> ().Hide ();
	}
}
