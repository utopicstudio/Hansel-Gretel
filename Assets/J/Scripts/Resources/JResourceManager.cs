using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace J
{
    /// <summary>
    /// Mode that this Application is running
    /// </summary>
    public enum ApplicationMode
    {
        Normal,
        Preview
    };

    /// <summary>
    /// Holds data for answers already setup on the resource
    /// </summary>
    public struct ResourceAnswer
    {
        /// <summary>
        /// Identifies the resource content that is being answered
        /// </summary>
        public string Identifier;

        /// <summary>
        /// The number of the page the answer is on
        /// </summary>
        public int PageIndex;

        /// <summary>
        /// The index of the option we're answering.
        /// </summary>
        public int OptionIndex;

        /// <summary>
        /// The actual answer data
        /// </summary>
        public string Data;

        public ResourceAnswer(string InIdentifier, int InPageIndex, int InOptionIndex, string InData = "")
        {
            Identifier = InIdentifier;
            PageIndex = InPageIndex;
            OptionIndex = InOptionIndex;
            Data = InData;
        }

        public override bool Equals(object obj)
        {
            ResourceAnswer Other = (ResourceAnswer)obj;
            return obj is ResourceAnswer && Other.Identifier == Identifier && Other.PageIndex == PageIndex && Other.OptionIndex == OptionIndex;            
        }

        public static bool operator ==(ResourceAnswer s1, ResourceAnswer s2)
        {
            return s1.Equals(s2);
        }
        public static bool operator !=(ResourceAnswer s1, ResourceAnswer s2)
        {
            return !s1.Equals(s2);
        }

        public override int GetHashCode()
        {
            //Can potentially collide with other hashes, but its a longshot
            return Identifier.GetHashCode() + 1000 * PageIndex + OptionIndex;
        }

        public void AddJSONContents(JSONObject j)
        {
            j.AddField("id_contenido", Identifier);
            j.AddField("indice_pregunta", PageIndex);
            j.AddField("indice_opcion", OptionIndex);
            j.AddField("respuesta", Data);
        }
    }

    /// <summary>
    /// Singleton class manager for the resources.
    /// </summary>
    [AddComponentMenu("J/Resources/ResourceManager")]
    public class JResourceManager : MonoBehaviour
    {
        public UnityEvent OnFetchComplete;
        public UnityEvent OnPushComplete;

        public struct ResourceMetaData
        {
            public string Id;
            public string Name;

            public ResourceMetaData(string InId, string InName)
            {
                Id = InId;
                Name = InName;
            }
        }

        /* Singleton getters */
        public static JResourceManager Instance { get; private set; }

        /* Server URL */
        public string FetchURL = "http://localhost:8000/";

        /* The current application mode */
        public ApplicationMode AppMode { get; private set; }

        /* Resource code to search when in preview mode */
        public string PreviewResourceCode { get; private set; }

        /* Contains the original Fetched data */ 
        private JSONObject ResourceData;

        /* Contains the resources (contents) of the application, indexed */
        private Dictionary<string, JSONObject> ResourceContentDictionary = new Dictionary<string, JSONObject>();

        /* Contains the answers the player has registered so far, indexed by their hash */
        private Dictionary<int, ResourceAnswer> Answers = new Dictionary<int, ResourceAnswer>();

        /* Code that statically identifies this resource */ 
        [SerializeField]
        private string BaseResourceCode;

        /* Code that identifies the actual clone the user if using */
        public ResourceMetaData SelectedResource { get; private set; }

        /* List of available resource codes related to this one, that could be played */
        private List<string> AvailableResourceCodes = new List<string>();
        
        void Awake()
        {
            //As this class is a singleton, we only allow one instance to be alive.
            if (Instance == null)
            {
                //Setup singleton values
                Instance = this;
                DontDestroyOnLoad(gameObject);

                //This will initialize important values, like the application mode, etc
                InitWebArguments();

                //Will need to manage level loading for resources
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLevelLoaded;

                //By default, we use the base resource code
                SelectedResource = new ResourceMetaData(BaseResourceCode, "Base Resource");
            }
            else if (Instance != this)
            {
                //We already have a manager, eliminate this one and maintain the original.
                Destroy(gameObject);
            }

        }

        void InitWebArguments()
        {
            //We initialize the value first, we could be on a build that doesn't support web argument passing, in this case we fallback to normal mode
            AppMode = ApplicationMode.Normal;

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
        /// Manually selects each significant value and stores it on this object.
        /// </summary>
        /// <param name="Key">Name of the associated argument</param>
        /// <param name="Value">Value of the argument, which needs to be parsed down to the required type</param>
        void ParseWebArgument(string Key, string Value)
        {
            if (Key.ToLower() == "mode")
            {
                switch (Value.ToLower())
                {
                    case "normal":
                        AppMode = ApplicationMode.Normal;
                        break;
                    case "preview":
                        AppMode = ApplicationMode.Preview;
                        break;
                    default:
                        AppMode = ApplicationMode.Normal;
                        break;
                }

                Debug.Log("Entered application mode: " + AppMode);
            }
            else if (Key.ToLower() == "code")
            {
                PreviewResourceCode = Value;
            }
        }

        /// <summary>
        /// Obtains the resource codes currently available on the scene
        /// </summary>
        /// <param name="MappedResources">A search map for each resource, indexed with its code. </param>
        /// <returns>An array of the resource codes currently available. </returns>
        string[] GetResourceCodes(out Dictionary<string, JResource> MappedResources)
        {
            //Initialize the out parameter
            MappedResources = new Dictionary<string, JResource>();

            //Get all the resources
            JResource[] resources = GameObject.FindObjectsOfType<JResource>();

            //Map each code to the resource
            List<string> codes = new List<string>();
            foreach (JResource r in resources)
            {
                if (!MappedResources.ContainsKey(r.Code))
                {
                    codes.Add(r.Code);
                    MappedResources.Add(r.Code, r);
                }
            }

            return codes.ToArray();
        }

        // Runs each time a scene is loaded, even when script is using DontDestroyOnLoad().
        // Finds a GameObject tagged with 'Player'. Needs to find exactly one.
        void OnLevelLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            //Start loading resources
            LoadResources();

            //Optionally force opening of preview resource
            if (JResourceManager.Instance.AppMode == ApplicationMode.Preview)
            {
                string ResourceCode = JResourceManager.Instance.PreviewResourceCode;
                JResource[] resources = GameObject.FindObjectsOfType<JResource>();

                foreach (JResource r in resources)
                {
                    if (r.Code == ResourceCode)
                    {
                        r.Show();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Obtains the resource codes currently available on the scene
        /// </summary>
        /// <returns>An array of the resource codes currently available. </returns>
        string[] GetResourceCodes()
        {
            Dictionary<string, JResource> MappedResources;
            return GetResourceCodes(out MappedResources);
        }

        /// <summary>
        /// Fetches the resource data for this game and stores it for later initialization.
        /// </summary>
        public void Fetch()
        {
            StartCoroutine(DoFetch());
        }

        IEnumerator DoFetch()
        {
            Debug.Log("Fetching resource data...");

            using (UnityWebRequest www = UnityWebRequest.Get(FetchURL + "/recursos/" + SelectedResource.Id))
            {
                JRemoteSession.Instance.SetupRequestSession(www);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log("Error on request:" + www.error);
                }
                else
                {
                    Debug.Log("Fetch succesful: " + www.downloadHandler.text);
                    ParseResource(www.downloadHandler.text);
                    LoadResources();
                    OnFetchComplete?.Invoke();
                }
            }
        }

        void ParseResource(string JsonString)
        {
            ResourceData = new JSONObject(JsonString);
            ResourceContentDictionary.Clear();

            JSONObject JsonData = new JSONObject(JsonString);
            JSONObject Content = JsonData["contenidos"];

            for (int i = 0; i < Content.list.Count; i++)
            {
                JSONObject j = (JSONObject)Content.list[i];                

                JSONObject Code = j["identificador"];
                string CodeString = Code.type == JSONObject.Type.STRING ? Code.str : Code.ToString();
                Debug.Log("Found content with code: " + CodeString);

                ResourceContentDictionary[CodeString] = j;
            }
        }

        /// <summary>
        /// Called to start the answer data upload to the remote service
        /// </summary>
        public void PushAnswers()
        {
            StartCoroutine(DoPushAnswers());
        }

        IEnumerator DoPushAnswers()
        {
            Debug.Log("Pushing data...");

            //Create the answers and add them to a collection
            List<JSONObject> Objects = new List<JSONObject>();
            foreach(KeyValuePair<int, ResourceAnswer> KV in Answers)
            {
                Objects.Add(new JSONObject(KV.Value.AddJSONContents));
            }

            //Create a JSONObject for the collection itself
            JSONObject Collection = new JSONObject(Objects.ToArray());

            string Data = Collection.ToString();
            Debug.Log("Parsed Answers into: " + Data);
            using (UnityWebRequest www = UnityWebRequest.Put(FetchURL + "/recursos/" + SelectedResource.Id + "/respuestas", Data))
            {
                www.SetRequestHeader("Content-Type", "application/json");
                JRemoteSession.Instance.SetupRequestSession(www);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log("Error on request:" + www.error);
                }
                else
                {
                    Debug.Log("Push successful!");
                    OnPushComplete?.Invoke();
                }
            }
        }

        /// <summary>
        /// Loads every resource available on the level and updates their runtime info with the remote DB data.
        /// </summary>
        void LoadResources()
        {
            if(ResourceContentDictionary.Count > 0)
            {
                Debug.Log("Populating resource content...");

                //Obtain all the available codes
                Dictionary<string, JResource> MappedResources;
                string[] codes = GetResourceCodes(out MappedResources);

                //Data is separated as a dictionary
                foreach (KeyValuePair<string, JResource> KVPair in MappedResources)
                {
                    if(ResourceContentDictionary.ContainsKey(KVPair.Key))
                    {
                        JSONObject Data = ResourceContentDictionary[KVPair.Key];
                        KVPair.Value.SetupFromJsonData(Data);
                    }
                }
            }
        }

        /// <summary>
        /// Registers and notifies the resource manager that the player has access to the specified cloned resource, which will be processed to see if it corresponds to a compatible game.
        /// </summary>
        /// <param name="BaseId">Id of the base game (static resource)</param>
        /// <param name="Id">Id of the cloned game that will be played</param>
        /// <param name="Name">Human readable name used for distinguishing between clones</param>
        public void RegisterAvailableResource(string BaseId, string Id, string Name)
        {
            if(BaseId == BaseResourceCode)
            {
                //Add to the available resources and setup as the selected, we will use the latest clone, unless we manually change them via selector.
                AvailableResourceCodes.Add(Id);
                SelectedResource = new ResourceMetaData(Id, Name);
            }
        }

        /// <summary>
        /// Request that the following "data" be saved as an answer for the given content option
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ContentIdentifier"></param>
        /// <param name="QuestionIndex"></param>
        /// <param name="OptionIndex"></param>
        public void RegisterAnswer(string Data, string ContentIdentifier, int PageIndex, int OptionIndex)
        {
            ResourceAnswer Answer = new ResourceAnswer(ContentIdentifier, PageIndex, OptionIndex, Data);
            Answers[Answer.GetHashCode()] = Answer;
        }

        /// <summary>
        /// Tries to obtain an answer's "data" using the given parameter addresses.
        /// </summary>
        /// <param name="ContentIdentifier"></param>
        /// <param name="PageIndex"></param>
        /// <param name="OptionIndex"></param>
        /// <param name="Data"></param>
        /// <returns></returns>
        public bool GetAnswer(string ContentIdentifier, int PageIndex, int OptionIndex, out string Data)
        {
            ResourceAnswer SearchDummy = new ResourceAnswer(ContentIdentifier, PageIndex, OptionIndex);
            ResourceAnswer Answer;
            if(Answers.TryGetValue(SearchDummy.GetHashCode(), out Answer))
            {
                Data = Answer.Data;
                return true;
            }
            else
            {
                Data = "";
                return false;
            }
        }
    }

}
