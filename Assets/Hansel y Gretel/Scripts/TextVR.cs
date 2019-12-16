using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextVR : MonoBehaviour
{

    public bool hidesOtherTexts = true;

    private void OnEnable()
    {
        if (hidesOtherTexts)
            ShowAndHideOthers();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void ShowAndHideOthers() { }
    public virtual void Show() { }
    public virtual void Hide() { }
}
