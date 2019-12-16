using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextOrderController : MonoBehaviour
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
        foreach (TextVR text in texts)
            text.Hide();
    }
}
