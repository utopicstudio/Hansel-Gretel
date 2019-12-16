using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(FadeParticleSystem))]
public class MyTriggerForCircle : MonoBehaviour {

	public bool isEnabled = true;
	public bool enterOnce;
	public UnityEngine.Events.UnityEvent onEnter;
	public bool exitOnce;
	public UnityEngine.Events.UnityEvent onExit;
	public UnityEngine.Events.UnityEvent onStay;

	private Collider collider;
	private bool m_enterEnabled = true, m_exitEnabled = true;
	private FadeParticleSystem fadeParticleSystem;

	// Use this for initialization
	void Start () {
		collider = GetComponent<Collider> ();
	}

	void OnTriggerEnter() {
		if (isEnabled) {
			if (m_enterEnabled) {
				onEnter.Invoke ();
			}
			if (enterOnce)
				m_enterEnabled = false;
		}
	}
	void OnTriggerExit() {
		if (isEnabled) {
			if (m_exitEnabled) {
				onExit.Invoke ();
			}
			if (exitOnce)
				m_exitEnabled = false;
		}
	}
	void OnTriggerStay() {
		if (isEnabled) {
			onStay.Invoke ();
		}
	}

	public void SetEnabled(bool b) {
		this.isEnabled = b;
	}
}
