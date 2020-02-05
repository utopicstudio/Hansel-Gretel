using UnityEngine;

namespace J
{

    [AddComponentMenu("J/UI/JFadeUI")]
    public class JFadeUI : JBase
    {

        enum UIAlphaStartMode { invisible, visible, visibleFadeIn, invisibleFadeOut }

        [Tooltip("Dejar vacío si el CanvasGroup está en este objeto")]
        [SerializeField] CanvasGroup[] target;
        [SerializeField] UIAlphaStartMode state = UIAlphaStartMode.visible;
        [SerializeField] float fadeInTime = 1f;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] bool disableWhenHidden = true;
        [SerializeField] UnityEngine.Events.UnityEvent DoAfterFadeIn;
        [SerializeField] UnityEngine.Events.UnityEvent DoAfterFadeOut;

        private bool _start_was_called = false;
        UIAlphaStartMode _state;

        private void OnValidate()
        {
            if (_start_was_called)
            {
                UpdateMode();
            }
        }
        private void Reset()
        {
            target = new CanvasGroup[1];
            CanvasGroup cg = gameObject.GetComponent<CanvasGroup>();
            if (cg)
                target[0] = cg;
        }
        private void Start()
        {
            UpdateMode();
            _start_was_called = true;
        }


        public void JShow()
        {
            state = UIAlphaStartMode.visibleFadeIn;
            UpdateMode();
        }
        public void JHide()
        {
            state = UIAlphaStartMode.invisibleFadeOut;
            UpdateMode();
        }
        public void JShowInstantly()
        {
            state = UIAlphaStartMode.visible;
            UpdateMode();
        }
        public void JHideInstantly()
        {
            state = UIAlphaStartMode.invisible;
            UpdateMode();
        }

        // Se mantiene por compatibilidad
        public void Show()
        {
            JShow();
        }
        // Se mantiene por compatibilidad
        public void Hide()
        {
            JHide();
        }
        // Se mantiene por compatibilidad
        public void ShowInstantly()
        {
            JShowInstantly();
        }
        // Se mantiene por compatibilidad
        public void HideInstantly()
        {
            JHideInstantly();
        }

        private void UpdateMode()
        {
            if (!_start_was_called || ModeChanged())
            {
                switch (state)
                {
                    case UIAlphaStartMode.invisible:
                        _HideInstantly();
                        break;
                    case UIAlphaStartMode.visible:
                        _ShowInstantly();
                        break;
                    case UIAlphaStartMode.visibleFadeIn:
                        _Show();
                        break;
                    case UIAlphaStartMode.invisibleFadeOut:
                        _Hide();
                        break;
                    default:
                        break;
                }
                _state = state;
            }
        }

        private bool ModeChanged()
        {
            return state != _state;
        }

        private void DoOnFadeInEnded()
        {
            foreach (var g in target)
            {
                if (g)
                {
                    //Make sure the fade was completed (algorithm not necessarily completes the curve)
                    g.alpha = 1.0f;
                }
            }

            DoAfterFadeIn.Invoke();
        }
        private void DoOnFadeOutEnded()
        {
            foreach (var g in target)
            {
                if (g)
                {
                    //Make sure the fade was completed (algorithm not necessarily completes the curve)
                    g.alpha = 0.0f;

                    if(disableWhenHidden)
                    {
                        g.gameObject.SetActive(false);
                    }
                }
            }
                    
        }


        private void _Show()
        {
            foreach (var g in target)
            {
                if (g)
                {
                    this._Fade(g, duration: fadeInTime, reverse: false);
                    if (disableWhenHidden)
                        g.gameObject.SetActive(true);
                }
            }
            Invoke("DoOnFadeInEnded", fadeInTime);
        }
        private void _Hide()
        {
            foreach (var g in target)
                if (g)
                {
                    this._Fade(g, duration: fadeOutTime, reverse: true);
                }
            Invoke("DoOnFadeOutEnded", fadeOutTime);
        }
        private void _ShowInstantly()
        {
            foreach (var g in target)
                if (g)
                {
                    g.alpha = 1f;
                    if (disableWhenHidden)
                        g.gameObject.SetActive(true);
                }
            Invoke("DoOnFadeInEnded", 0f);
        }
        private void _HideInstantly()
        {
            foreach (var g in target)
                if (g)
                {
                    g.alpha = 0f;
                }
            Invoke("DoOnFadeOutEnded", 0f);
        }

        private void _Fade(CanvasGroup g, float duration, bool reverse)
        {
            J.Instance.Lerp(x => g.alpha = x, duration: duration, repeat: 1, type: CurveType.Linear, reverse: reverse, callingScript: this);
        }
    }

}