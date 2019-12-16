using UnityEngine;

namespace J
{

	[AddComponentMenu("J/Util/JQuitGameTapTwice")]
	public class JQuitGameTapTwice : MonoBehaviour {
		
		[TooltipAttribute("Maximum seconds between the two taps")]
		[SerializeField]	float tapTime = 1.5f;
		[SerializeField]	bool tapOnce = false;
		[SerializeField]	GameObject activateObjectWhenTappingOnce;
		[SerializeField]	bool worksInEditor = false;

		private bool isDoingCooldown = false;
		private Coroutine lastCallToCoroutine;

		void Awake () {
			if (activateObjectWhenTappingOnce)
				activateObjectWhenTappingOnce.SetActive (false);
		}

		// Update is called once per frame
		void Update () {

			if (Input.GetKeyDown (KeyCode.Escape)) {
				if (isDoingCooldown || tapOnce) {
					this.Quit ();
				} else {
					if (activateObjectWhenTappingOnce)
						activateObjectWhenTappingOnce.SetActive (true);
				}

				if (lastCallToCoroutine != null)
					StopCoroutine (lastCallToCoroutine);
				lastCallToCoroutine = StartCoroutine (startCooldown ());

			}
		}

		System.Collections.IEnumerator startCooldown() {
			isDoingCooldown = true;
			yield return new WaitForSeconds (tapTime);
			isDoingCooldown = false;
			if (activateObjectWhenTappingOnce)
				activateObjectWhenTappingOnce.SetActive (false);
		}

		public void Quit () {
			Application.Quit ();
#if UNITY_EDITOR
            if (worksInEditor) {
				UnityEditor.EditorApplication.isPlaying = false;
			}
#endif
        }
	}

}