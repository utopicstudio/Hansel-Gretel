using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace J
{
    /// <summary>
    /// Defines a point used for triggering movement on the character.
    /// </summary>
    [AddComponentMenu("J/3D/JWaypoint"), RequireComponent(typeof(JCameraFade))]
    public class JWaypoint : MonoBehaviour
    {
        enum EWaypointMovementType
        {
            Teleport,
            Navmesh
        }

        /// <summary>
        /// Type of movement to use, defaults to Teleport
        /// </summary>
        [SerializeField]
        private EWaypointMovementType MovementType = EWaypointMovementType.Teleport;

        /// <summary>
        /// Distance from the floor that this navpoint will teleport the player to when doing a teleport movement
        /// If a NavMeshAgent is found on the target player, it will use said height instead
        /// </summary>
        [SerializeField]
        private float TargetHalfHeight = 1.5f;

        /// <summary>
        /// Max trace to perform when trying to find a floor to teleport the player to.
        /// </summary>
        [SerializeField]
        private float FindFloorMaxTrace = 5.0f;

        /// <summary>
        /// When using Navmesh movement, the affected agent is needed for movement.
        /// </summary>
        private NavMeshAgent AffectedAgent;

        /// <summary>
        /// Fade to use on teleport movement type
        /// </summary>
        private JCameraFade CameraFade;

        //Called before start
        private void Start()
        {
            AffectedAgent = J.Instance.PlayerGameObject.GetComponent<NavMeshAgent>();
            TargetHalfHeight = AffectedAgent != null ? AffectedAgent.height : TargetHalfHeight;

            if (MovementType == EWaypointMovementType.Teleport)
            {
                //CameraFade = J.Instance.PlayerGameObject.GetComponentInChildren<JCameraFade>();
                CameraFade = GetComponent<JCameraFade>();

                if(!CameraFade)
                {
                    Debug.LogError("No camera fade found on player when trying to perform a waypoint movement of type \"Teleport\". ");
                }
            }
            else
            {
                if (!AffectedAgent)
                {
                    Debug.LogError("No navmesh agentfound on player when trying to perform a waypoint movement of type \"Navmesh\". Cannot continue");
                }
            }
        }

        /// <summary>
        /// Enqueues the movement towards this waypoint
        /// </summary>
        public void WaypointMove()
        {
            if(MovementType == EWaypointMovementType.Teleport)
            {
                CameraFade.FadeOut();
                StartCoroutine(TeleportAfterSeconds(CameraFade.FadeOutTime));
            }
            else
            {
                AffectedAgent.SetDestination(transform.position);
            }
        }
        
        /// <summary>
        /// Used to enqueue the teleport after a fade out
        /// </summary>
        /// <param name="Seconds"></param>
        /// <returns></returns>
        private IEnumerator TeleportAfterSeconds(float Seconds)
        {
            yield return new WaitForSeconds(Seconds);

            //We need to find the target position... generally speaking is the 
            Vector3 TargetPosition = transform.position;
            RaycastHit Hit;
            if (Physics.Linecast(transform.position, transform.position - Vector3.up * FindFloorMaxTrace, out Hit))
            {
                TargetPosition = Hit.point + Vector3.up * TargetHalfHeight;
            }

            J.Instance.PlayerGameObject.transform.position = TargetPosition;
            Teleported();
        }

        /// <summary>
        /// Called when the teleport takes place
        /// </summary>
        private void Teleported()
        {
            CameraFade.FadeIn();
        }
    }
}
