using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using J;

namespace J
{


    [AddComponentMenu("J/3D/JRotate")]
    public class JRotate : MonoBehaviour
    {

        public float duration = 2f;

        public void Rotate(float angle)
        {
            Vector3 rot = transform.rotation.eulerAngles;
            rot = new Vector3(rot.x, rot.y + angle, rot.z);
            Quaternion rotation = Quaternion.Euler(rot);

            J.Instance.followCurve(x => transform.rotation = Quaternion.Lerp(transform.rotation, rotation, x), duration);
        }
    }

}