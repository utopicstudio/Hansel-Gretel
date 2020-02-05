using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

namespace J
{
    [System.Serializable]
    public class TypeGameObjectValuePair : System.Object
    {
        public ContentType Key;
        public GameObject Value;
    }

    /// <summary>
    /// The type of content that conforms a resource page.
    /// </summary>
    public enum ContentType
    {
        Text,
        Question,
        Assertion,
        Pairs_Text,
        Pairs_Images,
        Pairs_Image_Text,
    }

    /// <summary>
    /// Main class for the dynamic resource.
    /// </summary>
    [AddComponentMenu("J/Resources/Resource")]
    public class JResource : MonoBehaviour
    {
        /// <summary>
        /// Internal class that holds the logic for page on this resource
        /// </summary>
        class ContentPage
        {
            /// <summary>
            /// Generates a content page given a JsonObject 
            /// </summary>
            /// <param name="Data">JsonObject holding the data</param>
            /// <returns> All the content pages generated via the parsed json object.</returns>
            public static ContentPage[] ParseFromJsonData(JSONObject Data)
            {
                List<ContentPage> ListC = new List<ContentPage>();
                foreach (JSONObject j in Data.list)
                {
                    ListC.Add(new ContentPage(j));
                }

                return ListC.ToArray();
            }

            public ContentPage(JSONObject Data)
            {
                for (int i = 0; i < Data.list.Count; i++)
                {
                    string key = (string)Data.keys[i];
                    JSONObject j = (JSONObject)Data.list[i];

                    //How we parse the json data depends of the type of content type we're receiving
                    switch (key)
                    {
                        case "texto":
                            this.Data = System.Text.RegularExpressions.Regex.Unescape(j.str);
                            break;
                        case "imagen":
                            this.ImagenURL = j.str != null ? j.str.Replace("\\", "") : "";
                            break;
                        case "tipo":
                            this.Type = ParseType(j.str);
                            break;
                        case "opciones":
                            this.Options = ContentOption.ParseFromJsonData(j);
                            break;
                        case "indice":
                            this.Index = int.Parse(j.ToString());
                            break;
                        default:
                            break;
                    }

                }
            }

            /// <summary>
            /// Obtains a content type given the raw string data
            /// </summary>
            /// <param name="Type">String with the type to be parsed</param>
            /// <returns>Content type that pairs with the data given, defaults to TEXT</returns>
            private ContentType ParseType(string Type)
            {
                Type = Type.ToUpper();
                switch (Type)
                {
                    case "TEXTO":
                        return ContentType.Text;
                    case "ALTERNATIVA":
                        return ContentType.Question;
                    case "VERDADERO_FALSO":
                        return ContentType.Assertion;
                    case "UNIR_TEXTOS":
                        return ContentType.Pairs_Text;
                    case "UNIR_IMAGENES":
                        return ContentType.Pairs_Images;
                    case "UNIR_IMAGEN_TEXTO":
                        return ContentType.Pairs_Image_Text;
                    default:
                        return ContentType.Text;
                }
            }

            /// <summary>
            /// Tries to obtain a remote image if its already cached, or being downloading if not
            /// </summary>
            /// <param name="Instigator">Resource who needs to be updated when the image is loaded.</param>
            /// <returns>The cached image, or null if it hasn't been downloaded yet.</returns>
            public Texture2D TryGetImage(JResource Instigator)
            {
                if (Imagen)
                {
                    return Imagen;
                }
                else
                {
                    //Image not loaded yet will query this object to receive the image when we load it
                    BeginLoadImage(Instigator);
                    return null;
                }
            }

            /// <summary>
            /// Starts remote loading of an image, if no image is currently being loaded.
            /// </summary>
            /// <param name="Instigator">Resource who needs to be updated when the image is loaded.</param>
            private void BeginLoadImage(JResource Instigator)
            {
                if (!IsLoadingImage)
                {
                    CallbackLoader = Instigator;
                    IsLoadingImage = true;
                    Instigator.StartCoroutine(LoadImage());
                }
            }

            /// <summary>
            /// Async loading of the content page main image.
            /// </summary>
            /// <returns></returns>
            private IEnumerator LoadImage()
            {
                Debug.Log("Starting Laod Image from url: " + ImagenURL);
                using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(ImagenURL))
                {
                    yield return www.SendWebRequest();

                    if (www.error != null)
                    {
                        Debug.LogError(www.error);
                        IsLoadingImage = false;
                    }
                    else
                    {
                        Imagen = DownloadHandlerTexture.GetContent(www);
                        FinishLoadImage();
                    }
                }
            }

            /// <summary>
            /// Calls delegates when an image has completely loaded
            /// </summary>
            private void FinishLoadImage()
            {
                CallbackLoader.OnImageLoaded(Imagen, this);
                IsLoadingImage = false;
                Debug.Log("Image Loaded: " + Imagen);
            }


