/*============================================================================== 
 * Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartManager : MonoBehaviour
{
	public Toggle cardboardToggle;

	#region PUBLIC_METHODS
	public void StartWaypoints()
	{
		TransitionManager.enableVRMode = cardboardToggle.isOn;
		LoadingManager.scene = "Vuforia-3-AR-VR-Waypoints";
		Application.LoadLevel("Vuforia-2-Loading");
	}

	public void StartController()
	{
		TransitionManager.enableVRMode = cardboardToggle.isOn;
		LoadingManager.scene = "Vuforia-3-AR-VR-Controller";
		Application.LoadLevel("Vuforia-2-Loading");
	}

	public void StartPositional()
	{
		TransitionManager.enableVRMode = cardboardToggle.isOn;
		LoadingManager.scene = "Vuforia-3-AR-VR-PositionalTracking";
		Application.LoadLevel("Vuforia-2-Loading");
	}
	#endregion // PUBLIC_METHODS


	#region MONOBEHAVIOUR_METHODS
	void Update()
	{
		#if UNITY_ANDROID
		// On Android, the Back button is mapped to the Esc key
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			// Exit app
			Application.Quit();
		}
		#endif
	}
	#endregion // MONOBEHAVIOUR_METHODS
}