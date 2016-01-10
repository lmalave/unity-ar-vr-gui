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
	public class PositionMarkerTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
	{
		#region PUBLIC_MEMBER_VARIABLES
		public GameObject positionAnchor;
		public GameObject character;
		public GameObject vrWorld;
		#endregion // PUBLIC_MEMBER_VARIABLES

		#region PRIVATE_MEMBER_VARIABLES
		private TrackableBehaviour mTrackableBehaviour;
		private bool isTracked = false;
		private Vector3 initialAnchorPosition;
		private Quaternion initialAnchorRotation;


		#endregion // PRIVATE_MEMBER_VARIABLES
		
		
		
		#region UNTIY_MONOBEHAVIOUR_METHODS
		
		void Start()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour>();
			if (mTrackableBehaviour)
			{
				mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}
			initialAnchorPosition = positionAnchor.transform.position;  
			initialAnchorRotation = positionAnchor.transform.rotation;  
			Debug.Log ("initialAnchorPosition: " + initialAnchorPosition);
			Debug.Log ("initialAnchorRotation: " + initialAnchorRotation);
		}
		
		#endregion // UNTIY_MONOBEHAVIOUR_METHODS
		
		void Update()
		{
			Vector3 markerPosition = transform.position - character.transform.position;

			//Vector3 moveDirection = new Vector3 (0f, 0f, 0.1f);
			if (isTracked) {
				vrWorld.transform.position = transform.position;
				vrWorld.transform.rotation = transform.rotation;
				vrWorld.transform.position -= vrWorld.transform.right * initialAnchorPosition.x;
				vrWorld.transform.position -= vrWorld.transform.up * initialAnchorPosition.y;
				vrWorld.transform.position -= vrWorld.transform.forward * initialAnchorPosition.z;
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