            /// <summary>
            /// The type of content this page holds
            /// </summary>
            public ContentType Type;

            /// <summary>
            /// General main data field for this page.
            /// </summary>
            public string Data;

            /// <summary>
            /// Url of the support image of this page.
            /// </summary>
            public string ImagenURL;

            /// <summary>
            /// Index of this content page
            /// </summary>
            public int Index;

            /// <summary>
            /// Options for this page, if it has any question to be answered.
            /// </summary>
            public ContentOption[] Options;

            /// <summary>
            /// Support image that gets loaded Async from the ImageURL
            /// </summary>
            private Texture2D Imagen;

            /// <summary>
            /// Object that requested the load of the image, and wishes to be notified when it's completed.
            /// </summary>
            private JResource CallbackLoader;

            /// <summary>
            /// Flag to know if we're currently loading the secondary image
            /// </summary>
            private bool IsLoadingImage;
        }

        public class ContentOption
        {
            public static ContentOption[] ParseFromJsonData(JSONObject Data)
            {
                List<ContentOption> ListC = new List<ContentOption>();
                for (int i = 0; i < Data.list.Count; i++)
                {
                    JSONObject j = Data.list[i];
                    ListC.Add(new ContentOption(j));
                }

                return ListC.ToArray();
            }

            public ContentOption(JSONObject Data)
            {
                JsonData = Data;
                Index = (int)Data["numero_alternativa"].i;
            }

            //Methods for getting values
            public bool GetValueAsBool(string Key)
            {
                return (bool)JsonData[Key];
            }

            public string GetValueAsString(string Key)
            {
                return System.Text.RegularExpressions.Regex.Unescape(JsonData[Key].str);
            }

            /// <summary>
            /// Contains the JsonData that represents this object
            /// </summary>
            private JSONObject JsonData;

            /// <summary>
            /// Answer data stored as an string.
            /// </summary>
            public string AnswerData;

            /// <summary>
            /// Index that this answer is being shown at
            /// </summary>
            public int Index;
        }

        public UnityEvent OnShownEvent;
        public UnityEvent OnHiddenEvent;

        private bool bShown = false;
        private bool bDeferedOpen = false;

        /// <summary>
        /// Represents this resource code on the remote webservice
        /// </summary>
        public string Code;

        /// <summary>
        /// If the content of this resource should be treated as sequential, i.e. block buttons and force the player to complete each page sequentially
        /// </summary>
        public bool HasSequentialContent = false;

        /// <summary>
        /// If the resource starts opened.
        /// </summary>
        public bool bOpenOnStart = false;

        /// <summary>
        /// Exit button of the resource, used for sequential content
        /// </summary>
        public Button ExitButton;

        /// <summary>
        /// Next button of the resource, used for sequential content
        /// </summary>
        public Button NextButton;

        /// <summary>
        /// Prev button of the resource, used for sequential content
        /// </summary>
        public Button PrevButton;

        /// <summary>
        /// Prefabs to use for each Type of content option when generating a page.
        /// </summary>
        public TypeGameObjectValuePair[] BlueprintPrefabs;

        /// <summary>
        /// Indexed dictionary for ease of use.
        /// </summary>
        private Dictionary<ContentType, GameObject> _blueprintPrefabs;

        /// <summary>
        /// Main title for this resource, gets rendered as a header.
        /// </summary>
        private string Title;

        /// <summary>
        /// Description for this resource. Data only, doesn't get rendered.
        /// </summary>
        private string Description;

        /// <summary>
        /// Ordered list of content pages that conform this resource
        /// </summary>
        private ContentPage[] Pages;

        /// <summary>
        /// Temporary image to use when we're still loading an image.
        /// </summary>
        public Texture2D LoadingImage;

        //The detail zoom to use
        //private ZoomInDetail DetailZoom; //@TODO: this will be removed most likely

        /// <summary>
        /// Index of the current page we're showing right now
        /// </summary>
        private int CurrentPage;

        //The UI to show
        public GameObject UI;

        //Ref position to use when aligning transforms
        public Transform RefPosition;

        //UI Title
        private TextMeshPro TitleText;

        //UI Detail
        private TextMeshPro DetailText;

        //UI Image Wrapper
        private GameObject ImageWrapper;

        //UI Image
        private UnityEngine.UI.Image ContentImage;

        //UI Image Wrapper
        private GameObject OptionListWrapper;

        /// <summary>
        /// The listview controller of the option list
        /// </summary>
        private JListViewController ListController;

