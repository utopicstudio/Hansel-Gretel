using UnityEngine;
using UnityEngine.EventSystems;

namespace J
{


    [AddComponentMenu("J/VR/JVRButton")]
    public class JVRButton : MonoBehaviour, UnityEngine.EventSystems.IPointerEnterHandler, UnityEngine.EventSystems.IPointerExitHandler, UnityEngine.EventSystems.IPointerClickHandler
    {

        [SerializeField]    UnityEngine.EventSystems.EventTrigger target;
        [Min(0f)]
        [SerializeField]    float waitTime = 2.5f;
        [SerializeField]    AudioClip clickSound;
        [Range(0f, 1f)]
        [SerializeField]    float vol = 0.7f;

        /// <summary>
        /// If true, the button itself count downs and triggers the button action by itself.
        /// </summary>
        public bool UseInternalWaitTime = false;

        private float waitTimePrivate = 0f;
        private UnityEngine.EventSystems.EventTrigger eventTriggerComponent;

        private void Start()
        {
            if (target)
                eventTriggerComponent = target;
            else
                eventTriggerComponent = GetComponent<UnityEngine.EventSystems.EventTrigger>();
        }

        private void Update()
        {
            if (UseInternalWaitTime && waitTimePrivate > 0f)
            {
                waitTimePrivate -= Time.deltaTime;
                if (waitTimePrivate <= 0f)
                {
                    ButtonClickAfterCooldown();
                }
            }
        }

        #region IPointerEnterHandler implementation

        public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
        {
            waitTimePrivate = waitTime;
        }

        #endregion

        #region IPointerExitHandler implementation

        public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
        {
            waitTimePrivate = 0f;
        }

        #endregion

        #region IPointerClickHandler implementation
        public void OnPointerClick(PointerEventData eventData)
        {
            PlayClickSound(clickSound);
        }
        #endregion

        private void ButtonClickAfterCooldown()
        {
            eventTriggerComponent.OnPointerClick(null);
            PlayClickSound(clickSound);
        }
        private void PlayClickSound(AudioClip clip)
        {
            J.Instance.PlaySound(clip, vol);
        }
    }
}