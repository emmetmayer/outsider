using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager cameraManager;
    public ICinemachineCamera currentCam;

    // Start is called before the first frame update
    void Start()
    {
        cameraManager = this;
        currentCam = PlayerControl.player.mainCamera;
    }

    // Update is called once per frame
    public void SwitchCamera(ICinemachineCamera newCam)
    {
        currentCam.Priority = 10;
        newCam.Priority = 11;
        currentCam = newCam;
    }
}
