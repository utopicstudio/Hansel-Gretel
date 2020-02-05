using UnityEngine;
using System.Collections;

public class UtopicWaypoints : MonoBehaviour {

	//
	//public Transform 
	public Transform objectToMove;
	public float speed = 1f;
	public float startOffsetInSeconds = 0f;
	//public bool loop;
	public Transform[] points;

	private int i = 0;
	private bool isMoving = false;
	private float lerpParam = 0f;//dont change 0f

	// Use this for initialization
	void Start () {
		if (!objectToMove)
			objectToMove = this.transform;
		
		if (points.Length < 2) {
			Debug.LogWarning ("Waypoints in " + gameObject.name + " should have at least 2 points");
			gameObject.SetActive (false);
		} else {
			//Invoke ("startMoving", startOffsetInSeconds);
			StartCoroutine(startMoving(startOffsetInSeconds));
		}
	}
	IEnumerator startMoving(float t) {
		yield return new WaitForSeconds (startOffsetInSeconds);
		isMoving = true;
	}

	// Update is called once per frame
	void Update () {
		if (isMoving) {
			Vector3 pointA = points [i].position;
			Vector3 pointB = points [i + 1].position;
			transform.position = Vector3.Lerp (pointA, pointB, lerpParam);
			lerpParam += Time.deltaTime * speed * 0.01f;
			if ( lerpParam > 1f) {
				lerpParam = 0f;
				i += 1;
				pointA = points [i].position;
				pointB = points [i + 1].position;
				if (i + 1 >= points.Length)
					isMoving = false;
			}
		}
	}
}
