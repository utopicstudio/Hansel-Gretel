using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MyAudioFade : MonoBehaviour {

	public float fadeInDuration = 1f;
	public float fadeOutDuration = 1f;

	private AudioSource m_audio;
	private bool m_isFadingIn, m_isFadingOut;
	private float m_lerpVar = 0f;//must start at 0
	private bool m_fadeInAndFadeOut;
	private float m_currentPlaybackTime;
	private float m_maxVol;

	void Start () {
		m_audio = GetComponent<AudioSource> ();
		m_maxVol = m_audio.volume;
	}

	void FixedUpdate ()
	{
		if (Mathf.Abs( m_audio.time - Mathf.Round(m_audio.time) ) < 0.03f)
			print (Mathf.Round(m_audio.time));
		if (m_isFadingIn && !m_isFadingOut) {
			m_lerpVar += Time.deltaTime / fadeInDuration;
			m_audio.volume = Mathf.Lerp (0f, m_maxVol, m_lerpVar);
			if (m_lerpVar > 1f) {
				m_isFadingIn = false;
				m_lerpVar = 1f;
			}

		} else if (!m_isFadingIn && m_isFadingOut) {
			print ("fading OUT");
			m_lerpVar -= Time.deltaTime / fadeOutDuration;
			m_audio.volume = Mathf.Lerp (0f, m_maxVol, m_lerpVar);
			if (m_lerpVar < 0f) {
				m_isFadingOut = false;
				m_lerpVar = 0f;
			}
		}
		if (m_fadeInAndFadeOut && m_audio.isPlaying) {
			float timeLeft = m_audio.clip.length - m_audio.time;
			if (timeLeft < fadeOutDuration) {
				m_isFadingIn = false;
				m_isFadingOut = true;
				m_fadeInAndFadeOut = false;
			}

		}
	}

	public void FadeIn() {
		m_isFadingIn = true;
		m_isFadingOut = false;
		m_audio.Play ();
	}

	public void FadeOut() {
		m_isFadingOut = true;
		m_isFadingIn = false;
		m_audio.Play ();
	}
	public void FadeInAndOut() {
		m_fadeInAndFadeOut = true;
		FadeIn ();
	}
}