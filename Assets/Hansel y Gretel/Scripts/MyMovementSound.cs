using UnityEngine;
using System.Collections;

public class MyMovementSound : MonoBehaviour {

	[RangeAttribute(0f, 1f)]
	public float volume = 1f;

	[TooltipAttribute("Keep it low to play sounds on little movement of this gameobject")]
	[RangeAttribute(0.01f, 100f)]
	public float sensibility = 1f;

	[TooltipAttribute("Over this sensibility, the sounds wont play. Use this to avoid movement sounds when teletransporting of moving object with another script")]
	[RangeAttribute(5f, 300f)]
	public float trimSensibility = 10f;
	public AudioClip[] sounds;


	private Vector3 m_oldPosition;
	private float m_deltaDistance;
	private AudioSource m_audioSource;

	// Use this for initialization
	void Start () {
		m_oldPosition = transform.position;
		m_audioSource = gameObject.AddComponent<AudioSource> ();
		m_audioSource.playOnAwake = false;
		m_audioSource.volume = this.volume;
		InvokeRepeating ("MyCustomUpdate", 0.02f, 0.1f);
	}
	
	// Update is called once per frame
	void MyCustomUpdate () {
		m_deltaDistance = Vector3.Distance (transform.position, m_oldPosition);
		if (m_deltaDistance > sensibility  && m_deltaDistance < trimSensibility) {
			if (!m_audioSource.isPlaying)
				playRandomClip ();
		}
		m_oldPosition = transform.position;
	}

	void playRandomClip () {
		int rand = Random.Range (0, sounds.Length);
		m_audioSource.clip = sounds [rand];
		m_audioSource.Play ();
	}
}
