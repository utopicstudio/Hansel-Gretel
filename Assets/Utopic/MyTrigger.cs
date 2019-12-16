using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class MyTrigger : MonoBehaviour {

	public string tagCondition = "";
	public bool enterOnce = true;
	public bool exitOnce = true;
	public UnityEngine.Events.UnityEvent onEnter;
	public UnityEngine.Events.UnityEvent onExit;
	public UnityEngine.Events.UnityEvent onStay;

	private Collider collider;
	private bool m_enterEnabled = true, m_exitEnabled = true;

	void OnValidate() {
		tagCondition = tagCondition.Trim ();
	}

	// Use this for initialization
	void Start () {
		collider = GetComponent<Collider> ();
	}

	void OnTriggerEnter(Collider other) {
		if (!isTagValid (other.tag))
			return;
		if (m_enterEnabled) {
			onEnter.Invoke ();
		}
		if (enterOnce)
			m_enterEnabled = false;
	}
	void OnTriggerExit(Collider other) {
		if (!isTagValid (other.tag))
			return;
		if (m_exitEnabled) {
			onExit.Invoke ();
		}
		if (exitOnce)
			m_exitEnabled = false;
	}
	void OnTriggerStay(Collider other) {
		if (!isTagValid (other.tag))
			return;
		onStay.Invoke ();
	}
	private bool isTagValid(string tag) {
		if (tagCondition == "")
			return true;
		return tag == tagCondition;
	}
}