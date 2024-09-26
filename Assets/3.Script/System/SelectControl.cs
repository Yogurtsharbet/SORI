using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SelectData {
    public List<GameObject> clickedObject;
    public List<GameObject> Indicator;

    public SelectData(List<Renderer> clickedObject, List<GameObject> Indicator) {
        this.Indicator = Indicator;
        this.clickedObject = new List<GameObject>();
        foreach(var each in clickedObject) {
            this.clickedObject.Add(each.gameObject);
        }
    }
}

public class SelectControl : MonoBehaviour {
    private List<Material> selectedMaterials = new List<Material>();
    [SerializeField] private Material outlineShader;
    [SerializeField] private Material clickedShader;

    private CinemachineBrain cameraBrain;
    private DefaultInputActions inputAction;

    private PlayerBehavior playerBehavior;
    private CombineManager combineManager;
    private CombineContainer combineContainer;

    private List<Renderer> clickedObject;
    private Renderer prevObject;
    private Renderer nowObject;

    private Transform Indicator;

    private Vector3 mousePosition;
    private RaycastHit rayHit;
    private LayerMask layerMask;

    private Camera currentCamera { get { return cameraBrain.OutputCamera; } }
    private CameraControl.CameraStatus cameraStatus;

    private List<GameObject> SpawnedIndicator;
    private (List<Renderer>, List<GameObject>) SelectData;

    private void Awake() {
        playerBehavior = FindObjectOfType<PlayerBehavior>();
        combineManager = FindObjectOfType<CombineManager>();
        combineContainer = FindObjectOfType<CombineContainer>();

        cameraBrain = FindObjectOfType<CinemachineBrain>();
        Indicator = GetComponentInChildren<IndicatorControl>().transform;

        inputAction = new DefaultInputActions();
        inputAction.UI.Point.performed += value => OnPoint(value.ReadValue<Vector2>());
        inputAction.UI.Click.performed += value => OnClickLeft();
        inputAction.UI.Submit.performed += value => OnEnter();
        inputAction.UI.Cancel.performed += value => OnCancel();
        inputAction.UI.RightClick.performed += value => OnClickRight();

        layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Water"));
        layerMask = ~layerMask;

        clickedObject = new List<Renderer>();
        SpawnedIndicator = new List<GameObject>();
    }

    private void OnEnable() {
        inputAction.Enable();
    }

    private void OnDisable() {
        inputAction.Disable();
    }

    private void Update() {
        cameraStatus = CameraControl.Instance.cameraStatus;
        if (cameraStatus == CameraControl.CameraStatus.SelectView ||
            cameraStatus == CameraControl.CameraStatus.SelectTopView) {
            FindObject();
        }
        else {
            Unselect();
            ClearIndicator();
            foreach (var each in clickedObject) RemoveMaterial(each, clickedShader);
        }
    }

    private void FindObject() {
        if (Indicator.gameObject.activeSelf) return;
        if (Physics.Raycast(currentCamera.ScreenPointToRay(mousePosition),
            out rayHit, maxDistance: float.MaxValue, layerMask)) {

            if (FrameActivate.CompareTag(rayHit.collider.tag)) {
                nowObject = rayHit.collider.GetComponent<Renderer>();

                if (prevObject != nowObject) {
                    ApplyMaterial(nowObject, outlineShader);
                    RemoveMaterial(prevObject, outlineShader);
                    prevObject = nowObject;
                }
            }
        }
    }

    private void Select() {
        if (nowObject == null) return;

        ApplyMaterial(nowObject, clickedShader);
        if (FrameActivate.CheckMovable(nowObject.tag))
            IndicatorOn(nowObject);

        if (!clickedObject.Contains(nowObject))
            clickedObject.Add(nowObject);
    }

    private void Unselect() {
        if (nowObject != null) {
            RemoveMaterial(nowObject, outlineShader);
            RemoveMaterial(nowObject, clickedShader);
            clickedObject.Remove(nowObject);
            IndicatorOff();
        }
        if (prevObject != null) RemoveMaterial(prevObject, outlineShader);

        nowObject = null;
        prevObject = null;
    }

