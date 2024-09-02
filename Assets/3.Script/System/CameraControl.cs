using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Runtime.CompilerServices;
using System.Linq;

public class CameraControl : MonoBehaviour {
    public static CameraControl Instance;
    public bool isDebugging;

    private CinemachineVirtualCamera cameraTopView;
    private CinemachineVirtualCamera cameraCombineView;

    private CinemachineBlendListCamera cinematicIntro;
    private CinemachineBlendListCamera cinematicForest;

    private List<CinemachineVirtualCameraBase> allCamera = new List<CinemachineVirtualCameraBase>();
    private CinemachineVirtualCameraBase currentCamera;

    private PlayerMove playerMove;
    private Animator playerAnimator;

    private void Awake() {
        Instance = this;

        playerMove = FindObjectOfType<PlayerMove>();
        playerAnimator = playerMove.GetComponent<Animator>();

        cameraTopView = GetComponentsInChildren<CinemachineVirtualCamera>()[0];
        cameraCombineView = GetComponentsInChildren<CinemachineVirtualCamera>()[1];

        var cinematics = GetComponentsInChildren<CinemachineBlendListCamera>();
        cinematicIntro = cinematics[0];
        cinematicForest = cinematics[1];

        allCamera.Add(cameraTopView);
        allCamera.Add(cameraCombineView);
        allCamera.Add(cinematicIntro);
        allCamera.Add(cinematicForest);
    }

    private void Start() {
        if (isDebugging) return;

        FadeControl.Instance.FadeIn();
        SetCamera(cinematicIntro);
    }

    public void SetCamera(string camera) {
        if (camera == "Forest") SetCamera(cinematicForest);
    }

    public void SetCamera(CinemachineVirtualCameraBase camera) {
        Debug.Log($"Set to {camera.name}");
        playerMove.enabled = camera == cameraTopView;
        foreach (var eachCamera in allCamera) {
            if (eachCamera == camera) {
                eachCamera.Priority = 1;
                currentCamera = camera;

                if (camera is CinemachineBlendListCamera) {
                    StartCoroutine(WaitForCinematicEnd(camera as CinemachineBlendListCamera));
                    if (camera == cinematicIntro) playerAnimator.SetTrigger("cinematicIntro");
                }
            }
            else
                eachCamera.Priority = 0;
        }
    }

    public void ChangePlayerCamera() {
        SetCamera(currentCamera != cameraCombineView ? cameraCombineView : cameraTopView);
    }

    private IEnumerator WaitForCinematicEnd(CinemachineBlendListCamera camera) {
        Debug.Log($"Waiting Cinmatic : {camera.name}");
        int checkCount = 0;

        while(checkCount < 50) {
            if (!camera.IsBlending) {
                checkCount++;
            }
            yield return null;
        }
        Debug.Log($"Complete Cinmatic : {camera.name}");

        SetCamera(cameraTopView);
    }
}
