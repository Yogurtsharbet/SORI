using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;


public class CameraControl : MonoBehaviour {
    public static CameraControl Instance;
    public bool isDebugging;

    private List<CinemachineVirtualCameraBase> allCamera = new List<CinemachineVirtualCameraBase>();
    public enum CameraStatus {
        TopView, CombineView, SelectView, SelectTopView
    };
    public CameraStatus cameraStatus;

    private CinemachineVirtualCamera cameraTopView;
    private CinemachineVirtualCamera cameraCombineView;
    private CinemachineVirtualCamera cameraSelectView;
    private CinemachineVirtualCamera cameraSelectTopView;

    private CinemachineBlendListCamera cinematicIntro;
    private CinemachineBlendListCamera cinematicForest;

    public CinemachineVirtualCameraBase currentCamera { get; private set; }

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
        cameraSelectTopView = GetComponentsInChildren<CinemachineVirtualCamera>()[3];

        var cinematics = GetComponentsInChildren<CinemachineBlendListCamera>();
        cinematicIntro = cinematics[0];
        cinematicForest = cinematics[1];

        allCamera.Add(cameraTopView);
        allCamera.Add(cameraCombineView);
        allCamera.Add(cameraSelectView);
        allCamera.Add(cameraSelectTopView);
        allCamera.Add(cinematicIntro);
        allCamera.Add(cinematicForest);
    }

    private void Start() {
        SetCamera(cameraTopView);
        if (isDebugging) return;

        FadeControl.Instance.FadeIn();
        SetCamera(cinematicIntro);
    }

    private void OnValidate() {
        if ((int)cameraStatus >= 0 && (int)cameraStatus < allCamera.Count)
            SetCamera(cameraStatus);
    }

    public void SetCamera(CameraStatus camera) {
        SetCamera(allCamera[(int)camera]);
    }

    public void SetCamera(string camera) {
        if (camera == "Forest") SetCamera(cinematicForest);
    }

    public void SetCamera(CinemachineVirtualCameraBase camera) {
        Debug.Log($"Set to {camera.name}");
        cameraStatus = (CameraStatus)allCamera.IndexOf(camera);
        playerMove.enabled = camera == cameraTopView;
        playerMove.ClearCurretSpeed();

        foreach (var eachCamera in allCamera) {
            if (eachCamera == camera) {
                eachCamera.Priority = 1;
                currentCamera = camera;

                if (camera is CinemachineBlendListCamera) {
                    StartCoroutine(WaitForCinematicEnd(camera as CinemachineBlendListCamera));
                    CursorControl.SetCursor(CursorType.Loading);

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
        SetCamera(currentCamera != cameraTopView ? cameraTopView : cameraCombineView);
    }

    private IEnumerator WaitForCinematicEnd(CinemachineBlendListCamera camera) {
        Debug.Log($"Waiting Cinmatic : {camera.name}");
        playerMove.ClearCurretSpeed();
        int checkCount = 0;
        var lastChildCamera = camera.ChildCameras[camera.ChildCameras.Length - 1];

        while (checkCount < 30) {
            if (!camera.IsBlending && camera.IsLiveChild(lastChildCamera)) {
                checkCount++;
            }
            yield return null;
        }
        Debug.Log($"Complete Cinmatic : {camera.name}");

        CursorControl.SetCursor(CursorType.Default);
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

//TODO: SELECT VIEW 좌우 움직임 방지를 recentering 시간을 짧게하는 방식 말고, 직접 각도 지정으로 변경할 것. (어지러움)