    private void RepositionAtScreenOut() {
        Vector3 screenPos = currentCamera.WorldToScreenPoint(Indicator.position);

        // 화면 경계 체크 (X: 0 ~ Screen.width, Y: 0 ~ Screen.height)
        screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
        screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);

        // 다시 화면 좌표를 월드 좌표로 변환
        Vector3 worldPos = currentCamera.ScreenToWorldPoint(screenPos);

        // Indicator 위치 설정
        Indicator.position = worldPos;
    }

    private void IndicatorOn(Renderer target) {
        Indicator.gameObject.SetActive(true);
        Indicator.position = target.GetComponent<Collider>().bounds.center;
        // RepositionAtScreenOut();

        foreach (var each in SpawnedIndicator) {
            if (Vector3.Distance(each.transform.position, Indicator.position) <= 0.01f) {
                each.SetActive(false);
                break;
            }
        }
    }

    private void IndicatorOff() {
        Indicator.gameObject.SetActive(false);
        foreach (var each in SpawnedIndicator) {
            if (Vector3.Distance(nowObject.GetComponent<Collider>().bounds.center, each.transform.position) <= 0.01f) {
                each.SetActive(false);
                break;
            }
        }
    }

    private void SetIndicator() {
        foreach (var each in SpawnedIndicator) {
            if (!each.activeSelf) {
                each.GetComponent<IndicatorControl>().SetInstantitate(Indicator);
                Indicator.gameObject.SetActive(false);
                RemoveMaterial(nowObject, clickedShader);
                nowObject = null;
                return;
            }
        }

        GameObject spawn = Instantiate(Indicator, Indicator.position, Indicator.rotation, Indicator.parent).gameObject;
        spawn.GetComponent<IndicatorControl>().isInstantiated = true;

        SpawnedIndicator.Add(spawn);
        Indicator.gameObject.SetActive(false);
    }

    private void ClearIndicator() {
        foreach (var each in SpawnedIndicator) {
            each.SetActive(false);
        }
    }

    private void OnPoint(Vector2 value) {
        mousePosition = value;
    }

    private bool checkClick;
    private void OnClickLeft() {
        if (!checkClick) checkClick = true;
        else {
            checkClick = false;
            if (Indicator.gameObject.activeSelf)
                SetIndicator();
            else Select();
        }
    }

    private void OnClickRight() {
        Unselect();
    }

    private void OnEnter() {
        if (clickedObject.Count == 0) return;
        if (Indicator.gameObject.activeSelf) return;
        playerBehavior.ToggleCombineMode();

        FrameActivate.Activate(new SelectData(clickedObject, SpawnedIndicator));
        foreach (var each in clickedObject)
            RemoveMaterial(each, clickedShader);
        clickedObject.Clear(); SpawnedIndicator.Clear();
    }

    private void OnCancel() {
        switch (CameraControl.Instance.cameraStatus) {
            case CameraControl.CameraStatus.TopView: return;

            case CameraControl.CameraStatus.CombineView:
                playerBehavior.ToggleCombineMode();
                break;

            case CameraControl.CameraStatus.SelectView:
            case CameraControl.CameraStatus.SelectTopView:
                combineContainer.OpenCombineField();
                CameraControl.Instance.SetCamera(CameraControl.CameraStatus.CombineView);
                break;
        }
    }

    private void ApplyMaterial(Renderer renderer, Material material) {
        if (renderer == null) return;

        selectedMaterials.Clear();
        selectedMaterials.AddRange(renderer.sharedMaterials);
        selectedMaterials.Add(material);

        renderer.materials = selectedMaterials.ToArray();
    }

    private void RemoveMaterial(Renderer renderer, Material material) {
        if (renderer == null) return;

        selectedMaterials.Clear();
        selectedMaterials.AddRange(renderer.sharedMaterials);
        selectedMaterials.Remove(material);

        renderer.materials = selectedMaterials.ToArray();
    }
}

//TODO: clicked Shader dot 조금씩 위로 올라가는 연출
//TODO: 여러개 선택할 수 있고, 선택 범위 (distance 제한) 만들고, 선택한 오브젝트를 우클릭하면 해제되고.
//TODO: 선택한 오브젝트가 movable 이면 indicator 표시하고, 재차 클릭하면 indicator 고정

//TODO: R키 누르면 전부 Unselect