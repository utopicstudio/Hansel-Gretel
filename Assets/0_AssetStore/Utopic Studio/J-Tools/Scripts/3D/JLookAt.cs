using UnityEngine;

namespace J
{
    /// <summary>
    /// Simple rotation based look at, that maintains an object pointing towards another at all time.
    /// </summary>
    [AddComponentMenu("J/3D/JLookAt")]
    public class JLookAt : JBase
    {
        //Object to rotate
        [Tooltip("Dejar vacío para que este objeto sea el que mire a otro")]
        public Transform target;

        //Object that we will look at
        [Tooltip("Mirar a este objeto")]
        public Transform point;

        //if point is null, it will search for the player instead
        [Tooltip("Mirar al tag Player (Sólo si variable point está vacía)")]
        public bool lookAtPlayer = true;
        public bool allowVerticalRotation = true;
        public bool lookWithBackSide = false;

        [RangeAttribute(1f, 50f)]
        public float speed = 20f;
        
        private void Start()
        {
            if (!target)
            {
                target = transform;
            }
            if (lookAtPlayer && !point)
            {
                point = (GameObject.FindGameObjectWithTag("Player") as GameObject).transform;
            }

        }

        private void Update()
        {
            Vector3 dir = point.position - target.position;

            if (lookWithBackSide)
            {
                dir = -dir;
            }
            if (!allowVerticalRotation)
            {
                dir.y = 0f;
            }

            //Avoid rotation when not needed
            if (dir.sqrMagnitude > Vector3.kEpsilon)
            {
                //Obtain the desired rotation and lerp to it
                Quaternion rotation = Quaternion.LookRotation(dir);
                target.rotation = Quaternion.Slerp(target.rotation, rotation,
                    Time.deltaTime * speed);
            }
        }
    }

}