﻿/*============================================================================== 
 * Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 
 * ==============================================================================*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GazeTrigger : MonoBehaviour
{

    #region PUBLIC_MEMBER_VARIABLES
    public float activationTime = 1.5f;
	public float deactivationTime = 1.5f;
    public Material focusedMaterial;
    public Material nonFocusedMaterial;
    public bool Focused { get; set; }
	public GameObject objectToActivate;
	public string functionToCall;
	public bool followCamera;
	public GameObject cameraToFollow;
	public Vector3 followCameraOffset;
 	public bool faceCamera;
    #endregion // PUBLIC_MEMBER_VARIABLES


    #region PRIVATE_MEMBER_VARIABLES
    private float mFocusedTime = 0;
    private bool mTriggered = false;
    #endregion // PRIVATE_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
        mTriggered = false;
        mFocusedTime = 0;
        Focused = false;
        GetComponent<Renderer>().material = nonFocusedMaterial;
    }

    void Update()
    {
		if (followCamera) {
			Vector3 newPosition = cameraToFollow.transform.position; 
			newPosition += -cameraToFollow.transform.right * followCameraOffset.x;
			newPosition += cameraToFollow.transform.forward * followCameraOffset.z;
			newPosition.y = followCameraOffset.y; 
			transform.position = newPosition;
			transform.rotation = cameraToFollow.transform.rotation;
		}

        if (mTriggered) 
            return;

        UpdateMaterials(Focused);

		if (Focused && !mTriggered) // for now can only trigger once. Need to implement deactivation time.
        {
            // Update the "focused state" time
            mFocusedTime += Time.deltaTime;
            if (mFocusedTime > activationTime)
            {
                mTriggered = true;
                mFocusedTime = 0;
				objectToActivate.SendMessage (functionToCall);
				StartCoroutine(ResetAfter(0.4f * TransitionManager.transitionDuration));
             }
        }
        else
        {
            // Reset the "focused state" time
            mFocusedTime = 0;
        }
    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region PRIVATE_METHODS
    private void UpdateMaterials(bool focused)
    {
        Renderer meshRenderer = GetComponent<Renderer>();
        if (focused)
        {
            if (meshRenderer.material != focusedMaterial)
                meshRenderer.material = focusedMaterial;
        }
        else
        {
            if (meshRenderer.material != nonFocusedMaterial)
                meshRenderer.material = nonFocusedMaterial;
        }
        
        float t = focused ? Mathf.Clamp01(mFocusedTime / activationTime) : 0;
        foreach (var rnd in GetComponentsInChildren<Renderer>())
        {
            if (rnd.material.shader.name.Equals("Custom/SurfaceScan"))
            {
                rnd.material.SetFloat("_ScanRatio", t);
            }
        }
    }

    private IEnumerator ResetAfter(float seconds)
    {
        Debug.Log("Resetting View trigger after: " + seconds);

        yield return new WaitForSeconds(seconds);

        Debug.Log("Resetting View trigger: " + this.gameObject.name);

        // Reset variables
        mTriggered = false;
        mFocusedTime = 0;
        Focused = false;
        UpdateMaterials(false);
    }
    #endregion // PRIVATE_METHODS
}

