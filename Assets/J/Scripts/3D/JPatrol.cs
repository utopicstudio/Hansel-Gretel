using UnityEngine;
using UnityEngine.AI;

namespace J
{
	[AddComponentMenu("J/3D/JPatrol")]
	[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
	public class JPatrol : MonoBehaviour {

		[SerializeField]	Transform[] points;
		[Tooltip("Aumentar si el objeto no sigue al siguiente waypoint")]
		[SerializeField]	float arrivedDistance = 0.5f;
        public NavMeshAgent agente;

		protected int destPoint = 0;
		protected UnityEngine.AI.NavMeshAgent agent;
        private float timer = 5;
        public float maxTime;
       //private bool waiting;
       


		void Start () {

            agente = GameObject.FindGameObjectWithTag("Hansel").GetComponent<NavMeshAgent>();
            timer = maxTime;
			agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
			GotoNextPointAuto();
		}


		void GotoNextPointAuto() {
			// Returns if no points have been set up
			if (points.Length == 0)
				return;

			// Set the agent to go to the currently selected destination.
			agent.destination = points[destPoint].position;

			// Choose the next point in the array as the destination,
			// cycling to the start if necessary.
			destPoint = (destPoint + 1);
            timer = maxTime;
		}


		void Update () {
            // Choose the next destination point when the agent gets
            // close to the current one.
            if (agent.remainingDistance <= arrivedDistance) {
                timer -= Time.deltaTime;
            }
			if (!agent.pathPending && agent.remainingDistance < arrivedDistance && timer <= 0 && destPoint != points.Length)
				GotoNextPointAuto();
		}
	}

}