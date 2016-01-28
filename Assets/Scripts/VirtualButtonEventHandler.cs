/*============================================================================== 
 * Copyright (c) 2012-2014 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/

using UnityEngine;
using System.Collections.Generic;
using Vuforia;

/// <summary>
/// This class implements the IVirtualButtonEventHandler interface and
/// contains the logic to swap materials for the teapot model depending on what 
/// virtual button has been pressed.
/// </summary>
public class VirtualButtonEventHandler : MonoBehaviour,
IVirtualButtonEventHandler
{
	#region PUBLIC_MEMBER_VARIABLES

	public GameObject controllerObject;
	public GameObject buttonTextObject;
	public GameObject flameObject;
	public GameObject beamObject;
	public float fireDuration;
	public GameObject arTransitionManager;

	#endregion // PUBLIC_MEMBER_VARIABLES



	#region PRIVATE_MEMBER_VARIABLES

	private TextMesh buttonTextMesh;

	#endregion // PRIVATE_MEMBER_VARIABLES



	#region UNITY_MONOBEHAVIOUR_METHODS

	void Start()
	{
		// Register with the virtual buttons TrackableBehaviour
		VirtualButtonBehaviour[] vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
		for (int i = 0; i < vbs.Length; ++i)
		{
			vbs[i].RegisterEventHandler(this);
		}
		buttonTextMesh = buttonTextObject.GetComponent<TextMesh> ();

	}

	#endregion // UNITY_MONOBEHAVIOUR_METHODS



	#region PUBLIC_METHODS

	/// <summary>
	/// Called when the virtual button has just been pressed:
	/// </summary>
	public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
	{
		buttonTextMesh.text = "OnButtonPressed:" + vb.VirtualButtonName;
		Debug.Log("OnButtonPressed:" + vb.VirtualButtonName);
		if (vb.VirtualButtonName == "ButtonA") {
			arTransitionManager.SendMessage("GoToVR");
		} else if (vb.VirtualButtonName == "ButtonB") {
			arTransitionManager.SendMessage("GoToAR");
		} else if (vb.VirtualButtonName == "ButtonX") {
			flame ();
		} else if (vb.VirtualButtonName == "ButtonY") {
			//arTransitionManager.SendMessage("GoToVR");
			beam ();
		}
	}


	/// <summary>
	/// Called when the virtual button has just been released:
	/// </summary>
	public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
	{
		buttonTextMesh.text = "OnButtonReleased:" + vb.VirtualButtonName;
		Debug.Log("OnButtonReleased:" + vb.VirtualButtonName);
	}

	public void flame() {
		GameObject newFlame = Instantiate (flameObject, controllerObject.transform.position + controllerObject.transform.forward*3f, controllerObject.transform.rotation) as GameObject;
		Destroy (newFlame, fireDuration);
	}

	public void beam() {
		GameObject newBeam = Instantiate (beamObject, controllerObject.transform.position + controllerObject.transform.forward*3f, controllerObject.transform.rotation) as GameObject;
		Destroy (newBeam, fireDuration);
	}


	#endregion // PUBLIC_METHODS
}
