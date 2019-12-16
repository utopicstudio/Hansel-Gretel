using UnityEngine;
using TMPro;

namespace J
{

	[AddComponentMenu("J/UI/JFadeUI")]
	public class JFadeUI : MonoBehaviour {

		enum UIAlphaStartMode {invisible, visible, visibleFadeIn, invisibleFadeOut}

        [Tooltip("If empty it uses this object's CanvasGroup")]
        [SerializeField]    CanvasGroup[] canvasGroup;
        [SerializeField]    TextMeshPro[] tmpro;
        [SerializeField]	UIAlphaStartMode startVisibility = UIAlphaStartMode.visible;
		[SerializeField]	float fadeInTime = 1f;
		[SerializeField]	float fadeOutTime = 1f;
        [SerializeField] UnityEngine.Events.UnityEvent[] OnFadeInStarting;
        [SerializeField] UnityEngine.Events.UnityEvent[] OnFadeOutEnded;

        private bool _startHasHappened = false;
        private UIAlphaStartMode _mode;

        private void OnValidate()
        {
            if (_startHasHappened && startVisibility != _mode)
            {
                _mode = startVisibility;
                //UpdateVisibilityState();
            }
        }

        private void Start () {
            _startHasHappened = true;
            _mode = startVisibility;

            //UpdateVisibilityState();
		}
        private void UpdateVisibilityState()
        {
            switch (startVisibility)
            {
                case UIAlphaStartMode.invisible:
                    HideInstantly();
                    break;
                case UIAlphaStartMode.visible:
                    ShowInstantly();
                    break;
                case UIAlphaStartMode.visibleFadeIn:
                    Show();
                    break;
                case UIAlphaStartMode.invisibleFadeOut:
                    Hide();
                    break;
                default:
                    break;
            }
        }
        


		public void Show() {
            gameObject.SetActive(true);

            print("JFadeUI.Show()");
            Invoke("CallOnFadeInStarting", 0f);
            foreach (var g in canvasGroup)
			    //J.Instance.followCurve (x => g.alpha = x, duration: fadeInTime, repeat: 1, type: CurveType.Linear);
                J2.Instance.JLerp(x => g.alpha = x, duration: fadeInTime, repeat: 1, type: CurveType.Linear);
            foreach (var g in tmpro)
                J2.Instance.JLerp(x => g.alpha = x, duration: fadeInTime, repeat: 1, type: CurveType.Linear);
                
        }
		public void Hide()
        {
            print("JFadeUI.Hide()");
            foreach (var g in canvasGroup)
			    //J.Instance.followCurve (x => g.alpha = x, duration: fadeOutTime, repeat: 1, type: CurveType.Linear, reverse: true);
                J2.Instance.JLerp(x => g.alpha = x, duration: fadeOutTime, repeat: 1, type: CurveType.Linear, reverse: true);
            foreach (var g in tmpro)
                J2.Instance.JLerp(x => g.alpha = x, duration: fadeOutTime, repeat: 1, type: CurveType.Linear, reverse: true);
            Invoke("CallOnFadeOutEnded", fadeOutTime);
        }
		public void ShowInstantly()
        {
            print("JFadeUI.ShowInstantly()");
            foreach (var g in canvasGroup)
                g.alpha = 1f;
            foreach (var g in tmpro)
                g.alpha = 1f;
            Invoke("CallOnFadeInStarting", 0f);
        }
		public void HideInstantly()
        {
            print("JFadeUI.HideInstantly()");
            foreach (var g in canvasGroup)
                g.alpha = 0f;
            foreach (var g in tmpro)
                g.alpha = 0f;
            Invoke("CallOnFadeOutEnded", 0f);
        }

		public void SetFadeInTime(float f) {
			this.fadeInTime = f;
		}
		public void SetFadeOutTime(float f) {
			this.fadeOutTime = f;
		}
        public void CallOnFadeInStarting()
        {
            foreach (var item in OnFadeInStarting)
                item.Invoke();
        }
        public void CallOnFadeOutEnded()
        {
            foreach (var item in OnFadeOutEnded)
                item.Invoke();
        }

    }

}