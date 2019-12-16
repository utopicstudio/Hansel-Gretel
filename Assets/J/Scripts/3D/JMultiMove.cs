using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A less constricted version of the Move script, that handles movement when calling the Move method, isn't restricted to being applied OnStart only and can
/// move more than one object at the same time, while managing their queue. It has support for JTrans, so it doesn't need a transform to be setup on the target.
/// It's better
/// </summary>
[AddComponentMenu("J/3D/JMultiMove")]
public class JMultiMove : MonoBehaviour
{
    [System.Serializable]
    struct MovementQueueEntry
    {
        public MovementQueueEntry(Transform InAffectedTransform, JTrans InTargetTransform, bool bInAffectsRotation, float InLerpFactor)
        {
            AffectedTransform = InAffectedTransform;
            TargetTransform = InTargetTransform;
            bAffectsRotation = bInAffectsRotation;
            LerpFactor = InLerpFactor;
        }

        //For equality - We only check that the transform is the same
        public static bool operator ==(MovementQueueEntry c1, MovementQueueEntry c2)
        {
            return c1.AffectedTransform == c2.AffectedTransform;
        }

        public static bool operator !=(MovementQueueEntry c1, MovementQueueEntry c2)
        {
            return c1.AffectedTransform != c2.AffectedTransform;
        }

        public override bool Equals(System.Object obj)
        {
            return obj is MovementQueueEntry && this == (MovementQueueEntry)obj;
        }
        public override int GetHashCode()
        {
            return AffectedTransform.GetHashCode();
        }

        public Transform AffectedTransform;
        public JTrans TargetTransform;
        public bool bAffectsRotation;
        public float LerpFactor;
    }

    /// <summary>
    /// Actual movement queue
    /// </summary>
    [SerializeField]
    private List<MovementQueueEntry> MovementQueue = new List<MovementQueueEntry>();

    /// <summary>
    /// Distance that the target and affected transforms should have for them to be considered equal.
    /// </summary>
    public float DistanceTreshold = 0.01f;

    /// <summary>
    /// Contains a static list of which JMultiMove is responsible of moving each queue entry. Used to avoid multiple multimoves trying to move the same transform
    /// </summary>
    private static Dictionary<MovementQueueEntry, JMultiMove> QueueResponsability = new Dictionary<MovementQueueEntry, JMultiMove>();

    // Update is called once per frame
    void Update()
    {
        for (int i = MovementQueue.Count - 1; i >= 0; i--)
        {
            MovementQueueEntry Entry = MovementQueue[i];

            if(QueueResponsability[Entry] == this)
            {
                Entry.AffectedTransform.position = Vector3.Lerp(Entry.AffectedTransform.position, Entry.TargetTransform.position, Entry.LerpFactor);

                if (Entry.bAffectsRotation)
                {
                    Entry.AffectedTransform.rotation = Quaternion.Lerp(Entry.AffectedTransform.rotation, Entry.TargetTransform.rotation, Entry.LerpFactor);
                }

                //Should remove from the queue if we've completed movement
                float SqrDistance = (Entry.AffectedTransform.position - Entry.TargetTransform.position).sqrMagnitude;
                if (SqrDistance < (DistanceTreshold * DistanceTreshold))
                {
                    MovementQueue.RemoveAt(i);
                    QueueResponsability.Remove(Entry);
                }
            }
            else
            {
                Debug.Log("Queue responsibility changed to new multimove, updating");
                MovementQueue.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Adds a movement to the queue
    /// </summary>
    /// <param name="AffectedTransform"></param>
    /// <param name="TargetTransform"></param>
    /// <param name="LerpFactor"></param>
    /// <param name="bAffectsRotation"></param>
    public void Move(Transform AffectedTransform, JTrans TargetTransform, bool bAffectsRotation = false, float LerpFactor = 0.05f)
    {
        MovementQueueEntry Entry = new MovementQueueEntry(AffectedTransform, TargetTransform, bAffectsRotation, LerpFactor);
        MovementQueue.Remove(Entry);
        MovementQueue.Add(Entry);

        //This component is responsible of managing the entry now
        QueueResponsability[Entry] = this;
    }

    public void Move(Transform AffectedTransform, Transform TargetTransform, bool bAffectsRotation, float LerpFactor = 0.05f)
    {
        Move(AffectedTransform, new JTrans(TargetTransform), bAffectsRotation, LerpFactor);
    }

    public void Cancel(Transform AffectedTransform)
    {
        MovementQueueEntry DummyEntry = new MovementQueueEntry();
        DummyEntry.AffectedTransform = AffectedTransform;

        MovementQueue.Remove(DummyEntry);
        QueueResponsability.Remove(DummyEntry);
    }
}
