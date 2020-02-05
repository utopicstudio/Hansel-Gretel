using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

namespace J
{
    //Signature for the login failed callback (Code, Message)
    public delegate void OnRemoteSessionLoginFailedSignature(int Code, string Error);
    public delegate void OnRemoteSessionLoginSuccess();

    /// <summary>
    /// Responsible of managing the sessions to the remote server, authenticating and configuring HTTP requests and making that session data persistent for the app.
    /// </summary>
    [AddComponentMenu("J/Auth/JRemoteSession")]
    public class JRemoteSession : MonoBehaviour
    {
        [System.Serializable]
        public struct RemoteSessionData
        {
            /// <summary>
            /// The user id for this session
            /// </summary>
            [SerializeField, ReadOnly]
            public string Id;

            /// <summary>
            /// The token that authenticates the user against the server
            /// </summary>
            [SerializeField, ReadOnly]
            public string Token;

            public RemoteSessionData(string InId, string InToken)
            {
                Id = InId;
                Token = InToken;
            }

            // Check for validity of this struct
            public bool IsValidSession()
            {
                return /*!string.IsNullOrEmpty(Id) && */ !string.IsNullOrEmpty(Token); //@NOTE: sessions are only valid when the tokens are valid
            }

            public override string ToString()
            {
                return "Id: " + Id + "\nToken: " + Token;
            }
        }

        /// <summary>
        /// Type of user session, affects login type
        /// </summary>
        public enum SessionType
        {
            Alumno,
            Profesor
        }

        //Delegate calls for async login
        private OnRemoteSessionLoginSuccess OnLogin;
        private OnRemoteSessionLoginFailedSignature OnLoginFailed;

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static JRemoteSession Instance { get; private set; }

        /// <summary>
        /// The remote URL to manage a session against
        /// </summary>
        [SerializeField]
        private string Url = "http://vrforkids.utopicstudio.com";

        /// <summary>
        /// Relative path to the login url.
        /// </summary>
        [SerializeField]
        private string LoginPath = "/login";

        /// <summary>
        /// Name of the user field that the login attempt expects
        /// </summary>
        [SerializeField]
        private string UserField = "email";

        /// <summary>
        /// The password for the account
        /// </summary>
        [SerializeField]
        private string TypeField = "tipo";

        /// <summary>
        /// Name of the password field that the login attempt expects
        /// </summary>
        [SerializeField]
        private string PasswordField = "password";
        
        /// <summary>
        /// Holds the data for the session
        /// </summary>
        private RemoteSessionData _SessionData;

        public RemoteSessionData SessionData
        {
            get { return _SessionData; }
        }

        // Called to setup needed data
        private void Awake()
        {
            Singleton();
        }

        /// <summary>
        /// Configures the singleton object
        /// </summary>
        private void Singleton()
        {
            if (Instance == null)
            {
                Instance = this;

#if UNITY_WEBGL
                InitWebArguments();
#endif
            }
            else if (Instance != this)
            {
                Destroy(this.gameObject);
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
            Value = UnityWebRequest.UnEscapeURL(Value);
            Debug.Log("parsing Key: " + Key + " - Value: " + Value);
            if (Key.ToLower() == "token")
            {
                //User token, should be stored on the session
                _SessionData.Token = Value;
            }
            else if (Key.ToLower() == "user")
            {
                //User id is not necessary, but could be inside
                _SessionData.Id = Value;
            }

        }
#endif

        /// <summary>
        /// Configures a UnityWebRequest with the session data
        /// </summary>
        /// <param name="Request">Request that we want to setup</param>
        public void SetupRequestSession(UnityWebRequest Request)
        {
            if(Request != null && SessionData.IsValidSession())
            {
                Request.SetRequestHeader("auth-token", SessionData.Token);
            }
        }

        /// <summary>
        /// Performs a login attempt
        /// </summary>
        /// <param name="User">The name of the user, as expected on the remote end.</param>
        /// <param name="Type">Type of session </param>
        /// <param name="Password">The password, unhashed</param>
        /// <param name="bPersistSession">If the login attempt should be persistent on the PlayerPrefs if successfull</param>
        /// <returns>Whether the login attempt worked or not</returns>
        public void AsyncLogin(string User, string Password, SessionType Type, bool bPersistSession = true, OnRemoteSessionLoginSuccess OnLoginDelegate = null, OnRemoteSessionLoginFailedSignature OnFailedDelegate = null)
        {
            OnLogin = OnLoginDelegate;
            OnLoginFailed = OnFailedDelegate;
            StartCoroutine(DoRequest(User, Password, Type, bPersistSession));
        }
        
        private IEnumerator DoRequest(string User, string Password, SessionType Type, bool bPersistSession)
        {
            string TypeAsString = Type == SessionType.Alumno ? "ALUMNO" : "PROFESOR";
            string PostData = "{" + string.Format("\"{0}\":\"{1}\",\"{2}\":\"{3}\",\"{4}\":\"{5}\"", UserField, User, PasswordField, Password, TypeField, TypeAsString) + "}";
            
            //Send the request, Unity has an issue with POST requests and JSON, so we just use a PUT and change the method later.
            using (UnityWebRequest Request = UnityWebRequest.Put(Url + LoginPath, PostData))
            {
                Request.SetRequestHeader("Content-Type", "application/json");
                Request.method = UnityWebRequest.kHttpVerbPOST;
                yield return Request.SendWebRequest();

                if(Request.isNetworkError || Request.isHttpError)
                {
                    Debug.Log("Error on request:" + Request.error);

                    OnLoginFailed?.Invoke((int)Request.responseCode, Request.error);
                }
                else
                {
                    ParseLoginResponse(Request, bPersistSession);
                    OnLogin?.Invoke();
                }
            }
        }

        /// <summary>
        /// Used for parsing the returned data  when a login attempt is performed
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="bPersistSession"></param>
        private void ParseLoginResponse(UnityWebRequest Response, bool bPersistSession)
        {
            Debug.Log("Login Response: " + Response.downloadHandler.text);
            JSONObject JsonResponse = new JSONObject(Response.downloadHandler.text);
            
            //Parse the token
            for (int i = 0; i < JsonResponse.list.Count; i++)
            {
                string key = (string)JsonResponse.keys[i];
                JSONObject j = (JSONObject)JsonResponse.list[i];
                
                switch(key)
                {
                    case "token":
                        _SessionData.Token = j.str;
                        break;
                    case "respuesta":
                        //Need to go one level deeper
                        if(j.IsObject)
                        {
                            for(int k = 0; k < j.list.Count; k++)
                            {
                                key = (string)j.keys[k];
                                if(key == "id")
                                {
                                    _SessionData.Id = j.list[k].str;
                                }
                            }
                        }
                        break;
                    case "recursos":
                        if(j.IsArray)
                        {
                            for (int k = 0; k < j.list.Count; k++)
                            {
                                //Contains the clone resource data
                                JSONObject CloneEntry = j.list[k];
                                string BaseId = CloneEntry["id_base"].str;
                                string Id = CloneEntry["id"].str;
                                string Name = CloneEntry["nombre"].str;

                                JResourceManager.Instance.RegisterAvailableResource(BaseId, Id, Name);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            Debug.Log("Parsed Login response: " + _SessionData.ToString());
        }
    }
}
