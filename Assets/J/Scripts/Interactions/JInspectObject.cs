using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnFinishedInspectingGameObjectSignature : UnityEvent<GameObject>
{
}

[RequireComponent(typeof(JMultiMove))]
[RequireComponent(typeof(Canvas))]
[AddComponentMenu("J/Interactions/InspectObject")]
public class JInspectObject : JInspect
{
    /// <summary>
    /// Called when a game object has been removed from the front view of the inspector, player not necesarily finished using the inspection tool though.
    /// </summary>
    public OnFinishedInspectingGameObjectSignature OnFinishedInspectingGameObject;

    /// <summary>
    /// Original transform that the object had before being inspected.
    /// </summary>
    public JTrans OriginalObjectTransform;

    /// <summary>
    /// Transform that the inspected objects must move to, in order for them to being inspected
    /// </summary>
    public Transform InspectCenter;
    
    /// <summary>
    /// Speed in which to rotate the target transform
    /// </summary>
    public float AnglePerSecond = 45;

    protected override void ChangedInspectObject(GameObject NewObject)
    {
        if (Target)
        {
            FinishInspectObject();
        }

        OriginalObjectTransform = new JTrans(NewObject.transform);

        //Move object and player to their respective points
        MoveObject(NewObject.transform, InspectCenter);
    }

    /// <summary>
    /// Do any subclass specific finish implementation
    /// </summary>
    protected override void FinishInspectImpl()
    {
        //Remove the object itself
        FinishInspectObject();
    }

    private void FinishInspectObject()
    {
        JTrans CachedTransform = OriginalObjectTransform;
        GameObject CachedObject = Target;
        OnFinishedInspectingGameObject.Invoke(CachedObject);

        //Move  the object to its initial position
        MoveObject(CachedObject.transform, CachedTransform);

        //Clear
        Target = null;
    }

    // Use this for initialization
    public void RotateClockwise()
    {
        if(Target)
        {
            Vector3 rotationAngles = transform.TransformDirection(Vector3.up) * AnglePerSecond;
            Target.transform.eulerAngles += rotationAngles * Time.deltaTime;
        }
    }

    public void RotateCounterClockwise()
    {
        if (Target)
        {
            Vector3 rotationAngles = transform.TransformDirection(Vector3.down) * AnglePerSecond;
            Target.transform.eulerAngles += rotationAngles * Time.deltaTime;
        }
    }

    public void RotateTopDown()
    {
        if (Target)
        {
            Vector3 rotationAngles = transform.TransformDirection(Vector3.left) * AnglePerSecond;
            Target.transform.eulerAngles += rotationAngles * Time.deltaTime;
        }
    }

    public void RotateDownTop()
    {
        if (Target)
        {
            Vector3 rotationAngles = transform.TransformDirection(Vector3.right) * AnglePerSecond;
            Target.transform.eulerAngles += rotationAngles * Time.deltaTime;
        }
    }
}
