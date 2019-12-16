using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshAnimatorSpeedLink : MonoBehaviour
{

    public Animator animator;
    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    
    void Update()
    {
        if (navMeshAgent && animator)
            animator.SetFloat("Speed", navMeshAgent.desiredVelocity.magnitude);
    }
    
}
