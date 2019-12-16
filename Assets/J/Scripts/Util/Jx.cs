using UnityEngine;

namespace J
{

	[AddComponentMenu("J/Util/Jx")]
	public class Jx : MonoBehaviour {

		public void JxToggleSetActive() {
			this.gameObject.SetActive( !this.gameObject.activeSelf );
		}
		public void JxToggleCollider() {
			Collider col = GetComponent<Collider> ();
			if (col != null)
				col.enabled = !col.enabled;
		}
		public void JxToggleIsTrigger() {
			Collider col = GetComponent<Collider> ();
			if (col != null)
				col.isTrigger = !col.isTrigger;
		}
		public void JxToggleRenderer() {
			Renderer rend = GetComponent<Renderer> ();
			if (rend != null)
				rend.enabled = !rend.enabled;
		}
		public void JxToggleUseGravity() {
			Rigidbody rb = GetComponent<Rigidbody> ();
			if (rb != null)
				rb.useGravity = !rb.useGravity;
		}
		public void JxToggleIsKinematic() {
			Rigidbody rb = GetComponent<Rigidbody> ();
			if (rb != null)
				rb.isKinematic = !rb.isKinematic;
		}
	}

}