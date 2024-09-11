using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SelectControl : MonoBehaviour {
    private List<Material> selectedMaterials = new List<Material>();
    [SerializeField] private Material outlineShader;
    [SerializeField] private Material clickedShader;

    private CinemachineBrain cameraBrain;
    private DefaultInputActions inputAction;

    private PlayerBehavior playerBehavior;
    private CombineManager combineManager;

    private Renderer clickedObject;
    private Renderer prevObject;
    private Renderer nowObject;

    private Transform Indicator;

    private Vector3 mousePosition;
    private RaycastHit rayHit;
    private LayerMask layerMask;

    private Camera currentCamera { get { return cameraBrain.OutputCamera; } }
    private CameraControl.CameraStatus cameraStatus;

    public void SetTargetTag(string tag) { targetTag = tag; }
    private string targetTag;

    private void Awake() {
        playerBehavior = FindObjectOfType<PlayerBehavior>();
        combineManager = FindObjectOfType<CombineManager>();

        cameraBrain = FindObjectOfType<CinemachineBrain>();
        Indicator = transform.GetChild(0);

        inputAction = new DefaultInputActions();
        inputAction.UI.Point.performed += value => OnPoint(value.ReadValue<Vector2>());
        inputAction.UI.Click.performed += value => OnClickLeft();
        inputAction.UI.Submit.performed += value => OnEnter();
        inputAction.UI.Cancel.performed += value => OnCancel();
        inputAction.UI.RightClick.performed += value => OnClickRight();

        layerMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Water"));
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
            CancelSelected();
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
        //TODO: nowSelected �� Word�� �Ӽ��� ��ġ�ϴ���
        if (nowObject == null || clickedObject != null) return;

        ApplyMaterial(nowObject, clickedShader);
        IndicatorOn(nowObject);

        if (clickedObject != null)
            RemoveMaterial(clickedObject, clickedShader);
        clickedObject = nowObject;
    }

    private void CancelSelected() {
        IndicatorOff();

        if (clickedObject != null) RemoveMaterial(clickedObject, clickedShader);
        if (nowObject != null) RemoveMaterial(nowObject, outlineShader);
        if (prevObject != null) RemoveMaterial(prevObject, outlineShader);
        
        clickedObject = null;
        nowObject = null;
        prevObject = null;
    }

    private void RepositionAtScreenOut() {
        Vector3 screenPos = currentCamera.WorldToScreenPoint(Indicator.position);

        // ȭ�� ��� üũ (X: 0 ~ Screen.width, Y: 0 ~ Screen.height)
        screenPos.x = Mathf.Clamp(screenPos.x, 0, Screen.width);
        screenPos.y = Mathf.Clamp(screenPos.y, 0, Screen.height);

        // �ٽ� ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 worldPos = currentCamera.ScreenToWorldPoint(screenPos);

        // Indicator ��ġ ����
        Indicator.position = worldPos;
    }

    private void IndicatorOn(Renderer target) {
        Indicator.gameObject.SetActive(true);
        Indicator.position = target.GetComponent<Collider>().bounds.center;
        // RepositionAtScreenOut();

        //TODO: ��ġ�� ����. rotation ȭ�� �ٶ󺸵���
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
        playerBehavior.ToggleCombineMode();
        combineManager.Activate(clickedObject.gameObject, Indicator.gameObject);
    }

    private void OnCancel() {
        switch (CameraControl.Instance.cameraStatus) {
            case CameraControl.CameraStatus.TopView: return;

            case CameraControl.CameraStatus.CombineView:
                playerBehavior.ToggleCombineMode();
                break;

            case CameraControl.CameraStatus.SelectView:
            case CameraControl.CameraStatus.SelectTopView:
                //TODO: �ܾ�����â UI �ٽ� ����
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

//TODO: clicked Shader dot ���ݾ� ���� �ö󰡴� ����
//TODO: Outline woodbine �̳� bigflower�� �Ⱥ��̴°�

//TODO: clicked ���¿��� ���� ���� ������ �� �ְ� �ϰ�, ��Ŭ���� �ϸ� clicked�� �����ǵ���