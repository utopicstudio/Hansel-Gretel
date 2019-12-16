using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace J
{
    /// <summary>
    /// Helper component that creates a Proxy between the "J" manager classes and its methods.
    /// Making it possible to bind events even when the manager classes come from another level
    /// </summary>
    [AddComponentMenu("J/Util/JProxy")]
    public class JProxy : MonoBehaviour
    {
        #region J
        /// <summary>
        /// Volume to use when playing sound via the event method.
        /// </summary>
        public float DefaultPlaySoundVolume = 1.0f;

        /* Calls the J.Instance PlaySound */
        public void PlaySound(AudioClip audioClip, float volume)
        {
            if (J.Instance)
            {
                J.Instance.PlaySound(audioClip, volume);
            }
        }

        /* Simplified version of the play sound method can be used as an UnityEvent entry point (hence why we avoid default parameters) */
        public void PlaySound(AudioClip audioClip)
        {
            PlaySound(audioClip, 1.0f);
        }
        #endregion

        #region JResourceManager
        public void PushResourceAnswers()
        {
            if(JResourceManager.Instance)
            {
                JResourceManager.Instance.PushAnswers();
            }
        }
        #endregion
    }

}
