using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
    public TextVR[] texts;
    
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;
    }
    
    public void HideAll()
    {
        foreach (var elem in texts)
        {
            elem.Hide();
        }
    }
    /*
    public void Previous()
    {
        texts[currentIndex].Hide();
        currentIndex = (currentIndex - 1) % texts.Length;
        texts[currentIndex].Show();
    }
    public void Next()
    {
        texts[currentIndex].Hide();
        string msg = "desactivado: " + texts[currentIndex];

        currentIndex = (currentIndex + 1) % texts.Length;
        texts[currentIndex].Show();
        msg += "\tactivado: " + texts[currentIndex];

        Debug.Log(msg);
    }
    */
}
