using UnityEngine;

namespace J
{
	/*
	 * 		call public method 'begin()' to follow a curve. An GameObject should be created in proyect for each curve.
	 */
	[AddComponentMenu("J/Util/JFollowCurve")]
	public class JFollowCurve : MonoBehaviour {

		public delegate void CurveDelegate(float x);

		[SerializeField]	AnimationCurve curve;
		[SerializeField]	float amplitudeFactor = 1f;
		[Range(0.01f, 60f)]
		[SerializeField]	float durationFactor = 1f;
		[Tooltip("If equal to 0, it repeats forever")]
		[SerializeField]	int repeat = 0;

		protected float timeVariable = 0f;
		protected float curveDuration;
		protected float curveStartTime;
		protected float modifyFactor;


		protected float curveValue;
		protected bool updateEnabled;
		protected bool reverse;
		protected CurveDelegate foo;





		public void begin(CurveDelegate d , float duration = 1, float amplitude = 1, int repeat = 0, CurveType type = CurveType.Linear, bool reverse = false) {
			foo = d;
			updateEnabled = true;

			if (duration != 1)
				this.durationFactor = duration;
			if (amplitude != 1)
				this.amplitudeFactor = amplitude;
			if (repeat != 0)
				this.repeat = repeat;

			this.curve = CurveGenerator.GenerateCurve (type);
			curveDuration = curve.keys [curve.length - 1].time - curve.keys [0].time;
			curveStartTime = curve.keys [0].time;

			this.reverse = reverse;
		}






		void Start () {
			curve = CurveGenerator.GenerateCurve (CurveType.Linear);
		}

		void Update () {
			if (updateEnabled) {
				float t = timeVariable + Time.deltaTime / durationFactor;

				if (repeat > 0 && t > curveDuration) {
					repeat--;
					if (repeat == 0) {
						updateEnabled = false;
						if (reverse)
							curveValue = Mathf.Min (curveValue, 0f);
						else
							curveValue = Mathf.Max (curveValue, amplitudeFactor);
					}
				}
				if (updateEnabled) {
					timeVariable = t % curveDuration;
					if (reverse)
						curveValue = curve.Evaluate (curveStartTime + curveDuration - timeVariable) * amplitudeFactor;
					else
						curveValue = curve.Evaluate (curveStartTime + timeVariable) * amplitudeFactor;
				}
				foo (curveValue);
			}
		}

	}

}