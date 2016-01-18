using UnityEngine;
using System.Collections;

public class VirtualJoystickHandler : MonoBehaviour {

	public GameObject controllerPositionTextObject;
	public GameObject controllerRotationTextObject;
	public GameObject character;
	public GameObject characterPositionTextObject;
	public GameObject characterRotationTextObject;
	public GameObject directionPointer;
	public GameObject pointerPositionTextObject;
	public float TurnAngleThreshold = 10.0f;
	public float TurnRateOffset = 0.0f;
	public float MaxMovementSpeed = 2.0f;  // in meters/sec
	public float MaxTurnRate = 50f; // in degrees/sec
	public float TurnAngleToRateScale = 6.0f;	

	private TextMesh controllerPositionTextMesh;
	private TextMesh controllerRotationTextMesh;
	private TextMesh characterPositionTextMesh;
	private TextMesh characterRotationTextMesh;
	private TextMesh pointerPositionTextMesh;

	private static Vector3 yAxis = new Vector3 (0f, 1f, 0f);

	// Use this for initialization
	void Start () {
		controllerPositionTextMesh = controllerPositionTextObject.GetComponent<TextMesh> ();
		controllerRotationTextMesh = controllerRotationTextObject.GetComponent<TextMesh> ();
		characterPositionTextMesh = characterPositionTextObject.GetComponent<TextMesh> ();
		characterRotationTextMesh = characterRotationTextObject.GetComponent<TextMesh> ();
		pointerPositionTextMesh = pointerPositionTextObject.GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void HandleJoystickMovement() {
		//pointerDirection.Normalize ();
		controllerPositionTextMesh.text = "Controller Position: " + transform.position; 
		controllerRotationTextMesh.text = "Controller Rotation: " + transform.eulerAngles;
		characterPositionTextMesh.text = "Pointer direction: " + transform.up; 
		characterRotationTextMesh.text = "Character Rotation: " + character.transform.eulerAngles;

		//Vector3 moveDirection = new Vector3 (0f, 0f, 0.1f);
		Vector3 moveDirection = new Vector3 (Mathf.Pow(transform.up.x, 2)*Mathf.Sign(transform.up.x), 0, Mathf.Pow(transform.up.z, 2)*Mathf.Sign(transform.up.z)); // only take x and z components of direction

		if (moveDirection.magnitude > 0.1f) {			
			character.transform.Translate (moveDirection*MaxMovementSpeed*Time.deltaTime, Space.World);
		}
		Vector3 controllerXZ = Vector3.ProjectOnPlane(transform.right, yAxis);
		Vector3 characterXZ = Vector3.ProjectOnPlane (character.transform.right, yAxis);
		Vector3 controllerForwardDirectionInCharacterSpace = 
			character.transform.InverseTransformDirection (transform.forward);
		float controllerRelativeYRotation = Vector3.Angle (controllerXZ, characterXZ);
		if (Mathf.Abs(controllerForwardDirectionInCharacterSpace.x) > 0.1f) {
			float amountToRotate = (controllerForwardDirectionInCharacterSpace.x - 0.1f) * MaxTurnRate * Time.deltaTime;
			pointerPositionTextMesh.text = "amountToRotate: " + MaxTurnRate + ", " + Time.deltaTime + ", " + (MaxTurnRate * Time.deltaTime) + ", " +amountToRotate;
			Vector3 rotateDirection = new Vector3 (0f, amountToRotate, 0f);
			character.transform.Rotate (rotateDirection);
		}
	}
}
