using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Puntaje : MonoBehaviour
{
    public int puntos;
    public int puntosMax;
    public bool activado;
    public TextMeshProUGUI texto;
    public AudioSource audio;

    public void Checker()
    {
        if (!activado)
        {

            if (puntos == puntosMax)
            {
                Debug.Log("puntobreak");
                CallActions();
                activado = true;
            }
        }
    }

    public void PointSound() {
        audio.Play();
    }



    [System.Serializable]
    struct JActionStruct
    {
        public float delay;
        public UnityEngine.Events.UnityEvent action;
    }

    [SerializeField] UnityEngine.Events.UnityEvent action;
    [SerializeField] JActionStruct[] delayedActions;


    private void Reset()
    {
        delayedActions = new JActionStruct[1];
        delayedActions[0].delay = 1f;
    }

    public void CallActions()
    {
        action.Invoke();

        foreach (var jActionStruct in delayedActions)
        {
            StartCoroutine(_callAction(jActionStruct.action, jActionStruct.delay));
        }
    }

    IEnumerator _callAction(UnityEngine.Events.UnityEvent a, float delay)
    {
        yield return new WaitForSeconds(delay);
        a.Invoke();
    }

    private void Update() {
        texto.text = puntos.ToString();
    }
}
