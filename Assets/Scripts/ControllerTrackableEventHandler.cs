using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.IO;

namespace Vuforia
{
	/// <summary>
	/// A custom handler that implements the ITrackableEventHandler interface.
	/// </summary>
	public class ControllerTrackableEventHandler : MonoBehaviour,
	ITrackableEventHandler
	{
		#region PUBLIC_MEMBER_VARIABLES
		public GameObject controllerObject;
		public GameObject controllerPositionTextObject;
		public GameObject controllerRotationTextObject;
		public GameObject character;
		public GameObject characterPositionTextObject;
		public GameObject characterRotationTextObject;
		public GameObject directionPointer;
		public GameObject pointerPositionTextObject;
		public float TurnAngleThreshold = 10.0f;
		public float TurnRateOffset = 0.0f;
		public float MaxTurnRate = 1.5f;
		public float TurnAngleToRateScale = 6.0f;
		#endregion // PUBLIC_MEMBER_VARIABLES

		#region PRIVATE_MEMBER_VARIABLES
		private TextMesh controllerPositionTextMesh;
		private TextMesh controllerRotationTextMesh;
		private TextMesh characterPositionTextMesh;
		private TextMesh characterRotationTextMesh;
		private TextMesh pointerPositionTextMesh;
		private TrackableBehaviour mTrackableBehaviour;
		private bool isTracked = false;
		private static Vector3 yAxis = new Vector3 (0f, 1f, 0f);
		
		#endregion // PRIVATE_MEMBER_VARIABLES
		
		
		
		#region UNTIY_MONOBEHAVIOUR_METHODS
		
		void Start()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour>();
			if (mTrackableBehaviour)
			{
				mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}

			controllerPositionTextMesh = controllerPositionTextObject.GetComponent<TextMesh> ();
			controllerRotationTextMesh = controllerRotationTextObject.GetComponent<TextMesh> ();
			characterPositionTextMesh = characterPositionTextObject.GetComponent<TextMesh> ();
			characterRotationTextMesh = characterRotationTextObject.GetComponent<TextMesh> ();
			pointerPositionTextMesh = pointerPositionTextObject.GetComponent<TextMesh> ();
		}
		
		#endregion // UNTIY_MONOBEHAVIOUR_METHODS
		
		void Update()
		{
			// TODO: move movement logic to a different script
			// TODO: should add time sampling so it's not more sensitive for higher framerates
			Vector3 controllerObjectPosition = controllerObject.transform.position;
			Vector3 controllerObjectRotation = controllerObject.transform.eulerAngles;
			Vector3 pointerDirection = directionPointer.transform.position - controllerObjectPosition;
			pointerDirection.Normalize ();
			controllerPositionTextMesh.text = "Controller Position: " + controllerObjectPosition; 
			controllerRotationTextMesh.text = "Controller Rotation: " + controllerObjectRotation;
			Vector3 characterPosition = character.transform.position;
			Vector3 characterRotation = character.transform.eulerAngles;
			characterPositionTextMesh.text = "Pointer direction: " + pointerDirection; 
			characterRotationTextMesh.text = "Character Rotation: " + characterRotation;

			//Vector3 moveDirection = new Vector3 (0f, 0f, 0.1f);
			Vector3 moveDirection = new Vector3 (Mathf.Pow(pointerDirection.x, 2)*Mathf.Sign(pointerDirection.x), 0, Mathf.Pow(pointerDirection.z, 2)*Mathf.Sign(pointerDirection.z)); // only take x and z components of direction
			if (isTracked) {
				if (moveDirection.magnitude > 0.1f) {
					//character.transform.Translate (moveDirection*0.1f, Space.World);
				}
				Vector3 controllerXZ = Vector3.ProjectOnPlane(controllerObject.transform.right, yAxis);
				Vector3 characterXZ = Vector3.ProjectOnPlane (character.transform.right, yAxis);
				Vector3 controllerForwardDirectionInCharacterSpace = 
					character.transform.InverseTransformDirection (controllerObject.transform.forward);
				float controllerRelativeYRotation = Vector3.Angle (controllerXZ, characterXZ);
				pointerPositionTextMesh.text = "controllerForwardDirectionInCharacterSpace: " + controllerForwardDirectionInCharacterSpace;
				if (Mathf.Abs(controllerForwardDirectionInCharacterSpace.x) > 0.1f) {
					Vector3 rotateDirection = new Vector3 (0f, controllerForwardDirectionInCharacterSpace.x - 0.1f, 0f);
					character.transform.Rotate (rotateDirection);
				}
			}


			#if UNITY_ANDROID
			// On Android, the Back button is mapped to the Esc key
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				// Exit app
				Application.LoadLevel("Start");
			}
			#endif
		}
		
		
		
		#region PUBLIC_METHODS
		
		/// <summary>
		/// Implementation of the ITrackableEventHandler function called when the
		/// tracking state changes.
		/// </summary>
		public void OnTrackableStateChanged(
			TrackableBehaviour.Status previousStatus,
			TrackableBehaviour.Status newStatus)
		{
			if (newStatus == TrackableBehaviour.Status.DETECTED ||
			    newStatus == TrackableBehaviour.Status.TRACKED ||
			    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
			{
				OnTrackingFound();
			}
			else
			{
				OnTrackingLost();
			}
		}

		public bool GetIsTracked()
		{
			return isTracked;
		}


		#endregion // PUBLIC_METHODS
		
		
		
		#region PRIVATE_METHODS
		
		
		private void OnTrackingFound()
		{
			isTracked = true;

			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

			// Enable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = true;
			}

			// Enable colliders:
			foreach (Collider component in colliderComponents)
			{
				component.enabled = true;
			}

			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
		}
		
		
		private void OnTrackingLost()
		{
			isTracked = false;
			
			Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
			Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

			// Disable rendering:
			foreach (Renderer component in rendererComponents)
			{
				component.enabled = false;
			}

			// Disable colliders:
			foreach (Collider component in colliderComponents)
			{
				component.enabled = false;
			}

			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
		}

		#endregion // PRIVATE_METHODS
	}
}
