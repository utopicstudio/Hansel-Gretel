using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public class MyTapTwiceToQuitInAndroid : MonoBehaviour {
	
	[HeaderAttribute("Have only 1 object with this component")]
	[Space(8)]
	[TooltipAttribute("Maximum seconds between the two taps")]
	public float tapTime = 1.5f;
	public bool tapOnce = false;
	public GameObject activateObjectWhenTappingOnce;

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
				Application.Quit ();
			} else {
				if (activateObjectWhenTappingOnce)
					activateObjectWhenTappingOnce.SetActive (true);
			}

			if (lastCallToCoroutine != null)
				StopCoroutine (lastCallToCoroutine);
			lastCallToCoroutine = StartCoroutine (startCooldown ());

		}
	}

	IEnumerator startCooldown() {
		isDoingCooldown = true;
		yield return new WaitForSeconds (tapTime);
		isDoingCooldown = false;
		if (activateObjectWhenTappingOnce)
			activateObjectWhenTappingOnce.SetActive (false);
	}
}
