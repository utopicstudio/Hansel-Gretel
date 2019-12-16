using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinalScore : MonoBehaviour
{
    public TextMeshProUGUI textFinal;    
    void Update()
    {
        textFinal.text = "¡Muchas gracias por jugar! Resultado Final: " + QuestionManager.respuestasTotales.ToString() + " de 28 respuestas correctas";
    }
}
