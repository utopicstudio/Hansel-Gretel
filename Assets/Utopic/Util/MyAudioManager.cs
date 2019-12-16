﻿using UnityEngine;
using System.Collections;

public class MyAudioManager : MonoBehaviour {

	public AudioClip musicClip;
	[Range(0f,1f)] public float musicVolume = 1f;
	public bool playOnAwake = true;
	public bool loop = true;

	private AudioSource musicComponent;




	void Awake () {
		musicComponent = gameObject.AddComponent<AudioSource> ();
		musicComponent.playOnAwake = false;
		setMemberVariables ();
	}
	void Start () {
		if (playOnAwake && musicClip)
			PlayMusic (musicClip);
	}

	void OnValidate () {
		if (musicComponent)
			setMemberVariables ();
	}


	public void ChangeVolume (float newVolume, bool fade = true) {

		this.musicVolume = newVolume;
		if (fade) {
			LerpVolume (musicComponent.volume, newVolume, 1f);
		} else {
			musicComponent.volume = newVolume;
		}
	}


	public void Play2DSound(AudioClip audioClip, float volume = 1f) {
		GameObject go = null;
		AudioSource audioSource = null;

		go = new GameObject ("my2DSound");
		audioSource = go.AddComponent<AudioSource> ();
		audioSource.clip = audioClip;

		audioSource.volume = volume;
		audioSource.Play ();

		Destroy (go, audioClip.length + 1f);
	}

	public void PlayMusic(AudioClip musicClip, float volume = -1f, float fadeIn = 0f, float fadeOut = 0f) {
		

		StartCoroutine (_PlayMusic (musicClip, volume, fadeIn, fadeOut));
	}
	private IEnumerator _PlayMusic(AudioClip musicClip, float volume, float fadeIn, float fadeOut) {
		if (fadeOut > 0f) {
			LerpVolume (this.musicVolume, 0f, fadeOut);
			yield return new WaitForSeconds (fadeOut);
		}
		//if (musicComponent.isPlaying)
		//musicComponent.Stop ();

		if (volume != -1f) {
			this.musicVolume = volume;
			musicComponent.volume = this.musicVolume;
		}
		this.musicClip = musicClip;
		musicComponent.clip = musicClip;
		LerpVolume (0f, this.musicVolume, fadeIn);

		musicComponent.Play ();
	}

	public void StopMusic(float fadeOut = 0f) {
		LerpVolume(this.musicVolume, 0f, fadeOut);
		Invoke ("_StopMusic", fadeOut);
	}
	public void PauseMusic(float fadeOut = 0f) {
		LerpVolume(this.musicVolume, 0f, fadeOut);
		Invoke ("_PauseMusic", fadeOut);
	}
	public void UnPauseMusic(float fadeIn = 0f) {
		if (!musicComponent.isPlaying) {
			LerpVolume(0f, this.musicVolume, fadeIn);
			Invoke ("_UnPauseMusic", fadeIn);
		}
	}
	private void _StopMusic() {
		musicComponent.Stop ();
	}
	private void _PauseMusic() {
		musicComponent.Pause ();
	}
	private void _UnPause() {
		musicComponent.UnPause ();
	}

	private void LerpVolume (float startVal, float endVal, float time) {
		//GameObject.FindObjectOfType<LerpFactory.LerpFactoryScript>().Float ((x) => musicComponent.volume = x, startVal, endVal, time);
        musicComponent.volume = endVal;

    }

	private void setMemberVariables () {
		musicComponent.loop = this.loop;
		musicComponent.volume = this.musicVolume;
	}
}
