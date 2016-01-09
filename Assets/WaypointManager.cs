using UnityEngine;
using System.Collections;
using Vuforia;

public class WaypointManager : MonoBehaviour {

	public GameObject objectToMove;

	private bool isActive = false;

	public void MoveToWaypoint() {
		isActive = true;
	}

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (isActive) {
			if (Vector3.Distance (transform.position, objectToMove.transform.position) < 2) {
				isActive = false;
			} else {
				Vector3 moveDirection = transform.position - objectToMove.transform.position;
				Vector3 adjustedMoveDirection = new Vector3 (moveDirection.x, 0f, moveDirection.z);
				adjustedMoveDirection.Normalize ();
				objectToMove.transform.Translate (adjustedMoveDirection * 0.05f, Space.World);
			}
		}	
	}
}
