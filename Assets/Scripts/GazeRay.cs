/*============================================================================== 
 * Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GazeRay : MonoBehaviour
{
    #region PUBLIC_MEMBER_VARIABLES
    #endregion // PUBLIC_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS
    void Update()
    {
        // Check if the Head gaze direction is intersecting any of the ViewTriggers
        RaycastHit hit;
        Ray cameraGaze = new Ray(this.transform.position, this.transform.forward);
        Physics.Raycast(cameraGaze, out hit, Mathf.Infinity);
		if (hit.collider) {
			if (hit.collider.gameObject.GetComponent<ViewTrigger> ()) {
				ViewTrigger trigger = hit.collider.gameObject.GetComponent<ViewTrigger> ();
				trigger.Focused = true;
			}
			if (hit.collider.gameObject.GetComponent<GazeTrigger> ()) {
				GazeTrigger trigger = hit.collider.gameObject.GetComponent<GazeTrigger> ();
				trigger.Focused = true;
			}
		}
        /*foreach (var trigger in viewTriggers)
        {
            trigger.Focused = hit.collider && (hit.collider.gameObject == trigger.gameObject);
        }*/
    }
    #endregion // MONOBEHAVIOUR_METHODS
}

