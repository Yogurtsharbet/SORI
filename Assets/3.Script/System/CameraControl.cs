using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;


public class CameraControl : MonoBehaviour {
    public static CameraControl Instance;
    public Transform CameraBorder;
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
    private CinemachineBlendListCamera cinematicRuins;
    private CinemachineBlendListCamera cinematicHome;

    public CinemachineVirtualCameraBase currentCamera { get; private set; }

    private PlayerMove playerMove;
    private Animator playerAnimator;
    private Animator butterflyAnimator;

    [SerializeField] private Transform cinematicForestPosition;
    [SerializeField] private Transform cinematicRuinsPosition;
    [SerializeField] private GameObject cinematicRuinsRocks;
    [SerializeField] private GameObject cinematicRuinsBarrier;

    [SerializeField] private Material daySkybox;
    [SerializeField] private Canvas gameCanvas;
    private QuestController questController;
    private SFXManager sfxManager;

    public Transform RuinsPosition => cinematicRuinsPosition;

    private void Awake() {
        Instance = this;
        CameraBorder = transform.GetChild(0);

        playerMove = FindObjectOfType<PlayerMove>();
        playerAnimator = playerMove.GetComponent<Animator>();

        cameraTopView = GetComponentsInChildren<CinemachineVirtualCamera>()[0];
        cameraCombineView = GetComponentsInChildren<CinemachineVirtualCamera>()[1];
        cameraSelectView = GetComponentsInChildren<CinemachineVirtualCamera>()[2];
        cameraSelectTopView = GetComponentsInChildren<CinemachineVirtualCamera>()[3];

        var cinematics = GetComponentsInChildren<CinemachineBlendListCamera>();
        cinematicIntro = cinematics[0];
        cinematicForest = cinematics[1];
        cinematicRuins = cinematics[2];
        cinematicHome = cinematics[3];

        allCamera.Add(cameraTopView);
        allCamera.Add(cameraCombineView);
        allCamera.Add(cameraSelectView);
        allCamera.Add(cameraSelectTopView);
        allCamera.Add(cinematicIntro);
        allCamera.Add(cinematicForest);
        allCamera.Add(cinematicRuins);
        allCamera.Add(cinematicHome);

        butterflyAnimator = GameObject.FindGameObjectWithTag("Butterfly")?.GetComponent<Animator>();
        questController = gameCanvas.GetComponentInChildren<QuestController>();
        sfxManager = FindObjectOfType<SFXManager>();
    }

    private void Start() {
        SetCamera(cameraTopView);
        if (GameManager.Instance.currentScene == "Map") {
            if (GameManager.Instance.isCompleteTutorial) {
                return;
            }
        }

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
        else if (camera == "Ruins") SetCamera(cinematicRuins);
        else if (camera == "Home") SetCamera(cinematicHome);
    }

