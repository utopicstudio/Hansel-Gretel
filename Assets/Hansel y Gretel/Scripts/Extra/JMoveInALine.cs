using UnityEngine;

namespace J
{

	[AddComponentMenu("J/3D/JMoveInALine")]
	public class JMoveInALine : MonoBehaviour {


		enum ModificationType
		{
			Local, Global
		}

		[SerializeField]	ModificationType type;
		[SerializeField]	Vector3 direction = Vector3.forward;
		[SerializeField]	float moveFactor = 1f;
		[SerializeField]	float acceleration = 0f;

		protected float moveFactorPrivate;
		protected float accelerationPrivate;

		void Update () {
			accelerationPrivate = acceleration * Time.deltaTime;
			moveFactor += accelerationPrivate;
			moveFactorPrivate = moveFactor * Time.deltaTime;
			switch (type) {
			case ModificationType.Local:
				transform.position += transform.TransformVector (Vector3.Normalize (direction)) * moveFactorPrivate;
				break;
			case ModificationType.Global:
				transform.position += Vector3.Normalize(direction) * moveFactorPrivate;
				break;
			}
		}
	}

}