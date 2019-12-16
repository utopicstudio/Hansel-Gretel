using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// When inspecting something, the player will focus onto that object by moving onto the target position.
/// If another inspect is triggered, the previous one will be finished before continuing.
/// </summary>
[RequireComponent(typeof(JMultiMove))]
[RequireComponent(typeof(Canvas))]
[AddComponentMenu("J/Interactions/Inspect")]
public class JInspect : MonoBehaviour
{
    /// <summary>
    /// Called when the player just starts inspecting an object
    /// </summary>
    public UnityEvent OnBeginInspect;

    /// <summary>
    /// Called when the player finishes using the inspection object
    /// </summary>
    public UnityEvent OnFinishInspect;

    /// <summary>
    /// Target to apply the rotation to
    /// </summary>
    protected GameObject Target;

    /// <summary>
    /// Transform the player must move to, in order for the inspection to begin
    /// </summary>
    public Transform PlayerCenter;

    /// <summary>
    /// Original transform for the player object, that will be used when stopping the inspection.
    /// </summary>
    public JTrans OriginalPlayerTransform;

    /// <summary>
    /// If the player is currently inspecting an object
    /// </summary>
    protected bool bCurrentlyInspecting = false;

    /// <summary>
    /// Cached multi move component used to move the player and target
    /// </summary>
    protected JMultiMove _CachedMultiMove = null;

    /// <summary>
    /// Canvas to use for inspecting
    /// </summary>
    protected Canvas _CachedCanvas = null;

    /// <summary>
    /// Currently running inspect
    /// </summary>
    private static JInspect CurrentInspect = null;

    private void Awake()
    {
        _CachedMultiMove = GetComponent<JMultiMove>();
        _CachedCanvas = GetComponent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _CachedCanvas.enabled = false;
    }

    public void Inspect(GameObject InspectTarget)
    {
        if(CurrentInspect != null && CurrentInspect != this)
        {
            CurrentInspect.FinishInspect(true);
        }

        if (InspectTarget != null && InspectTarget != Target)
        {
            //We have to make sure we're not currently inspecting another object
            ChangedInspectObject(InspectTarget);

            Target = InspectTarget;

            //Move the player only if we're not currently in inspection range
            if (!bCurrentlyInspecting)
            {
                OriginalPlayerTransform = new JTrans(J.J.Instance.PlayerGameObject.transform);
                MoveObject(J.J.Instance.PlayerGameObject.transform, PlayerCenter, false);
                bCurrentlyInspecting = true;
                _CachedCanvas.enabled = true;
                CurrentInspect = this;

                OnBeginInspect.Invoke();
            }
        }
    }

    /// <summary>
    /// Called before we setup the new object
    /// </summary>
    /// <param name="NewObject"></param>
    protected virtual void ChangedInspectObject(GameObject NewObject)
    {

    }

    /// <summary>
    /// Stops inspecting the current object, calling any delegate and returning each object to its original position
    /// </summary>
    public void FinishInspect(bool bForceCancel = false)
    {
        if(bForceCancel)
        {
            //We force canceled, remove the character from the multi move queue
            _CachedMultiMove.Cancel(J.J.Instance.PlayerGameObject.transform);
        }
        else
        {
            //Return player to its original position
            MoveObject(J.J.Instance.PlayerGameObject.transform, OriginalPlayerTransform, false);
        }

        //Call any specifics from subclass
        FinishInspectImpl();

        //No longer using this
        bCurrentlyInspecting = false;
        _CachedCanvas.enabled = false;
        CurrentInspect = null;
        Target = null;
        OnFinishInspect.Invoke();
    }

    /// <summary>
    /// Do any subclass specific finish implementation
    /// </summary>
    protected virtual void FinishInspectImpl()
    {

    }

    /// <summary>
    /// Convenience method for using transforms
    /// </summary>
    /// <param name="MovingTransform"></param>
    /// <param name="TargetTransform"></param>
    protected void MoveObject(Transform MovingTransform, Transform TargetTransform, bool bForceRotation = true)
    {
        MoveObject(MovingTransform, new JTrans(TargetTransform), bForceRotation);
    }

    /// <summary>
    /// Moves async the object to its target position
    /// </summary>
    /// <param name="MovingTransform"></param>
    /// <param name="TargetTrans"></param>
    protected void MoveObject(Transform MovingTransform, JTrans TargetTrans, bool bForceRotation = true)
    {
        _CachedMultiMove.Move(MovingTransform, TargetTrans, bForceRotation);
    }
}
