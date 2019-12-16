using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    public Animator Fade;
    void Start()
    {
        Fade.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeOut() {
        Fade.gameObject.SetActive(true);
        Fade.SetTrigger("FadeOut");
        //Fade.gameObject.SetActive(false);
    }
    public void FadeIn()
    {
        Fade.gameObject.SetActive(true);
        Fade.SetTrigger("FadeIn");
        StartCoroutine("Desactivo");
    }
    IEnumerator Desactivo() {
        yield return new WaitForSeconds(2);
        Fade.gameObject.SetActive(false);
    }
}

