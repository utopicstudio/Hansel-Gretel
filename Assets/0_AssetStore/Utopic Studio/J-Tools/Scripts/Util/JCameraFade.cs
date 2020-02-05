using UnityEngine;

namespace J
{
    [AddComponentMenu("J/Util/JCameraFade")]
    public class JCameraFade : JBase
    {

        [Header("Llama a este script con JAction")]
        [Tooltip("Dejar vacio para que busque el tag MainCamera")]
        [SerializeField] Camera mainCamera;
        [SerializeField] Color opaqueColor = Color.black;
        [SerializeField] Color transparentColor = Color.clear;
        [SerializeField] float fadeinTime = 1f;
        [SerializeField] float fadeoutTime = 1f;
        [SerializeField] bool ignoreTimeScale = false;
        [SerializeField] float distanceFromCamera = 0.5f;
        [SerializeField] int orderInLayer = 100;
        public bool fadeInAtStart = true;

        private UnityEngine.UI.Image myImage;
        private string JFadeCanvasName = "JFadeCanvas";
        private string JFadeImageName = "JFadeImage";

        private GameObject block_screen_obj;
        private GameObject block_screen_img_obj;

        #region Property Get/Set
        public float FadeInTime
        {
            get { return fadeinTime; }
            set { fadeinTime = value; }
        }

        public float FadeOutTime
        {
            get { return fadeoutTime; }
            set { fadeoutTime = value; }
        }
        #endregion

        private void OnValidate()
        {
            if (block_screen_img_obj)
                block_screen_obj.transform.localPosition = new Vector3(0f, 0f, distanceFromCamera);
        }

        private void Start()
        {
            this.CreateFadeCanvasIfNeeded();

            // Initial camera state
            if (fadeInAtStart)
            {
                FadeOutInstantly();
                FadeIn();
            }
            /*else
            {
                FadeInInstantly();
            }*/
        }



        public void FadeIn()
        {
            _Fade(fadeinTime, this.transparentColor);
        }
        public void FadeOut()
        {
            _Fade(fadeoutTime, this.opaqueColor);
        }
        public void FadeInInstantly()
        {
            _Fade(0f, this.transparentColor);
        }
        public void FadeOutInstantly()
        {
            _Fade(0f, this.opaqueColor);
        }

        #region deprecated
        public void SetFadeInTime(float t)
        {
            this.fadeinTime = t;
        }
        public void SetFadeOutTime(float t)
        {
            this.fadeoutTime = t;
        }
        #endregion




        private void _Fade(float fadeDuration, Color targetColor)
        {
            CreateFadeCanvasIfNeeded();

            if (fadeDuration > 0)
            {
                Color imgColorBeforeLerp = myImage.color;
                J.Instance.Lerp((x) =>
               {
                   myImage.color = Color.Lerp(imgColorBeforeLerp, targetColor, x);
               }, fadeDuration);
            }
            else
            {
                myImage.color = targetColor;
            }
        }


        /// <summary>
        /// Crea un canvas que tapa a la cámara si es que aún no existe. Busca la primera cámara con el tag MainCamera
        /// </summary>
        private void CreateFadeCanvasIfNeeded()
        {
            GameObject camobj = GameObject.FindGameObjectWithTag("MainCamera");
            if (camobj)
            {
                Camera cam = camobj.GetComponent<Camera>();
                if (cam)
                {
                    mainCamera = cam;
                    Transform JFadeCanvasTransform = mainCamera.transform.Find(JFadeCanvasName);
                    if (!JFadeCanvasTransform)
                    {
                        CreateImageInFrontOfCamera(mainCamera, JFadeCanvasName, JFadeImageName);
                    }
                    else
                    {
                        FindImageInCanvas(JFadeCanvasTransform, JFadeImageName);
                    }
                }
                else
                {
                    Debug.LogWarning("JWarning - JCameraFade: El primero objeto con tag MainCamera encontrado no tiene el componente Camera");
                }
            }
            else
            {
                Debug.LogWarning("JWarning - JCameraFade: No se encuentra el tag MainCamera en la escena");
            }
        }

        private void CreateImageInFrontOfCamera(Camera cam, string canvasName, string imageName)
        {

            block_screen_obj = new GameObject(canvasName);
            block_screen_obj.transform.parent = cam.transform;


            Canvas cv = block_screen_obj.AddComponent<Canvas>();
            cv.renderMode = RenderMode.WorldSpace;
            cv.sortingOrder = orderInLayer;
            block_screen_obj.transform.JReset();
            block_screen_obj.transform.localPosition = new Vector3(0f, 0f, distanceFromCamera);
            block_screen_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(2, 2);

            block_screen_img_obj = new GameObject(imageName);
            block_screen_img_obj.transform.parent = block_screen_obj.transform;
            block_screen_img_obj.transform.JReset();
            UnityEngine.UI.Image img_img = block_screen_img_obj.AddComponent<UnityEngine.UI.Image>();
            img_img.raycastTarget = false;

            block_screen_img_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(2, 2);
            
            myImage = img_img;
            myImage.color = transparentColor;

        }

        private void FindImageInCanvas(Transform JFadeCanvasTransform, string ImageName)
        {
            Transform imageTransform = JFadeCanvasTransform.Find(ImageName);
            if(imageTransform)
            {
                myImage = imageTransform.gameObject.GetComponent<UnityEngine.UI.Image>();
            }
        }

    }

}