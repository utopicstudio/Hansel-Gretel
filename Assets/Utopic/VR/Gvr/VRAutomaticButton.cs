using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.EventSystems.EventTrigger))]
public class VRAutomaticButton : MonoBehaviour, UnityEngine.EventSystems.IPointerEnterHandler, UnityEngine.EventSystems.IPointerExitHandler {

	[Range(0.1f, 10f)]
	public float waitTime = 2.5f;
	public AudioClip clickSound;
	[Range(0f, 1f)]
	public float vol = 0.7f;

	private float waitTimePrivate = 0f;
	private UnityEngine.EventSystems.EventTrigger eventTriggerComponent;

	// Use this for initialization
	void Start () {
		eventTriggerComponent = GetComponent<UnityEngine.EventSystems.EventTrigger> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (waitTimePrivate > 0f) {
			waitTimePrivate -= Time.deltaTime;
			if (waitTimePrivate <= 0f) {
				ButtonClickAfterCooldown ();
			}
		}
	}

	#region IPointerEnterHandler implementation

	public void OnPointerEnter (UnityEngine.EventSystems.PointerEventData eventData)
	{
		waitTimePrivate = waitTime;
	}

	#endregion

	#region IPointerExitHandler implementation

	public void OnPointerExit (UnityEngine.EventSystems.PointerEventData eventData)
	{
		waitTimePrivate = 0f;
	}

	#endregion


	private void ButtonClickAfterCooldown() {
		eventTriggerComponent.OnPointerClick (null);
		PlayClickSound (clickSound);
	}
	private void PlayClickSound (AudioClip clip) {
		MyManager.Instance.audioManager.Play2DSound (clip, vol);
	}

}