    public void SetCamera(CinemachineVirtualCameraBase camera) {
        Debug.Log($"Set to {camera.name}");
        cameraStatus = (CameraStatus)allCamera.IndexOf(camera);
        if(playerMove==null) playerMove = FindObjectOfType<PlayerMove>();
        playerMove.enabled = camera == cameraTopView;
        playerMove.ClearCurretSpeed();
        if (camera == cameraTopView)
            gameCanvas.gameObject.SetActive(true);

        foreach (var eachCamera in allCamera) {
            if (eachCamera == camera) {
                eachCamera.Priority = 1;
                currentCamera = camera;



                if (camera is CinemachineBlendListCamera) {
                    StartCoroutine(WaitForCinematicEnd(camera as CinemachineBlendListCamera));
                    CursorControl.SetCursor(CursorType.Loading);

                    if (camera == cinematicIntro) {
                        //TODO: QUEST TEXT 바꾸기
                        questController.SetQuestText("길을 따라 가세요");
                        gameCanvas.gameObject.SetActive(false);
                        playerAnimator.SetTrigger("cinematicIntro");
                    }
                    else if (camera == cinematicForest) {
                        questController.SetQuestText("빛을 따라 가세요");
                        gameCanvas.gameObject.SetActive(false);
                        CinematicForestProcess();
                    }
                    else if (camera == cinematicRuins) {
                        gameCanvas.gameObject.SetActive(false);
                        CinematicRuinsProcess();
                    }
                    else if (camera == cinematicHome) {
                        Debug.Log("[DEBUG] :: Camera is CinematicHome");
                        gameCanvas.gameObject.SetActive(false);
                        CinematicHomeProcess();
                    }
                }
                else if (camera == cameraSelectView) {
                    if (GameManager.Instance.currentScene == "Map" &&
                        !RenderSettings.skybox.name.Contains("Night"))
                        questController.SetQuestText("돌을 선택하고 [Enter] 를 눌러\n돌을 치우세요");
                }
            }
            else
                eachCamera.Priority = 0;
        }

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

    private IEnumerator RotateDolly() {
        while (true) {
            yield return null;
            if (cinematicForest.IsLiveChild(cinematicForest.ChildCameras[3])) {
                yield return new WaitForSeconds(0.5f);

                var camera = cinematicForest.ChildCameras[3] as CinemachineVirtualCamera;
                var dolly = camera.GetCinemachineComponent<CinemachineTrackedDolly>();

                while (dolly.m_PathPosition < 8) {
                    yield return new WaitForSeconds(0.7f);
                    dolly.m_PathPosition++;
                    camera.m_Lens.FieldOfView -= Time.deltaTime;
                }

                camera.m_Lens.FieldOfView -= Time.deltaTime * 3f;
                if (camera.m_Lens.FieldOfView <= 50) break;
            }
        }
    }

    private void CinematicForestProcess() {
        StartCoroutine(RotateDolly());
        playerAnimator.SetFloat("MoveSpeed", 7f);
        playerAnimator.SetBool("isJump", false);

        butterflyAnimator.SetTrigger("cinematicForest");

        Sequence sequence = DOTween.Sequence();
        sequence
            .AppendCallback(() => playerMove.transform.LookAt(cinematicForestPosition.position))
            .Append(playerMove.transform.DOMove(cinematicForestPosition.position, 5f))
            .OnComplete(() => playerAnimator.SetTrigger("cinematicForest"))
            .OnKill(() => playerAnimator.SetFloat("MoveSpeed", 0f))
            .Play();
    }

    private void CinematicRuinsProcess() {
        playerAnimator.SetFloat("MoveSpeed", 7f);
        playerAnimator.SetBool("isJump", false);

        Sequence sequence = DOTween.Sequence();
        sequence
            .AppendCallback(() => playerMove.transform.LookAt(cinematicRuinsPosition.position))
            .Append(playerMove.transform.DOMove(cinematicRuinsPosition.position, 5f))
            .OnComplete(() => playerAnimator.SetFloat("MoveSpeed", 2f))
            .AppendCallback(() => StartCoroutine(ActiveRuinsRock()))
            .Append(playerMove.transform.DOMove(cinematicRuinsPosition.position + transform.forward, 1.5f))
            .OnComplete(() => playerAnimator.SetFloat("MoveSpeed", 0f))
            .OnKill(() => StartCoroutine(PlayerRuinsAnimation()))
            .Play();
    }

    private IEnumerator ActiveRuinsRock() {
        cinematicRuinsRocks.SetActive(true);
        sfxManager.PlayBrokenRockSfx();
        foreach (Transform eachRock in cinematicRuinsRocks.transform)
            eachRock.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(3000, 15000), Random.Range(3000, 20000), Random.Range(3300, 10000)), ForceMode.Impulse);
        yield return new WaitForSeconds(5f);
        foreach (Transform eachRock in cinematicRuinsRocks.transform)
            eachRock.GetComponent<Rigidbody>().isKinematic = true;
        cinematicRuinsBarrier.SetActive(false);
    }

    private IEnumerator PlayerRuinsAnimation() {
        while (true) {
            yield return null;
            if (cinematicRuins.IsLiveChild(cinematicRuins.ChildCameras[2])) {
                yield return new WaitForSeconds(2f);
                playerAnimator.Play("Impact");
                butterflyAnimator.Play("ButterFlyZone04");
                while (true) {
                    yield return null;
                    AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
                    if (!stateInfo.IsTag("DisableMovement")) {
                        playerAnimator.Play("Looking Around");
                        break;
                    }
                }
                break;
            }
        }
        GameManager.Instance.CompleteTutorial();
    }

    private void CinematicHomeProcess() {
        Debug.Log("[DEBUG] :: CinematicHomeProcess() is Ongoing");
        if (playerAnimator == null) playerAnimator = FindObjectOfType<PlayerBehavior>().GetComponent<Animator>();
        playerAnimator.SetFloat("MoveSpeed", 0f);
        playerAnimator.SetBool("isJump", false);
        StartCoroutine(GameEnd());
    }

    private IEnumerator GameEnd() {
        yield return new WaitForSeconds(3f);
        FindObjectOfType<MainLoading>().StartLoading(0);
        Debug.Log("[DEBUG] :: GameEnd() Coroutine proceed StartLoading()");
        yield return new WaitForSeconds(1.5f);
        Debug.Log("[DEBUG] :: GameEnd() Coroutine proceed WaitForSecs");


        var directionalLight = FindObjectOfType<CommonManager>().GetComponentInChildren<Light>();
        directionalLight.color = new Color(1f, 0.956f, 0.839f);
        RenderSettings.ambientLight = new Color(0.90f, 0.76f, 0.63f);
        RenderSettings.skybox = daySkybox;
        Debug.Log("[DEBUG] :: GameEnd() Coroutine proceed RenderSettings");

        Destroy(GameManager.Instance.gameObject);
        Destroy(gameObject);
    }
}

//TODO: SELECT VIEW 좌우 움직임 방지를 recentering 시간을 짧게하는 방식 말고, 직접 각도 지정으로 변경할 것. (어지러움)