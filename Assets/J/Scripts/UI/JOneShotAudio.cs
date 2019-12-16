using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace J
{
    /// <summary>
    /// Helper class to add a one shot audio that can be triggered manually and will die after playing
    /// </summary>
    [AddComponentMenu("J/UI/OneShotAudio")]
    public class JOneShotAudio : MonoBehaviour
    {
        /// <summary>
        /// Audio clip to play
        /// </summary>
        [SerializeField]
        AudioClip Clip;

        /// <summary>
        /// Volume to use when playing
        /// </summary>
        [Range(0f, 1f)]
        [SerializeField] float Volume = 0.7f;

        /// <summary>
        /// Plays the sound via the JManager.
        /// Fire & Forget
        /// </summary>
        public void Fire()
        {
            if (Clip)
            {
                J.Instance.PlaySound(Clip, Volume);
            }
        }
    }

}