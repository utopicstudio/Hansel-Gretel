using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class MyUIImageColor : MonoBehaviour {

	public bool active = true;
	public bool useThisAColor = false;
	public Color colorA;
	public Color colorB;
	public float loopTime = 1.5f;
	//[RangeAttribute(0f,1f)]
	//public float AVsB;

	private UnityEngine.UI.Image img;
	private bool aToB;
	private float lerpVar;


	// Use this for initialization
	void Start () {
		img = GetComponent<UnityEngine.UI.Image> ();
		if (!useThisAColor) {
			colorA = img.color;
		}
		aToB = true;
	}

	void Update () {
		if (active) {
			if (aToB) {
				lerpVar += Time.deltaTime / loopTime;
				img.color = Color.Lerp (colorA, colorB, lerpVar);
				if (lerpVar >= 1f) {
					aToB = false;
					lerpVar = 0f;
				}
			} else {
				lerpVar += Time.deltaTime / loopTime;
				img.color = Color.Lerp (colorB, colorA, lerpVar);
				if (lerpVar >= 1f) {
					aToB = true;
					lerpVar = 0f;
				}
			}
		}
	}

	public void setActive(bool b) {
		active = b;
	}
}
