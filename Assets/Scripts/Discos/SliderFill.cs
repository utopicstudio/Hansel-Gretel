using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderFill : MonoBehaviour
{   
    public Slider slide;
    public Slider slide2;
    public bool looked;
    public bool locked=false;
    //public GameObject trigger;
    public ParticleSystem particle;

    void Update()
    {
        
            if (looked)
            {
                slide.value += Time.deltaTime;
                if (slide.value == 1)
                {
                    if (!particle.isPlaying)
                    {
                        particle.Play();
                    }

                    slide2.value += Time.deltaTime;
                    if (slide2.value == 1)
                    {
                        //trigger.SetActive(true);
                    }
                }
            }
            else
            {
            if (!locked)
            {
                //trigger.SetActive(false);
                slide2.value -= Time.deltaTime;
                if (slide2.value == 0)
                {
                    if (particle.isPlaying)
                    {
                        particle.Stop();
                    }
                    particle.Stop();
                    slide.value -= Time.deltaTime;
                }
            }

        }
    }
}
