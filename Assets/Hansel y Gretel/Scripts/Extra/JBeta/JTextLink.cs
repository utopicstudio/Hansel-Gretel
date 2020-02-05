using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JTextLink : MonoBehaviour
{
    [Header("Este texto se pone sobre uno o ambos objetos referenciados")]
    [TextAreaAttribute]
    public string content;
    public UnityEngine.UI.Text text;
    public TMPro.TextMeshProUGUI textPro;
    [Tooltip("False, se llama una vez en Start(). True, se actualiza en OnValidate()")]
    public bool update = false;

    private void Start()
    {
        JUpdateText();
    }

    private void OnValidate()
    {
        if (update)
        {
            JUpdateText();
        }
    }
    public void JUpdateText()
    {
        if (text)
            text.text = content;
        if (textPro)
            textPro.text = content;
    }
}
