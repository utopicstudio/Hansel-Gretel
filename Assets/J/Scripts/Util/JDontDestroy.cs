using UnityEngine;

namespace J
{
	[AddComponentMenu("J/Util/JDontDestroy")]
	public class JDontDestroy : MonoBehaviour {
		void Awake () {
			DontDestroyOnLoad (this.gameObject);
		}
	}
}