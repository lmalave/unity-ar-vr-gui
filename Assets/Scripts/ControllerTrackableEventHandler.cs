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
		#endregion // PUBLIC_MEMBER_VARIABLES

		#region PRIVATE_MEMBER_VARIABLES
		private TextMesh controllerPositionTextMesh;
		private TextMesh controllerRotationTextMesh;
		private TextMesh characterPositionTextMesh;
		private TextMesh characterRotationTextMesh;
		private TextMesh pointerPositionTextMesh;
		private TrackableBehaviour mTrackableBehaviour;
		private bool isTracked = false;
		private Vector3 waypointDestination = Vector3.zero;

		
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
			if (waypointDestination != Vector3.zero) {
				if (Vector3.Distance (waypointDestination, character.transform.position) < 0.5) {
					waypointDestination = Vector2.zero;
				} else {
					Vector3 waypointDirection = waypointDestination - character.transform.position;
					Vector3 adjustedWaypointDirection = new Vector3 (waypointDirection.x, 0f, waypointDirection.z);
					adjustedWaypointDirection.Normalize ();
					character.transform.Translate (adjustedWaypointDirection * 0.01f, Space.World);
				}
			} else {
				Vector3 controllerObjectPosition = controllerObject.transform.position;
				Vector3 controllerObjectRotation = controllerObject.transform.eulerAngles;
				Vector3 pointerDirection = directionPointer.transform.position - controllerObjectPosition;
				controllerPositionTextMesh.text = "Controller Position: " + controllerObjectPosition; 
				controllerRotationTextMesh.text = "Pointer Positon: " + directionPointer.transform.position;
				Vector3 characterPosition = character.transform.position;
				Vector3 characterRotation = character.transform.eulerAngles;
				characterPositionTextMesh.text = "Pointer direction: " + pointerDirection; 
				characterRotationTextMesh.text = "Character Rotation: " + characterRotation;

				//Vector3 moveDirection = new Vector3 (0f, 0f, 0.1f);
				Vector3 moveDirection = new Vector3 (pointerDirection.x, 0, pointerDirection.z); // only take x and z components of direction
				pointerPositionTextMesh.text = "Move Direction z: " + moveDirection.z;
				if (isTracked && controllerObjectRotation.x > 30f && controllerObjectRotation.x < 330f) {
					character.transform.Translate (moveDirection, Space.World);
				}
				Vector3 rotateDirection = new Vector3 (0f, 0.3f, 0f);
				if (isTracked && controllerObjectRotation.z > 30f && controllerObjectRotation.z < 330f) {
					if (controllerObjectRotation.z < 180f) {
						rotateDirection *= -1f;
					}
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

		public void MoveToWaypoint (Vector3 waypointPosition) {
			waypointDestination = waypointPosition;
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
