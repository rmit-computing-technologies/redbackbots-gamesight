using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RotateTextToCamera : MonoBehaviour
{
    public Transform targetCamera; // Reference to the OVRCameraRig or any other camera
    private void Start(){
        try {
            targetCamera = GameObject.Find("CenterEyeAnchor").transform;
        } catch {
            Debug.LogError("Can't find CenterEyeAnchor");
        }
    }

    private void Update()
    {
        if (targetCamera != null){
            try {
                // Calculate the direction from the Text to the camera
                Vector3 directionToCamera = targetCamera.position - transform.position;

                // Calculate the rotation needed to align the Text with the camera's position
                Quaternion rotationToCamera = Quaternion.LookRotation(directionToCamera, Vector3.up);

                // Apply the rotation only along the Y-axis (flipped for readability)
                transform.rotation = Quaternion.Euler(0f, rotationToCamera.eulerAngles.y + 180f, 0f);
            } catch (Exception err){
                Debug.LogError("Failed to update text roate: " + err.ToString());
            }
        }

    }
}

