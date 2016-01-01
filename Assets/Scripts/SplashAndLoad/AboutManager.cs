/*============================================================================== 
 * Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/
using UnityEngine;
using System.Collections;

public class AboutManager : MonoBehaviour
{
    #region PUBLIC_METHODS
    public void OnStartAR_Carboard()
    {
		TransitionManager.enableVRMode = true;
        Application.LoadLevel("Vuforia-2-Loading");
    }
	public void OnStartAR_NoCardboard()
	{
		TransitionManager.enableVRMode = false;
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