using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraControl : MonoBehaviour {
    public static CameraControl Instance;
    public bool isDebugging;

    private CinemachineVirtualCamera cameraTopView;
    private CinemachineVirtualCamera cameraCombineView;
    private CinemachineVirtualCamera cameraSelectView;

    private CinemachineBlendListCamera cinematicIntro;
    private CinemachineBlendListCamera cinematicForest;

    private List<CinemachineVirtualCameraBase> allCamera = new List<CinemachineVirtualCameraBase>();
    private CinemachineVirtualCameraBase currentCamera;

    private PlayerMove playerMove;
    private Animator playerAnimator;
    private Animator butterflyAnimator;

    [SerializeField] private Transform cinematicForestPosition;

    private void Awake() {
        Instance = this;

        playerMove = FindObjectOfType<PlayerMove>();
        playerAnimator = playerMove.GetComponent<Animator>();
        butterflyAnimator = GameObject.FindGameObjectWithTag("Butterfly").GetComponent<Animator>();

        cameraTopView = GetComponentsInChildren<CinemachineVirtualCamera>()[0];
        cameraCombineView = GetComponentsInChildren<CinemachineVirtualCamera>()[1];
        cameraSelectView = GetComponentsInChildren<CinemachineVirtualCamera>()[2];

        var cinematics = GetComponentsInChildren<CinemachineBlendListCamera>();
        cinematicIntro = cinematics[0];
        cinematicForest = cinematics[1];

        allCamera.Add(cameraTopView);
        allCamera.Add(cameraCombineView);
        allCamera.Add(cameraSelectView);
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
        playerMove.ClearCurretSpeed();

        foreach (var eachCamera in allCamera) {
            if (eachCamera == camera) {
                eachCamera.Priority = 1;
                currentCamera = camera;

                if (camera is CinemachineBlendListCamera) {
                    StartCoroutine(WaitForCinematicEnd(camera as CinemachineBlendListCamera));

                    if (camera == cinematicIntro) 
                        playerAnimator.SetTrigger("cinematicIntro");

                    else if (camera == cinematicForest) {
                        StartCoroutine(RotateFreeLock());
                        playerAnimator.SetFloat("MoveSpeed", 7f);
                        butterflyAnimator.SetTrigger("cinematicForest");

                        Sequence sequence = DOTween.Sequence();
                        sequence
                            .AppendCallback(() => playerMove.transform.LookAt(cinematicForestPosition.position))
                            .Append(playerMove.transform.DOMove(cinematicForestPosition.position, 5f)
                            .OnComplete(() => playerAnimator.SetFloat("MoveSpeed", 3f)))
                            .Append(playerMove.transform.DOMove(cinematicForestPosition.position + transform.forward, 0.5f)
                            .OnComplete(() => playerAnimator.SetTrigger("cinematicForest")))
                            .OnKill(() => playerAnimator.SetFloat("MoveSpeed", 0f))
                            .Play();
                    }
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
        var lastChildCamera = camera.ChildCameras[camera.ChildCameras.Length - 1];

        while (checkCount < 30) {
            if (!camera.IsBlending && camera.IsLiveChild(lastChildCamera)) {
                checkCount++;
            }
            yield return null;
        }
        Debug.Log($"Complete Cinmatic : {camera.name}");

        playerMove.ClearCurretSpeed();
        SetCamera(cameraTopView);
    }

    private IEnumerator RotateFreeLock() {
        var camera = GetComponentInChildren<CinemachineFreeLook>();
        yield return new WaitForSeconds(2f);

        while(currentCamera != cameraTopView) {
            camera.m_XAxis.m_InputAxisValue = -0.5f;
            yield return null;
        }
    }
}
