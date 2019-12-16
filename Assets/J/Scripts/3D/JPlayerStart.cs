using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace J
{
    /// <summary>
    /// Defines a transform that the player will be moved to when loading the level, useful for managing persistent characters
    /// that need to be reallocated on new scene loading.
    /// </summary>
    [AddComponentMenu("J/3D/JPlayerStart")]
    public class JPlayerStart : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GameObject Player = J.Instance.PlayerGameObject;
            if(Player)
            {
                Player.transform.position = transform.position;
                Player.transform.rotation = transform.rotation;
            }
        }
    }
}
