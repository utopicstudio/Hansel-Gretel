using UnityEngine;
using System.Collections;

public class FadeParticleSystem : MonoBehaviour {

	enum FadeParticleSystemStartMode {invisible, visible, visibleFadeIn}
	[SerializeField]
	FadeParticleSystemStartMode startVisibility = FadeParticleSystemStartMode.visible;


	public float time;
	public AudioClip soundOnShow;
	public AudioClip soundOnHide;

	private ParticleSystem particle;
	private ParticleSystem[] particlesInChildren;
	private Color color;
	private Color[] colorsInChildren;

	void Start () {
		particle = GetComponent<ParticleSystem> ();
		color = particle.startColor;
		particlesInChildren = GetComponentsInChildren<ParticleSystem> ();
		colorsInChildren = new Color[particlesInChildren.Length];
		for (int i=0; i<particlesInChildren.Length; i++) {
			colorsInChildren [i] = particlesInChildren [i].startColor;
		}


		switch (startVisibility) {
		case FadeParticleSystemStartMode.invisible:
			HideInstantly ();
			break;
		case FadeParticleSystemStartMode.visible:
			ShowInstantly ();
			break;
		case FadeParticleSystemStartMode.visibleFadeIn:
			Show ();
			break;
		}

	}
	






	public void Show() {
		playSound (soundOnShow);
		StartCoroutine (ShowPrivate ());
	}
	public void Hide() {
		playSound (soundOnHide);
		StartCoroutine (HidePrivate ());
	}
	public void ShowInstantly() {
		playSound (soundOnShow);
		particle.startColor = color;
		for (int i=0; i<particlesInChildren.Length; i++) {
			particlesInChildren[i].startColor = colorsInChildren[i];
		}
	}
	public void HideInstantly() {
		playSound (soundOnHide);
		particle.startColor = Color.clear;
		for (int i=0; i<particlesInChildren.Length; i++) {
			particlesInChildren [i].startColor = Color.clear;
		}
	}
		
	private IEnumerator ShowPrivate(){
		if (!particle.isPlaying)
			particle.Play();
		for (float i = 0f; i < time;i += Time.deltaTime) {

			particle.startColor = Color.Lerp (Color.clear, color, i/time);
			for (int j=0; j<particlesInChildren.Length; j++) {
				particlesInChildren[j].startColor = Color.Lerp (Color.clear, colorsInChildren[j], i/time);
			}
			yield return new WaitForEndOfFrame ();
		}

	}	
	private IEnumerator HidePrivate(){

		for (float i = 0f; i < time;i += Time.deltaTime) {

			particle.startColor = Color.Lerp (color, Color.clear, i/time);
			for (int j=0; j<particlesInChildren.Length; j++) {
				particlesInChildren[j].startColor = Color.Lerp (colorsInChildren[j], Color.clear, i/time);
			}
			yield return new WaitForEndOfFrame ();
		}

	}

	private void playSound(AudioClip sound) {
		if (sound)
			AudioSource.PlayClipAtPoint (sound, transform.position);
	}















	public void FadeInParticle(){
	
		StartCoroutine (FadeIn ());

	}
	IEnumerator FadeIn(){
		if (!particle.isPlaying)
			particle.Play();
		for (float i = 0f; i < time;i += Time.deltaTime) {

			//i += Time.deltaTime;

			particle.startColor = Color.Lerp (Color.clear, color, i/time);
			yield return new WaitForEndOfFrame ();
		}
	
	}

	public void FadeOutParticle(){
	
		StartCoroutine (FadeOut ());

	}
	IEnumerator FadeOut(){
	
		for (float i = 0f; i < time;i += Time.deltaTime) {

			//i += Time.deltaTime;

			particle.startColor = Color.Lerp (color, Color.clear, i/time);
			yield return new WaitForEndOfFrame ();
		}
	
	}











}
