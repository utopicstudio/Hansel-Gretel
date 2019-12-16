using UnityEngine;

namespace J
{
	
	[AddComponentMenu("J/Util/JSceneLoad")]
	public class JSceneLoad : MonoBehaviour {

        [Tooltip("Escena debe estar en Build Settings")]
		[SerializeField]    string sceneName;
		[SerializeField]	float delay = 0f;

		public void ChangeScene () {
			Invoke ("_ChangeScene", delay);
		}
		private void _ChangeScene () {
			sceneName = sceneName.Trim ();
			if (sceneName != null && sceneName != "") {
				UnityEngine.SceneManagement.SceneManager.LoadScene (sceneName);
			}
		}
	}

}