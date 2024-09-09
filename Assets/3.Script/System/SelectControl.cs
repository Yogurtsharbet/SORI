using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections.Generic;

public class SelectControl : MonoBehaviour {
    private CinemachineBrain cameraBrain;
    private DefaultInputActions inputAction;

    private Renderer clickedObject;
    private Renderer prevObject;
    private Renderer nowObject;

    private Vector3 mousePosition;
    private RaycastHit rayHit;
    private LayerMask layerMask;

    private Camera currentCamera { get { return cameraBrain.OutputCamera; } }
    private CameraControl.CameraStatus cameraStatus;

    private List<Material> selectedMaterials = new List<Material>();
    [SerializeField] private Material outlineShader;
    [SerializeField] private Material clickedShader;

    private Transform Indicator;

    public void SetTargetTag(string tag) { targetTag = tag; }
    private string targetTag;

    private void Awake() {
        cameraBrain = FindObjectOfType<CinemachineBrain>();
        Indicator = transform.GetChild(0);

        inputAction = new DefaultInputActions();
        inputAction.UI.Point.performed += value => OnPoint(value.ReadValue<Vector2>());
        inputAction.UI.Click.performed += value => OnClickLeft();
        inputAction.UI.Submit.performed += value => OnEnter();
        inputAction.UI.Cancel.performed += value => OnCancel();
        inputAction.UI.RightClick.performed += value => OnClickRight();

        layerMask = 1 << LayerMask.NameToLayer("Ground");
        layerMask = ~layerMask;
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
            // Clear VFX when camera is not int SelectMode
            if (prevObject != null) {
                RemoveMaterial(prevObject, outlineShader);
                prevObject = null;
            }
            if (nowObject != null) {
                RemoveMaterial(nowObject, outlineShader);
                nowObject = null;
            }
            if (clickedObject != null) {
                RemoveMaterial(clickedObject, clickedShader);
                clickedObject = null;
            }
        }
    }

    private void FindObject() {
        if (clickedObject != null) return;

        if (Physics.Raycast(currentCamera.ScreenPointToRay(mousePosition),
            out rayHit, maxDistance: float.MaxValue, layerMask)) {

            if (rayHit.collider.CompareTag(targetTag)) {
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
        //TODO: nowSelected 가 Word의 속성과 일치하는지
        if (nowObject == null || clickedObject != null) return;

        ApplyMaterial(nowObject, clickedShader);
        IndicatorOn(nowObject);

        if (clickedObject != null)
            RemoveMaterial(clickedObject, clickedShader);
        clickedObject = nowObject;
    }

    private void CancelSelected() {
        if (clickedObject == null) return;

        RemoveMaterial(clickedObject, clickedShader);
        IndicatorOff();

        clickedObject = null;
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

        //TODO: 위치값 조정. rotation 화면 바라보도록
    }

    private void IndicatorOff() {
        Indicator.gameObject.SetActive(false);
    }

    private void OnPoint(Vector2 value) {
        mousePosition = value;
    }

    private void OnClickLeft() {
        Select();
    }

    private void OnClickRight() {
        CancelSelected();
    }

    private void OnEnter() {
        if (clickedObject == null) return;
        FindObjectOfType<PlayerBehavior>().ToggleCombineMode();
        //TODO: Frame에서 메서드를 만들어서, 여기서 clickedSelected를 보내기
    }

    private void OnCancel() {
        switch (CameraControl.Instance.cameraStatus) {
            case CameraControl.CameraStatus.TopView: return;

            case CameraControl.CameraStatus.CombineView:
                FindObjectOfType<PlayerBehavior>().ToggleCombineMode();
                break;

            case CameraControl.CameraStatus.SelectView:
            case CameraControl.CameraStatus.SelectTopView:
                //TODO: 단어조합창 UI 다시 띄우기
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
//TODO: Outline woodbine 이나 bigflower에 안보이는거

//TODO: clicked 상태에서 방향 등을 설정할 수 있게 하고, 우클릭을 하면 clicked가 해제되도록