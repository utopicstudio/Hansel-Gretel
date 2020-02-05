using UnityEngine;
using System.Collections;

namespace J
{

    [AddComponentMenu("J/Util/JActionRedirect")]
    public class JActionRedirect : MonoBehaviour
    {
        [SerializeField] UnityEngine.Events.UnityEvent actionRedirect;
        
        public void CallAction()
        {
            actionRedirect.Invoke();
        }
    }

}