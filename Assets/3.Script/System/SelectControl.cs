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


    private List<Renderer> clickedObject;
    private Renderer prevObject;
    private Renderer nowObject;

    private Transform Indicator;
    public IndicatorControl IndicatorControl { get; private set; }

    private Vector3 mousePosition;
    private RaycastHit rayHit;
    private LayerMask layerMask;

    private Camera currentCamera { get { return cameraBrain.OutputCamera; } }
    private CameraControl.CameraStatus cameraStatus;

    private List<GameObject> SpawnedIndicator;
    public bool IsSelectComplete { get { return clickedObject.Count != 0 && !Indicator.gameObject.activeSelf; } }

    private void Awake() {
        cameraBrain = FindObjectOfType<CinemachineBrain>();
        IndicatorControl = GetComponentInChildren<IndicatorControl>();
        Indicator = IndicatorControl.transform;

        inputAction = new DefaultInputActions();
        inputAction.UI.Point.performed += value => OnPoint(value.ReadValue<Vector2>());
        inputAction.UI.Click.performed += value => OnClickLeft();
        inputAction.UI.RightClick.performed += value => OnClickRight();
        //inputAction.UI.Cancel.performed += value => OnCancel();

        layerMask = 
            (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Water")) |
            (1 << LayerMask.NameToLayer("Ignore Raycast"));
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
        //TODO: cameraStatus 를 GameState 로 바꾸기 
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
                nowObject = rayHit.collider.GetComponentInChildren<Renderer>();

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
        Collider collider = nowObject.GetComponent<Collider>();
        if (collider == null) collider = nowObject.GetComponentInParent<Collider>();

        if (FrameActivate.CheckMovable(collider.tag))
            IndicatorOn(collider);

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

    public void UnselectAll() {
        Unselect();
        foreach(var each in clickedObject) 
            RemoveMaterial(each, clickedShader);
        clickedObject.Clear();   
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
        var collider = target.GetComponent<Collider>();
        if (collider == null)
            collider = target.transform.parent.GetComponent<Collider>();

        Indicator.gameObject.SetActive(true);
        var position = collider.bounds.center;
        if (collider.CompareTag("RUSTKEY"))
            position.y = collider.GetComponent<RustKeyMovement>().defaultY;
        Indicator.position = position;
        // RepositionAtScreenOut();

        foreach (var each in SpawnedIndicator) {
            if (Vector3.Distance(each.transform.position, Indicator.position) <= 0.01f) {
                each.SetActive(false);
                break;
            }
        }
    }

    private void IndicatorOn(Collider target) {
        Indicator.gameObject.SetActive(true);
        var position = target.bounds.center;
        if (target.CompareTag("RUSTKEY"))
            position.y = target.GetComponent<RustKeyMovement>().defaultY;
        Indicator.position = position;
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
            var collider = nowObject.GetComponent<Collider>();
            if(collider == null) 
                collider = nowObject.transform.parent.GetComponent<Collider>();

            if (Vector3.Distance(collider.bounds.center, each.transform.position) <= 0.01f) {
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

    private bool checkClick;    // 이중클릭 막기 위한 임시방편
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

    public void ActivateSelected() {
        FrameActivate.Activate(new SelectData(clickedObject, SpawnedIndicator));
        foreach (var each in clickedObject)
            RemoveMaterial(each, clickedShader);
        clickedObject.Clear(); SpawnedIndicator.Clear();
    }

    // GameState 구현에 따른 삭제. 240927.
    //private void OnCancel() {
    //    switch (CameraControl.Instance.cameraStatus) {
    //        case CameraControl.CameraStatus.TopView: return;

    //        case CameraControl.CameraStatus.CombineView:
    //            playerBehavior.ToggleCombineMode();
    //            break;

    //        case CameraControl.CameraStatus.SelectView:
    //        case CameraControl.CameraStatus.SelectTopView:
    //            combineContainer.OpenCombineField();
    //            CameraControl.Instance.SetCamera(CameraControl.CameraStatus.CombineView);
    //            break;
    //    }
    //}

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