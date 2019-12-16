using UnityEngine;
using System.Collections;

public class MyButtonAdapterToController : UnityEngine.EventSystems.EventTrigger {

	public bool doOnceOnEnter;
	public bool doOnceOnExit;

	private bool isBeingLooked;
	private bool m_onceEnterFlag;
	private bool m_onceExitFlag;

	void Update() {
		if (Input.GetButtonDown ("Fire2") && isBeingLooked) {
			GetComponent<UnityEngine.UI.Button> ().onClick.Invoke ();
		}
	}

	public override void OnPointerEnter (UnityEngine.EventSystems.PointerEventData eventData)
	{
		if (doOnceOnEnter && m_onceEnterFlag) {
			return;
		}
		m_onceEnterFlag = true;
		isBeingLooked = true;
		base.OnPointerEnter (eventData);
	}
	public override void OnPointerExit (UnityEngine.EventSystems.PointerEventData eventData)
	{
		if (doOnceOnExit && m_onceExitFlag) {
			return;
		}
		m_onceExitFlag = true;
		isBeingLooked = false;
		base.OnPointerExit (eventData);
	}

}
