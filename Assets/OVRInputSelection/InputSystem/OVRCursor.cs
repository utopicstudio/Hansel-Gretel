using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ControllerSelection
{
    /// <summary>
    /// Modified version of the cursor which can receive raycast info from the input module.
    /// </summary>
    public abstract class OVRCursor : MonoBehaviour
    {
        /// <summary>
        /// Configure the ray used on the scan, should use default values
        /// </summary>
        /// <param name="rayResult"></param>
        public abstract void SetRay(Ray ray);

        /// <summary>
        /// Configure the raycast hit data, this should be used to configure non-default values and cursor hovering
        /// </summary>
        /// <param name="rayResult"></param>
        public abstract void SetRayHit(Vector3 Location, Vector3 Normal);

        /// <summary>
        /// Setups the state of the main click button of the cursor
        /// </summary>
        /// <param name="bPressed"></param>
        public abstract void SetButtonPressed(bool bPressed);
    }
}
