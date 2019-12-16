using UnityEngine;

namespace J
{

    /// <summary>
    /// Usar J.followCurve para hacer Lerp fuera de la función Update()
    /// 
    /// Esta clase es de uso interno para realizar Lerp y debería ser
    /// llamada a travéz de J.JAutomaticLerp()
    /// </summary>
    public class JFollowCurve2 : MonoBehaviour
    {

		public delegate void CurveDelegate(float x);

		[SerializeField]	AnimationCurve curve;
		[SerializeField]	float amplitudeFactor = 1f;
		[SerializeField]	int repeatCounter;
        [SerializeField]    float timeProgress;
        [SerializeField]    float curveCurrentTime;
        [SerializeField]    float curveCurrentValue;
        [SerializeField]    string createdBy;


        protected float duration = 1f;
		protected float curveRealDuration;
		protected float curveStartTime;
		protected float modifyFactor;


		protected float curveValue;
		protected bool updateEnabled;
		protected bool reverse;
		protected CurveDelegate curve_delegate;


        private float t;


		public void beginFollowCurve(CurveDelegate d , float duration, float amplitude, int repeat, CurveType type, bool reverse, MonoBehaviour callingScript) {

            t = 0f;
            curve_delegate = d;
            updateEnabled = true;

			this.amplitudeFactor = amplitude;
            if (callingScript)
                this.createdBy = callingScript.name + " (Component " + callingScript.GetType().Name + ")";
			
			this.repeatCounter = repeat;

			this.curve = CurveGenerator.GenerateCurve (type);
            
			curveRealDuration = curve.keys [curve.length - 1].time - curve.keys [0].time;
			curveStartTime = curve.keys [0].time;

			this.reverse = reverse;

            this.duration = duration;

            
		}






		private void Start () {
			curve = CurveGenerator.GenerateCurve (CurveType.Linear);
		}

		private void Update () {
			if (updateEnabled)
            {
                // 0 <= t <= 1
				t = Mathf.Clamp01(t + (Time.deltaTime / duration));

                float t_curve = t * curveRealDuration;

                //print("curveRealDuration="+curveRealDuration+"   t="+t);
				if (reverse)
					curveValue = curve.Evaluate (curveStartTime + curveRealDuration - t_curve) * amplitudeFactor;
				else
					curveValue = curve.Evaluate (curveStartTime + t_curve) * amplitudeFactor;


                
                curve_delegate(curveValue);



                this.curveCurrentValue = curveValue;
                this.curveCurrentTime = t_curve;
                this.timeProgress = t * this.duration;

                if (repeatCounter > 0 && t == 1f)
                {
                    repeatCounter--;
                }
                if (repeatCounter == 0)
                {
                    updateEnabled = false;
                }
            }
		}

	}

}