using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
public class KMoveNavMeshAgentsStructed : MonoBehaviour
{
   

[System.Serializable]
public struct Personaje
{
    public NavMeshAgent agent;
    public Transform posicion;
}

    [Header("Colocar la cantidad de personajes")]
    public Personaje[] Personajes;

    //[Header("En el arreglo AGENTS, usa sólo los que vas a mover")]
    //[Header("Nota: Llamar a este MoveAgents() max 1 vez")]
    //[SerializeField] float rotationSpeedOnArrive = 10;
    //[SerializeField] UnityEngine.AI.NavMeshAgent[] agents;
    //[Header("Usar mismo orden que el arreglo AGENTS")]
    //[SerializeField] Transform[] destinations;
    [Header("Colocar los eventos")]
    [SerializeField] UnityEngine.Events.UnityEvent OnArriveAny;
    //[SerializeField] UnityEngine.Events.UnityEvent OnArriveAnyStruct;
    [SerializeField] UnityEngine.Events.UnityEvent OnArriveAll;
    [SerializeField] UnityEngine.Events.UnityEvent[] eventOnArrivePerCharacter;

    private List<bool> boolOnArrive;
    private bool any = false;
    private bool all = false;
    private bool moveAgentsCalled = false;
    private bool eventsCalled = false;
    private bool[] eventsPerCharacter;


    



    public void Start()
    {


        boolOnArrive = Enumerable.Repeat<bool>(false, Personajes.Length).ToList<bool>();
        eventsPerCharacter = new bool[Personajes.Length];
        for (int i = 0; i < Personajes.Length; i++)
        {
            eventsPerCharacter[i] = false;
        }
        

    }
    public void Update()
    {
        if (moveAgentsCalled && !eventsCalled)
        {
            for (int i = 0; i < Personajes.Length; i++)
            {
                if (Personajes[i].agent && Personajes[i].agent.enabled && Personajes[i].agent.gameObject.activeInHierarchy && !Personajes[i].agent.pathPending)
                {
                    if (Personajes[i].agent.remainingDistance <= Personajes[i].agent.stoppingDistance)
                    {
                        if (!Personajes[i].agent.hasPath || Personajes[i].agent.velocity.sqrMagnitude <= 0.2f)
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
        if (Personajes.Length <= Personajes.Length)
        {
            for (int i=0; i< Personajes.Length; i++)
            {
                if (Personajes[i].agent.gameObject.activeInHierarchy && Personajes[i].agent.isActiveAndEnabled)
                    //agents[i].SetDestination(destinations[i].position);
                    Personajes[i].agent.SetDestination(Personajes[i].posicion.position);
            }
            //personaje1.agent[i].SetDestination(personaje1.posicion.position);
            //personaje2.agent.SetDestination(personaje2.posicion.position);
        }
    }
    public void MoveAgentsInstantly()
    {
        moveAgentsCalled = true;
        if (Personajes.Length <= Personajes.Length)
        {
            for (int i = 0; i < Personajes.Length; i++)
            {
                if (Personajes[i].agent.gameObject.activeInHierarchy && Personajes[i].agent.isActiveAndEnabled)
                {
                    Personajes[i].agent.ResetPath();
                    Personajes[i].agent.Warp(Personajes[i].posicion.position);
                }
            }
        }
        
    }
 


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
        if (i < Personajes.Length)
        {
            Vector3 lookPos = Personajes[i].posicion.forward;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            J.J.Instance.followCurve(x => Personajes[i].agent.transform.rotation = Quaternion.Slerp(Personajes[i].agent.transform.rotation, rotation, x), 10 * rotationSpeedOnArrive / Personajes[i].agent.angularSpeed);
        }
        */
    }
}
