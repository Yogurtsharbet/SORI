using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour {
    private CinemachineVirtualCamera cameraTopView;
    private CinemachineVirtualCamera cameraCombineView;

    private void Awake() {
        cameraTopView = GetComponentsInChildren<CinemachineVirtualCamera>()[0];
        cameraCombineView = GetComponentsInChildren<CinemachineVirtualCamera>()[1];
    }

    public void ChangeCamera() {
        cameraCombineView.Priority = cameraCombineView.Priority == 0 ? 2 : 0;
    }
}