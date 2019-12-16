using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternativeButtonEvent : MonoBehaviour
{

    public J.JRenderOption_Alternative jRenderOption;
    public string resourceUIName = "ResourceUI";
    public float delay = 0f;

    private Transform t;

    // Start is called before the first frame update
    void Start()
    {
        if (!jRenderOption)
            jRenderOption = GetComponent<J.JRenderOption_Alternative>();
        jRenderOption.OnAnswerValueChange += DoOnAlternativeSelect;
    }

    void DoOnAlternativeSelect()
    {
        t = transform;
        Debug.Log(t.name);
        while (t.name != resourceUIName)
        {
            t = t.parent;
            Debug.Log(t.name);
        }
        t = t.parent;

        // Hide TextWeb UI
        //Invoke("hideUI", delay);
        
    }
    private void hideUI()
    {
        t.GetComponent<J.JResource>().Hide();
    }
}
