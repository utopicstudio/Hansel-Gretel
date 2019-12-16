using UnityEngine;

namespace J
{

    public class JTimeScale : MonoBehaviour
    {
        [Range(0f,10f)]
        [SerializeField] float timeScale = 1f;

        private void Start()
        {
            SetTimeScale(timeScale);
        }

        private void OnValidate()
        {
            SetTimeScale(timeScale);
        }
        public void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }

    }

}