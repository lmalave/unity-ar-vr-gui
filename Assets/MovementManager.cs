using UnityEngine;
using System.Collections;

public class MovementManager : MonoBehaviour {

	public GameObject controllerObject;
	public GameObject positionTextObject;
	public GameObject rotationTextObject;
	public GameObject objectToMove;

	private TextMesh positionTextMesh;
	private TextMesh rotationTextMesh;

	// Use this for initialization
	void Start () {
		positionTextMesh = positionTextObject.GetComponent<TextMesh> ();
		rotationTextMesh = rotationTextObject.GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 controllerObjectPosition = controllerObject.transform.position;
		Vector3 controllerObjectRotation = controllerObject.transform.eulerAngles;
		positionTextMesh.text = "Position: " + controllerObjectPosition; 
		rotationTextMesh.text = "Rotation: " + controllerObjectRotation;
		Vector3 moveDirection = new Vector3 (0f, 0f, 0.01f);
		if (controllerObjectRotation.x > 30f && controllerObjectRotation.x < 330f) {
			if (controllerObjectRotation.x > 180f) {
				moveDirection *= -1f;
			}
			objectToMove.transform.Translate (moveDirection);
		}
	}
}
