using UnityEngine;

public class MoveOscilate : MonoBehaviour {

	enum AnimationCurveType
	{
		Sine, Custom
	}
	[SerializeField]
	AnimationCurveType curveType;
	public AnimationCurve movementCurve;
	public Vector3 movementDirection = Vector3.up;
	public float distance = 1f;
	public float duration = 1f;

	private float timeVariable = 0f;
	private Vector3 initialPosition;

	void Start () {
		InitializeMovementCurve ();

		initialPosition = transform.position;
	}
	void OnValidate () {
		InitializeMovementCurve ();
	}

	void Update () {
		timeVariable = (timeVariable + Time.deltaTime) % duration;
		transform.position = initialPosition + Vector3.Normalize(movementDirection) * movementCurve.Evaluate (timeVariable);
	}

	private void InitializeMovementCurve() {
		switch (curveType) {
		case AnimationCurveType.Sine:
			float tangentAngle = distance / (duration * 0.1666f);
			movementCurve = new AnimationCurve ();
			movementCurve.AddKey (new Keyframe (0f, 0f, -tangentAngle, tangentAngle));
			movementCurve.AddKey (new Keyframe (duration * 1 / 4, distance));
			movementCurve.AddKey (new Keyframe (duration * 3 / 4, -distance));
			movementCurve.AddKey (new Keyframe (duration, 0f, tangentAngle, -tangentAngle));
			break;
		case AnimationCurveType.Custom:
			duration = movementCurve.keys [movementCurve.length - 1].time;
			break;
		default:
			break;
		}


	}
}
