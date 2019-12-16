using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
public class MoveNavMeshAgentsStructed : MonoBehaviour
{
   

[System.Serializable]
public struct Personaje
{
    public NavMeshAgent agent;
    public Transform posicion;
}

    


    [Header("En el arreglo AGENTS, usa sólo los que vas a mover")]
    [Header("Nota: Llamar a este MoveAgents() max 1 vez")]
    [SerializeField] float rotationSpeedOnArrive = 10;
    [SerializeField] UnityEngine.AI.NavMeshAgent[] agents;
    [Header("Usar mismo orden que el arreglo AGENTS")]
    [SerializeField] Transform[] destinations;
    [SerializeField] UnityEngine.Events.UnityEvent OnArriveAny;
    [SerializeField] UnityEngine.Events.UnityEvent OnArriveAnyStruct;
    [SerializeField] UnityEngine.Events.UnityEvent OnArriveAll;
    [SerializeField] UnityEngine.Events.UnityEvent[] eventOnArrivePerCharacter;

    private List<bool> boolOnArrive;
    public bool llego1;
    private bool any = false;
    private bool all = false;
    private bool moveAgentsCalled = false;
    private bool eventsCalled = false;
    private bool[] eventsPerCharacter;


    public Personaje[] personaje;



    public void Start()
    {


        boolOnArrive = Enumerable.Repeat<bool>(false, personaje.Length).ToList<bool>();
        eventsPerCharacter = new bool[personaje.Length];
        for (int i = 0; i < personaje.Length; i++)
        {
            eventsPerCharacter[i] = false;
        }
        

    }
    public void Update()
    {
        if (moveAgentsCalled && !eventsCalled)
        {
            for (int i = 0; i < personaje.Length; i++)
            {
                if (personaje[i].agent && personaje[i].agent.enabled && personaje[i].agent.gameObject.activeInHierarchy && !personaje[i].agent.pathPending)
                {
                    if (personaje[i].agent.remainingDistance <= personaje[i].agent.stoppingDistance)
                    {
                        if (!personaje[i].agent.hasPath || personaje[i].agent.velocity.sqrMagnitude <= 0.2f)
                        {
                            CallEventOnArrive(i);
                        }
                    }
                }
            }
        }
        //public void Update()
        //{
        //    if (moveAgentsCalled && !eventsCalled)
        //    {
        //        for (int i = 0; i < agents.Length; i++)
        //        {
        //            if (agents[i] && agents[i].enabled && agents[i].gameObject.activeInHierarchy && !agents[i].pathPending)
        //            {
        //                if (agents[i].remainingDistance <= agents[i].stoppingDistance)
        //                {
        //                    if (!agents[i].hasPath || agents[i].velocity.sqrMagnitude <= 0.2f)
        //                    {
        //                        CallEventOnArrive(i);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }
    public void MoveAgents()
    {
        //Debug.Log("muevo agentes");
        moveAgentsCalled = true;
        if (personaje.Length <= personaje.Length)
        {
            for (int i=0; i< personaje.Length; i++)
            {
                if (personaje[i].agent.gameObject.activeInHierarchy && personaje[i].agent.isActiveAndEnabled)
                    //agents[i].SetDestination(destinations[i].position);
                    personaje[i].agent.SetDestination(personaje[i].posicion.position);
            }
            //personaje1.agent[i].SetDestination(personaje1.posicion.position);
            //personaje2.agent.SetDestination(personaje2.posicion.position);
        }
    }
    public void MoveAgentsInstantly()
    {
        moveAgentsCalled = true;
        if (personaje.Length <= destinations.Length)
        {
            for (int i = 0; i < personaje.Length; i++)
            {
                if (personaje[i].agent.gameObject.activeInHierarchy && personaje[i].agent.isActiveAndEnabled)
                {
                    personaje[i].agent.ResetPath();
                    personaje[i].agent.Warp(destinations[i].position);
                }
            }
        }
        
    }
    //public void CallEventOnArriveStruct()
    //{
      
    //    llego1 = true;
    //    if (llego1 == true)
    //    {
    //        Debug.Log("funciona el struct");

    //        if (!any)
    //            OnArriveAnyStruct.Invoke();
    //        any = true;
    //    }
    //}



        public void CallEventOnArrive(int i)
    {
        RotateAgents(i);
        
        this.boolOnArrive[i] = true;
        if (boolOnArrive.Any<bool>(b => b == true))
        {
            if (!any)
                OnArriveAny.Invoke();
            any = true;
        }
        if (boolOnArrive.All<bool>(b => b == true))
        {
            if (!all)
                OnArriveAll.Invoke();
            all = true;
        }
        if (i < eventOnArrivePerCharacter.Length && !eventsPerCharacter[i])
        {
            eventOnArrivePerCharacter[i].Invoke();
            eventsPerCharacter[i] = true;
        }
    }
    
    private void RotateAgents(int i)
    {
        /*
        if (i < destinations.Length)
        {
            Vector3 lookPos = destinations[i].forward;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            //J.J.instance.followCurve(x => agents[i].transform.rotation = Quaternion.Slerp(agents[i].transform.rotation, rotation, x), 10 * rotationSpeedOnArrive / agents[i].angularSpeed);
        }
        */
    }
}
