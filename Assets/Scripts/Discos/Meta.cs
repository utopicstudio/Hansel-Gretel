using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Meta : MonoBehaviour
{
    public Slider slide;
    public bool looked;
    public bool activado;
    public GameObject[] botones;
    //public ParticleSystem particles;

    void Update()
    {

        if (looked)
        {
            slide.value += Time.deltaTime;
           
        }
        else
        {
            slide.value -= Time.deltaTime;
           
        }

        if (!activado)
        {
            if (slide.value == 1)
            {
                CallActions();
                activado = true;
                //particles.Play();
                for (int i = 0; i < botones.Length; i++) {
                    botones[i].SetActive(false);
                }
            }
        }
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
}
