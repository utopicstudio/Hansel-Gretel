using UnityEngine;

namespace J
{
		
	[AddComponentMenu("J/J")]
	public class J : MonoBehaviour {

		//this class is a singleton)
		public static J Instance {get; private set;}

        /// <summary>
        /// GameObject that represents the player (checked via TAG)
        /// </summary>
        public GameObject PlayerGameObject { get; private set; }

		private void Awake() {
            this.Singleton();
		}

        private void Singleton()
        {
            if (Instance == null)
            {
                Instance = this;
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLevelLoaded;
            }
            else if (Instance != this)
            {
                Destroy(this.gameObject); // Nota: Esto podria eliminar todo un objeto, con otros componentes
            }
        }

        // Runs each time a scene is loaded, even when script is using DontDestroyOnLoad().
        // Finds a GameObject tagged with 'Player'. Needs to find exactly one.
        void OnLevelLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            PlayerGameObject = null;
            GameObject[] goArr = GameObject.FindGameObjectsWithTag("Player");
            if (goArr.Length > 0)
            {
                PlayerGameObject = goArr[0];
            }
        }

        public void PlaySound(AudioClip audioClip, float volume = 1f)
        {
            if(audioClip)
            {
                Debug.Log("Playing sound.");
                GameObject go = null;
                AudioSource audioSource = null;

                go = new GameObject("_j_audio_");
                audioSource = go.AddComponent<AudioSource>();
                audioSource.clip = audioClip;

                audioSource.volume = volume;
                audioSource.Play();

                Destroy(go, audioClip.length + 0.05f);
            }
            else
            {
                Debug.Log("No sound given.");
            }
        }

        /*
		 * Used for doing Lerp or non-linear interpolation.
		 * The first parameter is a function that does something with a float value
		 * that changes between 0 and param_'amplitude'
		 */
        public void followCurve(JFollowCurve.CurveDelegate d , float duration = 1, float amplitude = 1, int repeat = 0, CurveType type = CurveType.Linear, bool reverse = false) {
			GameObject go = new GameObject ("_j_followCurve");
				
			JFollowCurve followCurve = go.AddComponent<JFollowCurve> ();
            go.transform.parent = this.transform;
            followCurve.begin( d, duration, amplitude, repeat, type, reverse);
			Destroy (go, duration);
		}
    }


















}