        //Generates the data from the json string given
        public void SetupFromJsonData(JSONObject Data)
        {
            for (int i = 0; i < Data.list.Count; i++)
            {
                string key = (string)Data.keys[i];
                JSONObject j = (JSONObject)Data.list[i];

                //Setup the value depending of which 
                switch (key)
                {
                    case "nombre":
                        this.Title = j.str;
                        break;
                    case "descripcion":
                        this.Description = j.str;
                        break;
                    case "preguntas":
                        //This is an array
                        Pages = ContentPage.ParseFromJsonData(j);
                        break;
                    default:
                        break;
                }

            }

            //The title is common, so we should set it up here
            TitleText.text = Title;

            //Check if we're open, we should reset the current page
            if (bShown)
                GoToPage(CurrentPage);
        }

        void Awake()
        {
            //DetailZoom = GetComponent<ZoomInDetail>();

            //UI Components, search for the text ones
            //UnityEngine.UI.Text[] TextComponents = transform.GetComponentsInChildren<UnityEngine.UI.Text>(true);
            TextMeshPro[] TextComponents = transform.GetComponentsInChildren<TextMeshPro>(true);
            foreach (TextMeshPro T in TextComponents)
            {
                if (T.gameObject.CompareTag("ResourceTitle"))
                {
                    TitleText = T;
                }
                else if (T.gameObject.CompareTag("ResourceDetail"))
                {
                    DetailText = T;
                }
            }

            //Image objects and wrappers
            UnityEngine.UI.Image[] ImageComponents = transform.GetComponentsInChildren<UnityEngine.UI.Image>(true);
            foreach (UnityEngine.UI.Image T in ImageComponents)
            {
                if (T.gameObject.CompareTag("ResourceImageWrapper"))
                {
                    ContentImage = T;
                    ImageWrapper = ContentImage.transform.parent.gameObject;
                    break;
                }
            }

            //Look for the optionlist
            ListController = transform.GetComponentInChildren<JListViewController>(true);
            OptionListWrapper = ListController.transform.parent.gameObject; //Looks awful, but the scroll has always this structure


            if (!TitleText || !DetailText || !ImageWrapper || !OptionListWrapper || !ListController)
            {
                Debug.LogError("UI Components not found, maybe you changed their tag?");
            }

            /*if (!DetailZoom)
            {
                Debug.LogError("ZoomInDetail component not found, cannot show");
            }*/

            if (!UI)
            {
                Debug.LogError("No UI Component bound, cannot continue");
            }

            //Generate the dictionary using the serializable keyvalue pair
            _blueprintPrefabs = new Dictionary<ContentType, GameObject>();
            foreach (TypeGameObjectValuePair KV in BlueprintPrefabs)
            {
                _blueprintPrefabs.Add(KV.Key, KV.Value);
            }

            //Manually hide, we don't want to start showing resources without them being selected
            UI.SetActive(false);
        }

        private void Start()
        {
            //Check for auto open config, only when on normal application mode
            bDeferedOpen = bOpenOnStart && J.Instance.AppMode == ApplicationMode.Normal;
        }

        private void Update()
        {
            if (bDeferedOpen)
            {
                Show();
                bDeferedOpen = false;
            }
        }

        /// <summary>
        /// Shows the resource, displaying the first page if not specified and moving the player to the "Interest" point
        /// </summary>
        public void Show(int ShowPage = 0)
        {
            //Has to be first... if not, the internal IEnumerator from the yield will not run (as its deactivated)
            UI.SetActive(true);

            //Show the page if valid, if not, fallback to first page
            if (!GoToPage(ShowPage))
            {
                GoToPage(0);
            }

            //Invoke the event
            OnShownEvent.Invoke();
            bShown = true;
        }

        /// <summary>
        /// Hides this control, returning control and position to the controller
        /// </summary>
        public void Hide()
        {
            //Chance to persist the answers into local repository
            StoreOptionAnswers();
            UI.SetActive(false);
            OnHiddenEvent.Invoke();
            bShown = false;
        }

        /// <summary>
        /// Given a gameobject transform, it moves said transform to the RefPosition
        /// </summary>
        /// <param name="T"></param>
        public void AlignTransform(Transform T)
        {
            if(RefPosition)
            {
                T.position = RefPosition.position;
                T.rotation = transform.rotation;
            }
        }

