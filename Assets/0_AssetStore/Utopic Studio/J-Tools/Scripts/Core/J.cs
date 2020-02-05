using UnityEngine;
using UnityEngine.Events;

namespace J
{

    /// <summary>
    /// Mode this application is currently running.
    /// </summary>
    public enum ApplicationMode
    {
        Normal,
        Preview
    }

    /// <summary>
    /// Main manager for JTools
    /// </summary>
    [AddComponentMenu("J/J")]
    public class J : JBase
    {
        //Called when the application boots on preview mode
        public UnityEvent OnPreviewMode;

        //this class is a singleton
        public static J Instance { get; private set; }

        //Current mode
        private ApplicationMode _AppMode = ApplicationMode.Normal;
        public ApplicationMode AppMode
        {
            get { return _AppMode; }
        }

        /// <summary>
        /// GameObject that represents the player (checked via TAG)
        /// </summary>
        public GameObject PlayerGameObject { get; private set; }

        private void Awake()
        {
            this.Singleton();
        }

        private void Singleton()
        {
            if (Instance == null)
            {
                Instance = this;
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLevelLoaded;

#if UNITY_WEBGL
                InitWebArguments();
#endif
            }
            else if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            //Trigger a call for the preview mode
            if(_AppMode == ApplicationMode.Preview)
            {
                OnPreviewMode?.Invoke();
            }
        }

#if UNITY_WEBGL
        void InitWebArguments()
        {
            //Obtain the sections that conform this url (separated by the GET char)
            string[] UrlSections = Application.absoluteURL.Split('?');
            Debug.Log("Obtained application url:" + Application.absoluteURL);
            if (UrlSections.Length > 1)
            {
                //Arguments are on the second section of this URL
                string[] Arguments = UrlSections[1].Split('&');
                foreach (string Arg in Arguments)
                {
                    //We need a final pass on the arguments, 
                    string[] KeyValue = Arg.Split('=');
                    ParseWebArgument(KeyValue[0], KeyValue[1]);
                }
            }
        }

        /// <summary>
        /// Processes each significant value of the web session arguments.
        /// </summary>
        /// <param name="Key">Name of the associated argument</param>
        /// <param name="Value">Value of the argument, which needs to be parsed down to the required type</param>
        void ParseWebArgument(string Key, string Value)
        {
            if (Key.ToLower() == "mode")
            {
                if (Key.ToLower() == "mode")
                {
                    switch (Value.ToLower())
                    {
                        case "normal":
                            _AppMode = ApplicationMode.Normal;
                            break;
                        case "preview":
                            _AppMode = ApplicationMode.Preview;
                            break;
                        default:
                            _AppMode = ApplicationMode.Normal;
                            break;
                    }

                    Debug.Log("Entered application mode: " + _AppMode);
                }
            }

        }
#endif

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

        /// <summary>
        /// Crea un sonido (no depende de el espacio o distancia 3D)
        /// </summary>
        /// <param name="audioClip">Clip de audio</param>
        /// <param name="volume">Volumen</param>
        public void PlaySound(AudioClip audioClip, float volume = 1f)
        {
            if (audioClip)
            {
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

        /// <summary>
        /// Lerp automático que no requiere de código adentro de la función Update()
        /// </summary>
        /// <param name="deleg">Función que se llama con un parámetro float correspondiente
        /// al valor de la curva (normalmente es de 0 a 1)</param>
        /// <param name="duration">Duración de la transición de 0 a 1</param>
        /// <param name="amplitude">Aumento de amplitud por M (parámetro irá de 0 a M) </param>
        /// <param name="repeat">Cantidad de veces que se hace el Lerp</param>
        /// <param name="type">Tipo de Lerp (lineal, sinusoidal, cuadrático, logaritmico, exponencial, etc)</param>
        /// <param name="reverse">Para hacer Lerp de 1 a 0</param>
        /// <param name="callingScript">usar keyword 'this' aca. sirve para visualizar
        /// cual objeto/script llamó a JLerp</param>
        public void Lerp(JFollowCurve.CurveDelegate deleg, float duration = 1, float amplitude = 1, int repeat = 1, CurveType type = CurveType.Linear, bool reverse = false, MonoBehaviour callingScript = null)
        {
            GameObject go = new GameObject("_j_followCurve " + (int)duration + "s" + " (x" + repeat + ")");

            JFollowCurve followCurve = go.AddComponent<JFollowCurve>();
            go.transform.parent = this.transform;
            followCurve.beginFollowCurve(deleg, duration, amplitude, repeat, type, reverse, callingScript);
            Destroy(go, duration);
        }

        //@DEPRECATED - Maintained for older scripts
        public void followCurve(JFollowCurve.CurveDelegate d, float duration = 1, float amplitude = 1, int repeat = 1, CurveType type = CurveType.Linear, bool reverse = false)
        {
            Lerp(d, duration, amplitude, repeat, type, reverse);
        }
    }
}