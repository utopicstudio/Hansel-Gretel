using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace J
{
    /// <summary>
    /// Redirects the game flow towards a specific scene when ApplicationMode.Preview is enabled by searching the Resource DB.
    /// If normal mode is enabled, goes to the configured main scene.
    /// Meant to be used on a "loading" scene that works as a fork for the initial game flow.
    /// </summary>
    [AddComponentMenu("J/Resources/ResourceRedirector")]
    public class JResourceRedirector : MonoBehaviour
    {
        /// <summary>
        /// Loader for the scene
        /// </summary>
        public JSceneLoad SceneLoad;

        /// <summary>
        /// Holds an indexed array for the Resource codes and its scenes.
        /// </summary>
        public JResourceIndex Index;

        private void Awake()
        {
            if(!SceneLoad)
            {
                Debug.LogWarning("No Scene load found, cannot perform preview redirections!"); 
            }
        }

        /// <summary>
        /// Redirects to the chosen scene for the preview mode.
        /// Selection of the resource to focus is done on that level.
        /// </summary>
        public void PerformPreviewRedirection()
        {
            if(SceneLoad)
            {
                string RedirectPath;
                string ResourceCode = JResourceManager.Instance.PreviewResourceCode;
                if (Index.FindResourceScene(JResourceManager.Instance.PreviewResourceCode, out RedirectPath))
                {
                    Debug.Log("Redirecting to preview mode scene");
                    SceneLoad.LoadScene(RedirectPath, 0.0f);
                }
                else
                {
                    Debug.LogWarning("Trying to redirect to scene holding resource with code: '" + ResourceCode + "' failed, resource path not found on ResourceIndex");
                }
            }
        }
    }

}