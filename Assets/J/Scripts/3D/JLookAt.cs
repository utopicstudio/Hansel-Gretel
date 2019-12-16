using UnityEngine;

namespace J
{

    [AddComponentMenu("J/3D/JLookAt")]
    public class JLookAt : MonoBehaviour
    {
        public GameObject[] players;    
        
        public enum LookAtType
        {
            Constant, OneShot
        }

        [SerializeField] LookAtType mode;
        [SerializeField] Transform target;
        [SerializeField] bool x, y=true, z;
        [SerializeField] UnityEngine.AI.NavMeshAgent navMeshAgent;

        private bool startWasCalled = false;

        private void Start()
        {
            startWasCalled = true;
            
            players = GameObject.FindGameObjectsWithTag("Player");
            target = players[0].transform;
        }

        void Update()
        {
            if (mode == LookAtType.Constant)
            {
                Quaternion rotation = CalculateRotation(target);
                if (!x)
                    rotation.x = 0f;
                if (!y)
                    rotation.y = 0f;
                if (!z)
                    rotation.z = 0f;
                //transform.LookAt(target);
                transform.rotation = rotation;
            }
        }
        private Quaternion CalculateRotation(Transform target)
        {
            return Quaternion.Euler(Quaternion.LookRotation(target.position - transform.position).eulerAngles);
        }
        public void LookAt(Transform target)
        {
            if (target == null)
                target = this.target;
            //if (navMeshAgent)
            //    navMeshAgent.enabled = false; 
            transform.rotation = CalculateRotation(target);
            //if (navMeshAgent)
            //    navMeshAgent.enabled = true;
        }
    }

}