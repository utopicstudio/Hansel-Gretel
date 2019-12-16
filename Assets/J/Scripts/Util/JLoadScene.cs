using UnityEngine;

namespace J
{
	
	[AddComponentMenu("J/Util/JLoadScene")]
	public class JLoadScene : MonoBehaviour {

        
		[SerializeField]	string sceneName;
		[SerializeField]	float delay = 0f;

        public void JLoadLevel()
        {
            this.LoadTheLevel();
        }
        public void LoadTheLevel () {
            Invoke("LoadTheLevelPrivate", delay);
		}
        private void LoadTheLevelPrivate () {
			sceneName = sceneName.Trim ();
			if (sceneName != null && sceneName != "") {
				UnityEngine.SceneManagement.SceneManager.LoadScene (sceneName);
			}
		}
	}

}