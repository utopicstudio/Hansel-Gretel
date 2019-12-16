using UnityEngine;
using System.Collections;

public class EndGame : MonoBehaviour {

	public float waitBeforeEnd;
	public float fadeDuration;
	public Color color;


	private CameraFade fadeComponent;

	// Use this for initialization
	void Start () {
		fadeComponent = gameObject.AddComponent<CameraFade> ();
	}

	public void fadeOutToNextScene() {
		Invoke ("fadeOut_private", waitBeforeEnd);
	}
	private void fadeOut_private() {
		GameObject go = GameObject.Find ("_Audio");
		foreach (var audio in go.GetComponentsInChildren<MyAudioFade>()) {
			audio.FadeOut ();
		}/*
		GameObject.Find ("noche (1)").GetComponent<MyAudioFade> ().FadeOut ();
		GameObject.Find ("noche (2)").GetComponent<MyAudioFade> ().FadeOut ();
		GameObject.Find ("noche (3)").GetComponent<MyAudioFade> ().FadeOut ();
		GameObject.Find ("noche (4)").GetComponent<MyAudioFade> ().FadeOut (); */

		fadeComponent.StartFade (color, fadeDuration);
		Invoke ("changeScene", fadeDuration - GameObject.Find ("ManagerOfScenes").GetComponent<ManagerOfScenes> ().loadDelay);
	}
	private void changeScene() {
		GameObject.Find ("ManagerOfScenes").GetComponent<ManagerOfScenes> ().NextScene ();
	}
}
