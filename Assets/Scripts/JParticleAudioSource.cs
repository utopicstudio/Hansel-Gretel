using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JParticleAudioSource : MonoBehaviour
{
  
    public AudioSource audioSource;
    public ParticleSystem particleSystemPlayer;
    public float ResetTime =3f;
  
    bool played;
   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     

        if (particleSystemPlayer.isPlaying)
        {
    
            if (!audioSource.isPlaying && !played)
            {
                CancelInvoke("ResetAudio");
                audioSource.Play();
                played = true;
                Invoke("ResetAudio", ResetTime);
            }
           
        }
    }
    void ResetAudio()
    {
        played = false;
    }
}
