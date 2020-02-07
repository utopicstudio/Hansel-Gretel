using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptorManager : MonoBehaviour
{
    public Renderer MeshChangeTexture;//
    //public Material[] Luces;
    Texture textureChange;
    Texture originalTexture;
    public Material[] materiales;
    public Renderer[] render;

    public bool[] booleanos;
    int contador;


    [System.Serializable]
    struct JActionStruct
    {
        public float delay;
        public UnityEngine.Events.UnityEvent action;
    }

    [Tooltip("Para que las acciones se llamen al iniciar la escena")]
    [SerializeField] bool callOnStart = false;
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


    void Start()
    {
    if (callOnStart)
        CallActions();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CheckComplete() {
        contador = 0;
        for (int i = 0; i < booleanos.Length; i++) {
            if (booleanos[i]) {
                contador++;
                if (contador == 4) {
                    action.Invoke();
                    //Debug.Log("gane");
                }
            }
        }

       
    }
}


