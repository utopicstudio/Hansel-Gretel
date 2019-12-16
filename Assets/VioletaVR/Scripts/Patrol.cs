// Patrol.cs
using UnityEngine;
using System.Collections;


public class Patrol : MonoBehaviour {

	public Transform[] points;
	private int destPoint = 0;
	private UnityEngine.AI.NavMeshAgent agent;
    public int timer = 5;


	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

		// Disabling auto-braking allows for continuous movement
		// between points (ie, the agent doesn't slow down as it
		// approaches a destination point).
		//agent.autoBraking = false;

		GotoNextPoint();
	}


	void GotoNextPoint() {
		// Returns if no points have been set up
		if (points.Length == 0)
			return;

		// Set the agent to go to the currently selected destination.
		agent.destination = points[destPoint].position;

		// Choose the next point in the array as the destination.
		destPoint = (destPoint + 1);

        // Timer reset
        timer = 5;
	}


	void Update () {
		// Choose the next destination point when the agent gets
		// close to the current one.
		if (!agent.pathPending && agent.remainingDistance < 0.5f && timer <= 0)
			GotoNextPoint();
	}
}