using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;

namespace J
{
	
	[AddComponentMenu("J/Util/JSceneLoad")]
	public class JSceneLoad : JBase
    {

        [Tooltip("Escena debe estar en Build Settings o en un bundle")]
        //Default scene name when calling LoadScene with no parameters
		[SerializeField]
        string sceneName;

        //Default scene delay when calling LoadScene with no parameters
        [SerializeField]
        float delay = 0f;

        //Scene bundle index, will try to load levels via Bundles if this parameter is setup
        [SerializeField, Tooltip("Scene bundle index, will try to load levels via Bundles if this parameter is set")]
        SceneBundleIndex BundleIndex;

        [SerializeField, Tooltip("Name of the relative folder where the bundles are located on the webserver.")]
        string AssetBundleDirectory = "AssetBundles";

        /// <summary>
        /// Loads the scene, no parameters for listener callbacks
        /// </summary>
        public void LoadScene()
        {
            LoadScene(sceneName, delay);
        }
        
        /// <summary>
        /// Full load scene method
        /// </summary>
        /// <param name="SceneName"></param>
        /// <param name="Delay"></param>
        public void LoadScene(string SceneName, float Delay)
        {
            if(Delay > 0.0f)
            {
                StartCoroutine(EnqueueSceneChange(SceneName, Delay));
            }
            else
            {
                _PerformSceneChange(SceneName);
            }
        }

        private IEnumerator EnqueueSceneChange(string SceneName, float Delay)
        {
            yield return new WaitForSeconds(Delay);
            _PerformSceneChange(SceneName);
        }

        /// <summary>
        /// Actually performs the scene change via SceneManager or SceneBundles depending if an index has been setup
        /// </summary>
        /// <param name="SceneName"></param>
		private void _PerformSceneChange (string SceneName) {

            //First perform validations
            sceneName = sceneName.Trim ();
			if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogWarning("Trying to perform SceneChange with empty scene path");
                return;
            }
                        
            //Then change the scene depending on the loading type
            if(BundleIndex && BundleIndex.Contains(sceneName))
            {
                //Obtain this game's application url
                string[] UrlSections = Application.absoluteURL.Split('?');
                string URL = UrlSections[0];
                URL = URL.TrimEnd('/');

                //Append the directory and bundle path
                URL += "/" + AssetBundleDirectory + "/" + BundleIndex[sceneName];
                StartCoroutine(ChangeSceneByBundle(URL));
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            }
        }

        private IEnumerator ChangeSceneByBundle(string AssetBundleURL)
        {
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(AssetBundleURL, 1, 0);
            yield return request.SendWebRequest();
            AssetBundle SceneBundle = DownloadHandlerAssetBundle.GetContent(request);

            //Load the scene and finish
            if (SceneBundle)
            {
                string[] Paths = SceneBundle.GetAllScenePaths();
                UnityEngine.SceneManagement.SceneManager.LoadScene(Paths[0]);
            }
        }

        #region backwards_compatibility
        /// <summary>
        /// Note: Backwards compatibility with first version.
        /// </summary>
        public void ChangeScene()
        {
            LoadScene();
        }
        #endregion

    }

}