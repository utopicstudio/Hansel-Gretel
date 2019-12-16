using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MigasDePan : MonoBehaviour
{
    [Tooltip("Remember to put a rigidbody into this object:")]
    public GameObject throwedObject;
    public Transform point;
    public Vector3 offset;
    public Transform forward;
    public float velocity;

    public void Throw()
    {
        GameObject obj = Instantiate(throwedObject);
        obj.transform.position = point.position + offset;
        obj.transform.forward = forward.forward;
        obj.GetComponent<Rigidbody>().AddForce(forward.forward * velocity, ForceMode.VelocityChange);
    }
}
