using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    //claudio inostroza

    public bool OnSwitch;
    public Light lightSource;
    float Intensity;
    public float IntensityMax;
    public float VelocityFade = 1;

    // Turn ON /OFF the light for events
    public void OnLight()
    {
        if (OnSwitch)
        {
            OnSwitch = false;
        }
        else
        {
            OnSwitch = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        Intensity = Mathf.Clamp(Intensity, 0, IntensityMax);
        lightSource.intensity = Intensity;

        if (OnSwitch)
        {
            Intensity += Time.deltaTime * VelocityFade;
        }
        else
        {
            Intensity -= Time.deltaTime * VelocityFade;
        }
    }
}
