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
		#region PRIVATE_MEMBER_VARIABLES
		
		public GameObject controllerObject;
		public GameObject controllerPositionTextObject;
		public GameObject controllerRotationTextObject;
		public GameObject character;
		public GameObject characterPositionTextObject;
		public GameObject characterRotationTextObject;

		private TextMesh controllerPositionTextMesh;
		private TextMesh controllerRotationTextMesh;
		private TextMesh characterPositionTextMesh;
		private TextMesh characterRotationTextMesh;

		private TrackableBehaviour mTrackableBehaviour;
		private bool isTracked = false;

		
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
		}
		
		#endregion // UNTIY_MONOBEHAVIOUR_METHODS
		
		void Update()
		{
			Vector3 controllerObjectPosition = controllerObject.transform.position;
			Vector3 controllerObjectRotation = controllerObject.transform.eulerAngles;
			controllerPositionTextMesh.text = "Controller Position: " + controllerObjectPosition; 
			controllerRotationTextMesh.text = "Controller Rotation: " + controllerObjectRotation;
			Vector3 characterPosition = character.transform.position;
			Vector3 characterRotation = character.transform.eulerAngles;
			characterPositionTextMesh.text = "Character Position: " + characterPosition; 
			characterRotationTextMesh.text = "Character Rotation: " + characterRotation;

			Vector3 moveDirection = new Vector3 (0f, 0f, 0.1f);
			if (isTracked && controllerObjectRotation.x > 30f && controllerObjectRotation.x < 330f) {
				if (controllerObjectRotation.x > 180f) {
					moveDirection *= -1f;
				}
				character.transform.Translate (moveDirection);
			}
			Vector3 rotateDirection = new Vector3 (0f, 1f, 0f);
			if (isTracked && controllerObjectRotation.z > 30f && controllerObjectRotation.z < 330f) {
				if (controllerObjectRotation.z < 180f) {
					rotateDirection *= -1f;
				}
				character.transform.Rotate (rotateDirection);
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
