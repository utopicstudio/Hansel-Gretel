using UnityEngine;
using System.Collections;

public class MySimplePlayer : MonoBehaviour {

	enum Type {FirstPerson, Look}
	[SerializeField] Type type;
	public Transform player;
	public Camera mainCamera;
	public float speed = 1f;
	public float jumpForce = 1f;

	private Vector3 movementVector;
	private Vector3 lookVector;
	private Rigidbody rb;
	private Collider collider;
	private RaycastHit raycastHit;
	private bool isGrounded;

	// Use this for initialization
	void Start () {
		if (mainCamera == null)
			mainCamera = Camera.main;
		rb = GetComponent<Rigidbody> ();
		collider = GetComponent<Collider> ();

		checkGrounded ();
	}

	bool checkGrounded() {
		collider.Raycast (new Ray (transform.position, Vector3.down), out raycastHit, .1f);
		isGrounded = raycastHit.point == null ? false : true;
		return isGrounded;
	}

	// Update is called once per frame
	void Update () {
		switch (type) {
		case Type.FirstPerson:
			movementVector = new Vector3 (Input.GetAxis ("Horizontal"), 0f, Input.GetAxis ("Vertical"));
			player.localPosition += movementVector * speed;
			break;
		case Type.Look:
			//lookVector = new Vector3 (Input.GetAxis ("Mouse X"), Input.GetAxis ("Mouse Y"), 0f);
			//mainCamera.transform.LookAt (mainCamera.transform.position + mainCamera.transform.forward + lookVector * speed);
			movementVector = new Vector3 (Input.GetAxis ("Mouse X"), 0f, 0f);
			player.localPosition += movementVector * speed;
			break;
		default:
			break;
		}
		if (Input.GetButtonDown ("Jump"))// && checkGrounded())
			rb.AddForce (Vector3.up * jumpForce, ForceMode.Impulse);


	}
}
