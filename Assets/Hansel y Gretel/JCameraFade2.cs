using UnityEngine;

namespace J
{
    [AddComponentMenu("J/Util/JCameraFade2")]
    public class JCameraFade2 : MonoBehaviour
    {

        [Header("Llama a este script con JAction")]
        [Tooltip("Dejar vacio para que busque el tag MainCamera")]
        [SerializeField] Camera mainCamera;
        [SerializeField] Color opaqueColor = Color.black;
        [SerializeField] Color transparentColor = Color.clear;
        public float fadeinTime = 1f;
        public float fadeoutTime = 1f;
        [SerializeField] bool ignoreTimeScale = false;
        [SerializeField] float distanceFromCamera = 0.05f;
        public bool fadeInAtStart = true;

        private UnityEngine.UI.Image imgInFront;
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
            if (!imgInFront)
                this.CreateFadeCanvasIfNeeded(true);
        }


        public void JSetFadeInTime(float t)
        {
            this.fadeinTime = t;
        }
        public void JSetFadeOutTime(float t)
        {
            this.fadeoutTime = t;
        }
        public void JFadeIn()
        {
            print("JFadeIn called");
            _Fade(fadeinTime, this.transparentColor);
        }
        public void JFadeOut()
        {
            print("JFadeOut called");
            _Fade(fadeoutTime, this.opaqueColor);
        }
        public void JFadeInInstantly()
        {
            print("JFadeInInstantly called");
            _Fade(0f, this.transparentColor);
        }
        public void JFadeOutInstantly()
        {
            print("JFadeOutInstantly called");
            _Fade(0f, this.opaqueColor);
        }




        private void _Fade(float fadeDuration, Color targetColor)
        {
            if (!imgInFront)
                CreateFadeCanvasIfNeeded(false);
            if (!imgInFront)
                return;
            print("_Fade :: imgInFront exists");

            Color imgColorBeforeLerp = imgInFront.color;
            if (fadeDuration > 0f)
            {
                J2.Instance.JLerp((x) =>
                {
                   imgInFront.color = Color.Lerp(imgColorBeforeLerp, targetColor, x);
                }, fadeDuration);
            } else
            {
                imgInFront.color = targetColor;
            }
            print("camera _Fade() called");
        }


        /// <summary>
        /// Crea un canvas que tapa a la cámara si es que aún no existe. Busca la primera cámara con el tag MainCamera
        /// </summary>
        private void CreateFadeCanvasIfNeeded(bool calledAtStart)
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
                        this.CreateImageInFrontOfCamera(mainCamera, JFadeCanvasName, JFadeImageName, calledAtStart);
                    }
                }
                else
                    Debug.LogWarning("JWarning - JCameraFade: El primero objeto con tag MainCamera encontrado no tiene el componente Camera");
            }
            else
                Debug.LogWarning("JWarning - JCameraFade: No se encuentra el tag MainCamera en la escena");
        }

        private void CreateImageInFrontOfCamera(Camera cam, string canvasName, string imageName, bool calledAtStart)
        {

            block_screen_obj = new GameObject(canvasName);
            block_screen_obj.transform.parent = cam.transform;


            Canvas cv = block_screen_obj.AddComponent<Canvas>();
            cv.renderMode = RenderMode.WorldSpace;
            cv.sortingOrder = 100;
            block_screen_obj.transform.JReset();
            block_screen_obj.transform.localPosition = new Vector3(0f, 0f, distanceFromCamera);
            block_screen_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(2, 2);


            block_screen_img_obj = new GameObject(imageName);
            block_screen_img_obj.transform.parent = block_screen_obj.transform;
            block_screen_img_obj.transform.JReset();
            UnityEngine.UI.Image img_img = block_screen_img_obj.AddComponent<UnityEngine.UI.Image>();
            img_img.raycastTarget = false;

            block_screen_img_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(2, 2);
            
            imgInFront = img_img;
            imgInFront.color = opaqueColor;


            // Initial camera state
            if (calledAtStart)
            {
                print(calledAtStart);
                if (fadeInAtStart)
                    this.JFadeIn();
                else
                    this.JFadeInInstantly();
            }


        }


    }

}