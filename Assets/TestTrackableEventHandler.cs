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
	public class TestTrackableEventHandler : MonoBehaviour,
	ITrackableEventHandler
	{
		#region PRIVATE_MEMBER_VARIABLES
		
		private TrackableBehaviour mTrackableBehaviour;
		private bool isRendered = false;
		private bool isTracked = false;
		private bool codeRetrieved = false;
		private int mWeatherCode = 0;
		private string weatherCondition = "";
		private GameObject kittenObject;
		private GameObject positionTextObject;
		private GameObject rotationTextObject;
		TextMesh positionTextMesh;
		TextMesh rotationTextMesh;

		
		#endregion // PRIVATE_MEMBER_VARIABLES
		
		
		
		#region UNTIY_MONOBEHAVIOUR_METHODS
		
		void Start()
		{
			mTrackableBehaviour = GetComponent<TrackableBehaviour>();
			if (mTrackableBehaviour)
			{
				mTrackableBehaviour.RegisterTrackableEventHandler(this);
			}
			
			// Note - s
			kittenObject = GameObject.Find ("Kitten");
			positionTextObject = GameObject.Find ("PositionText");
			rotationTextObject = GameObject.Find ("RotationText");
			positionTextMesh = positionTextObject.GetComponent<TextMesh> ();
			rotationTextMesh = rotationTextObject.GetComponent<TextMesh> ();

			
		}
		

		#endregion // UNTIY_MONOBEHAVIOUR_METHODS

		void Update()
		{
			
			positionTextMesh.text = "Position: " + kittenObject.transform.position;
			rotationTextMesh.text = "Rotation: " + kittenObject.transform.eulerAngles;
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
		
		#endregion // PUBLIC_METHODS
		
		
		
		#region PRIVATE_METHODS
		
		
		private void OnTrackingFound()
		{
			isTracked = true;
			

			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
		}
		
		
		private void OnTrackingLost()
		{
			if(isRendered)
			{
				// just try to hide all weather objects
			}
			
			isRendered = false;
			isTracked = false;
			
			Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
		}
		


		#endregion // PRIVATE_METHODS
	}
}