        /// <summary>
        /// Goes to the specified page and configures it accordingly
        /// </summary>
        /// <param name="InPage">The index of the page to go to</param>
        /// <returns>If the page was indeed changed</returns>
        private bool GoToPage(int InPage)
        {
            if (Pages == null || InPage < 0 || InPage >= Pages.Length)
            {
                return false;
            }

            //Update the current index
            CurrentPage = InPage;

            //Change the content here
            ContentPage currentContent = Pages[InPage];
            DetailText.text = currentContent.Data;

            //Try to setup the image if there is one
            if (!string.IsNullOrEmpty(currentContent.ImagenURL))
            {
                //First we should enable the image wrapper
                ImageWrapper.SetActive(true);

                //There's an image url, should try to get it if it exists
                Texture2D image = currentContent.TryGetImage(this);
                if (image)
                {
                    ContentImage.overrideSprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0, 0));
                }
                else
                {
                    ContentImage.overrideSprite = Sprite.Create(LoadingImage, new Rect(0, 0, LoadingImage.width, LoadingImage.height), new Vector2(0, 0));
                }

            }
            else
            {
                //No image wrapper needed
                ImageWrapper.SetActive(false);
            }

            //Check if we should update the list area
            if (UsesOptionListArea(currentContent))
            {
                OptionListWrapper.SetActive(true);
                ListController.Clear();

                FillOptionList(currentContent);
            }
            else
            {
                OptionListWrapper.SetActive(false);
            }

            //Optionally block the progress UI
            ConditionalLockProgressUI();

            return true;
        }

        /// <summary>
        /// Goes to the next page if valid, and configures it accordingly
        /// </summary>
        public void NextPage()
        {
            GoToPage(CurrentPage + 1);
        }

        /// <summary>
        /// Goes to the previous page if valid, and configures it accordingly
        /// </summary>
        public void PrevPage()
        {
            GoToPage(CurrentPage - 1);
        }

        private bool UsesOptionListArea(ContentPage content)
        {
            return content.Type != ContentType.Text;
        }

        private void FillOptionList(ContentPage content)
        {
            IRenderOptionFactory factory = _blueprintPrefabs[content.Type].GetComponent<JRenderOption>().GetFactory();
            JRenderOption[] options = factory.BuildRenderOptions(content.Options);

            foreach (JRenderOption Opt in options)
            {
                ListController.Add(Opt.gameObject);

                //Add a delegate for when the value changes, to make sure we're unlocking progress when needed
                Opt.OnAnswerValueChange += ConditionalLockProgressUI;
            }

        }

        /// <summary>
        /// Called when we asked for an image when it was still loading and now has completed
        /// </summary>
        /// <param name="Image">The image loaded</param>
        /// <param name="From">The caller</param>
        private void OnImageLoaded(Texture2D Image, ContentPage From)
        {
            //Check that we still want this image (could have changed page)
            if (Pages[CurrentPage] == From)
            {
                ContentImage.overrideSprite = Sprite.Create(Image, new Rect(0, 0, Image.width, Image.height), new Vector2(0, 0));
            }
        }

        /// <summary>
        /// Stores every content option answer into the Manager's database
        /// </summary>
        private void StoreOptionAnswers()
        {
            foreach(ContentPage Page in Pages)
            {
                //Options can be null in a text ContentPage
                if(Page.Options != null)
                {
                    foreach (ContentOption Opt in Page.Options)
                    {
                        if (Opt.AnswerData != null)
                        {
                            JResourceManager.Instance.RegisterAnswer(Opt.AnswerData, Code, Page.Index, Opt.Index);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the accessibility of the progress UI, depending of the state of the page's content
        /// </summary>
        private void ConditionalLockProgressUI()
        {
            if(ExitButton != null && NextButton != null && PrevButton != null)
            {
                if (HasSequentialContent)
                {
                    ContentPage Page = Pages[CurrentPage];

                    //We start with the general case and then find exceptions 
                    //questions start locked and need only one answer to unlock, whereas other start unlocked and need only one unanswered option to be locked
                    bool bLockedPage = Page.Type == ContentType.Question;
                    if (Page != null && Page.Options != null && Page.Options.Length > 0)
                    {
                        foreach(ContentOption Option in Page.Options)
                        {
                            if(Option.AnswerData == null)
                            {
                                //Empty answer, we can only continue if this isn't a alternative question
                                if(Page.Type != ContentType.Question)
                                {
                                    bLockedPage = true;
                                    break;
                                }
                            }
                            else
                            {
                                //Non empty answer is enough for a question type
                                if (Page.Type == ContentType.Question)
                                {
                                    bLockedPage = false;
                                    break;
                                }
                            }
                        }
                    }

                    //Check if the content page is locked
                    if(bLockedPage)
                    {
                        ExitButton.gameObject.SetActive(false);
                        NextButton.gameObject.SetActive(false);
                        PrevButton.gameObject.SetActive(CurrentPage != 0);
                    }
                    else
                    {
                        //Page isn't locked, we enable everything, except the ExitButton and NextButton which are exclusive
                        ExitButton.gameObject.SetActive(CurrentPage == Pages.Length - 1);
                        NextButton.gameObject.SetActive(!ExitButton.gameObject.activeSelf);
                        PrevButton.gameObject.SetActive(CurrentPage != 0);
                    }
                    
                }
                else
                {
                    //Enable all progress UI
                    ExitButton.gameObject.SetActive(true);
                    NextButton.gameObject.SetActive(CurrentPage < Pages.Length - 1);
                    PrevButton.gameObject.SetActive(CurrentPage != 0);
                }
            }
        }
    }

}