using UnityEngine;
using System.Collections;

public class MyCameraFade2ndTechnique : MonoBehaviour {

	public float fadeOutDuration = 2.5f;
	public Color color;

	private CameraFade m_cameraFade;

	// Use this for initialization
	void Start () {
		m_cameraFade = gameObject.AddComponent<CameraFade> ();
	}

	public void FadeIn() {
		// ???
	}
	public void FadeOut() {
		m_cameraFade.StartFade (color, fadeOutDuration);
	}
}
