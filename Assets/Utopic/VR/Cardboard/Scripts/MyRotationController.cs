using UnityEngine;

namespace Utopic.VR.UtopicCardboard {


[DisallowMultipleComponent]
public class MyRotationController : MonoBehaviour {

	public float rotationSpeed;
	float rot;
	Quaternion quat;

	Transform cameraChild;

	void Start () {
		quat = Quaternion.identity;

		cameraChild = transform.Find ("Camera");
		if (cameraChild == null) {
			Debug.LogWarning ("Para rotacion manual, se necesita 'Camera' como hijo del Player");
		}
	}

	// Update is called once per frame
	void Update () {
		if (quat == Quaternion.identity) {
			if (Mathf.Abs (rot) > 0.1f) {
				transform.rotation = Quaternion.Euler (0f, transform.rotation.eulerAngles.y + rot * rotationSpeed, 0f);
			}
		} else {
			transform.localEulerAngles = new Vector3(0f, quat.eulerAngles.y, 0f);
		}
	}

	void setYRotationAmmount (float val) {
		rot = val;
		quat = Quaternion.identity;
	}
	void setYRotationAmmountQuaternion (Quaternion val) {
		quat = val;
	}
}


}//END_NAMESPACE