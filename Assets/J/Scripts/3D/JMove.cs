using UnityEngine;

namespace J
{
		
	[AddComponentMenu("J/3D/JMove")]
	public class JMove : MonoBehaviour {

		enum ModificationType
		{
			Local, Global
		}

		[SerializeField]	AnimationCurve curve;
		[SerializeField]	ModificationType type;
		[SerializeField]	Vector3 direction = Vector3.forward;
		[SerializeField]	float moveFactor = 1f;
		[Range(0.01f, 60f)]
		[SerializeField]	float durationFactor = 1f;

		protected float timeVariable = 0f;
		protected Vector3 initialPosition;
		protected float curveDuration;
		protected float curveStartTime;
		protected float modifyFactor;
        public bool loop;

		void Start () {
			UpdateCurveInfo ();

			initialPosition = transform.position;
		}
		void OnValidate () {
			UpdateCurveInfo ();
		}

		void Update () {

			timeVariable = (timeVariable + Time.deltaTime / durationFactor) % curveDuration;
			modifyFactor = curve.Evaluate (curveStartTime + timeVariable) * moveFactor;


			switch (type) {
			case ModificationType.Local:
				transform.position = initialPosition + transform.TransformVector (Vector3.Normalize (direction)) * modifyFactor;
				break;
			case ModificationType.Global:
				transform.position = initialPosition + Vector3.Normalize(direction) * modifyFactor;
				break;
			}
		}

		protected void UpdateCurveInfo() {
			curveDuration = curve.keys [curve.length - 1].time - curve.keys [0].time;
			curveStartTime = curve.keys [0].time;
		}
	}


}