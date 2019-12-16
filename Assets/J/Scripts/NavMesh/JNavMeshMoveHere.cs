using UnityEngine;

namespace J
{

    [AddComponentMenu("J/NavMesh/JNavMeshMoveHere")]
    public class JNavMeshMoveHere : MonoBehaviour
    {
        [Tooltip("Si se deja vacío, se usa el tag Player que deberia tener un componente NavMeshAgent")]
        [SerializeField]    UnityEngine.AI.NavMeshAgent agent;
        [Tooltip("Si se deja vacío, se camina hacia este objeto")]
        [SerializeField]    Transform destination;


        public void MoveNavMeshAgent()
        {
            if (!agent)
                agent = GameObject.FindGameObjectWithTag("Player").GetComponent<UnityEngine.AI.NavMeshAgent>();

            if (destination)
                agent.destination = destination.position;
            else
                agent.destination = transform.position;
        }
    }
}