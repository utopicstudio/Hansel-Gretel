using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOscilate : MonoBehaviour {


	enum AnimationCurveType
	{
		Sine, Custom
	}
	[SerializeField]
	AnimationCurveType curveType;
	public AnimationCurve scaleCurve;
	public float scaleAmmount = 1f;
	public float duration = 1f;

	private float timeVariable = 0f;
	private Vector3 initialScale;

	void Start () {
		InitializeScaleCurve ();

		initialScale = transform.localScale;
	}
	void OnValidate () {
		InitializeScaleCurve ();
	}

	void Update () {
		timeVariable = (timeVariable + Time.deltaTime) % duration;
		transform.localScale = initialScale + Vector3.one * scaleCurve.Evaluate (timeVariable);
	}

	private void InitializeScaleCurve() {
		switch (curveType) {
		case AnimationCurveType.Sine:
			float tangentAngle = scaleAmmount / (duration * 0.1666f);
			scaleCurve = new AnimationCurve ();
			scaleCurve.AddKey (new Keyframe (0f, 0f, -tangentAngle, tangentAngle));
			scaleCurve.AddKey (new Keyframe (duration * 1 / 4, scaleAmmount));
			scaleCurve.AddKey (new Keyframe (duration * 3 / 4, -scaleAmmount));
			scaleCurve.AddKey (new Keyframe (duration, 0f, tangentAngle, -tangentAngle));
			break;
		case AnimationCurveType.Custom:
			duration = scaleCurve.keys [scaleCurve.length - 1].time;
			break;
		default:
			break;
		}


	}
}
