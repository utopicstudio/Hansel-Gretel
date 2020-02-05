using UnityEngine;

namespace J
{
	[AddComponentMenu("J/3D/JPatrol")]
	[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
	public class JPatrol : MonoBehaviour {

		[SerializeField]	Transform[] points;
		[Tooltip("Aumentar si el objeto no sigue al siguiente waypoint")]
		[SerializeField]	float arrivedDistance = 0.5f;


		protected int destPoint = 0;
		protected UnityEngine.AI.NavMeshAgent agent;


		void Start () {
			agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

			GotoNextPoint();
		}


		void GotoNextPoint() {
			// Returns if no points have been set up
			if (points.Length == 0)
				return;

			// Set the agent to go to the currently selected destination.
			agent.destination = points[destPoint].position;

			// Choose the next point in the array as the destination,
			// cycling to the start if necessary.
			destPoint = (destPoint + 1) % points.Length;
		}


		void Update () {
			// Choose the next destination point when the agent gets
			// close to the current one.
			if (!agent.pathPending && agent.remainingDistance < arrivedDistance)
				GotoNextPoint();
		}
	}

}