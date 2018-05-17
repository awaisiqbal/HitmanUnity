using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class EndScreen : MonoBehaviour
{
	// postprocessing profile with depth of field blur effect
    public PostProcessingProfile blurProfile;

    // postprocessing profile without blur
    public PostProcessingProfile normalProfile;

    // postprocessing component from camera
    public PostProcessingBehaviour cameraPostProcess;

    // switch between the blurProfile and normalProfile
    public void EnableCameraBlur(bool state)
    {
        if (cameraPostProcess != null && blurProfile != null && normalProfile != null)
        {
            cameraPostProcess.profile = (state) ? blurProfile : normalProfile;
        }
    }
}
