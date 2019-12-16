using UnityEngine;

namespace J
{
    [AddComponentMenu("J/Util/J Camera Fade")]
    public class JCameraFade : MonoBehaviour
    {

        [Tooltip("Dejar vacio para que busque el tag MainCamera")]
        [SerializeField] Camera mainCamera;
        [SerializeField] Color fadeoutColor = Color.black;
        [SerializeField] Color transparentColor = Color.clear;
        [SerializeField] float fadeinTime = 1f;
        [SerializeField] float fadeoutTime = 1f;
        [SerializeField] bool ignoreTimeScale = false;
        [SerializeField] float distanceFromCamera = 0.5f;
        [SerializeField] private UnityEngine.UI.Image myImage;
        private string JFadeCanvasName = "JFadeCanvas";
        private string JFadeImageName = "JFadeImage";
        

        private GameObject block_screen_obj;
        private GameObject block_screen_img_obj;

        private void OnValidate()
        {
            if (block_screen_img_obj)
                block_screen_obj.transform.localPosition = new Vector3(0f, 0f, distanceFromCamera);
           
        }

        private void Start()
        {
            this.CreateFadeCanvasIfNeeded();
        }



        public void FadeIn()
        {
            _Fade(fadeinTime, this.transparentColor);
        }
        public void FadeOut()
        {
            _Fade(fadeoutTime, this.fadeoutColor);
        }
        public void FadeInInstantly()
        {
            _Fade(0f, this.fadeoutColor);
        }
        public void FadeOutInstantly()
        {
            _Fade(0f, this.transparentColor);
        }




        private void _Fade(float fadeDuration, Color targetColor)
        {
            CreateFadeCanvasIfNeeded();
            myImage.CrossFadeColor(targetColor, fadeDuration, ignoreTimeScale, true);
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
                    if (!mainCamera.transform.Find(JFadeCanvasName))
                    {
                        this.CreateImageInFrontOfCamera(mainCamera, JFadeCanvasName, JFadeImageName);
                    }
                }
                else
                    Debug.LogWarning("JWarning - JCameraFade: El primero objeto con tag MainCamera encontrado no tiene el componente Camera");
            }
            else
                Debug.LogWarning("JWarning - JCameraFade: No se encuentra el tag MainCamera en la escena");
        }

        private void CreateImageInFrontOfCamera(Camera cam, string canvasName, string imageName)
        {

            block_screen_obj = new GameObject(canvasName);
            block_screen_obj.transform.parent = cam.transform;


            Canvas cv = block_screen_obj.AddComponent<Canvas>();
            cv.renderMode = RenderMode.WorldSpace;
            block_screen_obj.transform.JReset();
            block_screen_obj.transform.localPosition = new Vector3(0f, 0f, distanceFromCamera);
            block_screen_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(2, 2);
            


            block_screen_img_obj = new GameObject(imageName);
            block_screen_img_obj.transform.parent = block_screen_obj.transform;
            block_screen_img_obj.transform.JReset();
            UnityEngine.UI.Image img_img = block_screen_img_obj.AddComponent<UnityEngine.UI.Image>();
            block_screen_img_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(2, 2);

            myImage = img_img;
            myImage.color = transparentColor;

        }


    }

}