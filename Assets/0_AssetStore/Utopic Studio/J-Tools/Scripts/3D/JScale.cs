using UnityEngine;

namespace J
{

	[AddComponentMenu("J/3D/JScale")]
	public class JScale : MonoBehaviour {

		enum ModificationType
		{
			Additive, Multiplicative
		}

		[SerializeField]	AnimationCurve curve;
		[SerializeField]	ModificationType type;
		[SerializeField]	float scaleFactor = 1f;
		[Range(0.01f, 60f)]
		[SerializeField]	float durationFactor = 1f;

		protected float timeVariable = 0f;
		protected Vector3 initialScale;
		protected float curveDuration;
		protected float curveStartTime;
		protected float modifyFactor;

		void Start () {
			UpdateCurveInfo ();

			initialScale = transform.localScale;
		}
		void OnValidate () {
			UpdateCurveInfo ();
		}

		void Update () {
			timeVariable = (timeVariable + Time.deltaTime / durationFactor) % curveDuration;
			modifyFactor = curve.Evaluate (curveStartTime + timeVariable) * scaleFactor;
			switch (type) {
			case ModificationType.Additive:
				transform.localScale = initialScale + Vector3.one * modifyFactor;
				break;
			case ModificationType.Multiplicative:
				transform.localScale = initialScale * modifyFactor;
				break;
			}
		}

		protected void UpdateCurveInfo() {
			curveDuration = curve.keys [curve.length - 1].time - curve.keys [0].time;
			curveStartTime = curve.keys [0].time;
		}
	}


}