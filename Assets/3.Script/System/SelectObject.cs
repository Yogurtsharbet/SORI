using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections.Generic;

public class SelectObject : MonoBehaviour {
    private CinemachineBrain cameraBrain;
    private DefaultInputActions inputAction;

    private GameObject clickedSelected;
    private Renderer prevSelected;
    private Renderer nowSelected;

    private Vector3 mousePosition;
    private RaycastHit rayHit;
    private LayerMask layerMask;

    private Camera currentCamera { get { return cameraBrain.OutputCamera; } }
    private CameraControl.CameraStatus cameraStatus;

    private List<Material> selectedMaterials = new List<Material>();
    [SerializeField] private Material outlineShader;
    [SerializeField] private Material clickedShader;

    private void Awake() {
        cameraBrain = FindObjectOfType<CinemachineBrain>();
        
        inputAction = new DefaultInputActions();
        inputAction.UI.Point.performed += value => OnPoint(value.ReadValue<Vector2>());
        inputAction.UI.Click.performed += value => OnClick();
        inputAction.UI.Submit.performed += value => OnEnter();

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
            if (prevSelected != null) {
                RemoveMaterial(prevSelected, outlineShader);
                prevSelected = null;
            }
            if (nowSelected != null) {
                RemoveMaterial(nowSelected, outlineShader);
                nowSelected = null;
            }
            if (clickedSelected != null) {
                RemoveMaterial(clickedSelected.GetComponent<Renderer>(), clickedShader);
                clickedSelected = null;
            }
        }
    }

    private void FindObject() {
        if (Physics.Raycast(currentCamera.ScreenPointToRay(mousePosition),
            out rayHit, maxDistance: float.MaxValue, layerMask)) {
            nowSelected = rayHit.collider.GetComponent<Renderer>();
            if (prevSelected == null) prevSelected = nowSelected;

            if (prevSelected != nowSelected) {
                ApplyMaterial(nowSelected, outlineShader);
                RemoveMaterial(prevSelected, outlineShader);
                prevSelected = nowSelected;
            }
        }
    }

    private void Select() {
        //TODO: nowSelected �� Word�� �Ӽ��� ��ġ�ϴ���

        if (nowSelected == null) return;
        ApplyMaterial(nowSelected, clickedShader);
        if (clickedSelected != null)
            RemoveMaterial(clickedSelected.GetComponent<Renderer>(), clickedShader);
        clickedSelected = nowSelected.gameObject;
    }

    private void OnPoint(Vector2 value) {
        mousePosition = value;
    }

    private void OnClick() {
        Select();
    }

    private void OnEnter() {
        if (clickedSelected == null) return;
        FindObjectOfType<PlayerBehavior>().ToggleCombineMode();
        //TODO: Frame���� �޼��带 ����, ���⼭ clickedSelected�� ������
    }

    private void ApplyMaterial(Renderer renderer, Material material) {
        selectedMaterials.Clear();
        selectedMaterials.AddRange(renderer.sharedMaterials);
        selectedMaterials.Add(material);

        renderer.materials = selectedMaterials.ToArray();
    }

    private void RemoveMaterial(Renderer renderer, Material material) {
        selectedMaterials.Clear();
        selectedMaterials.AddRange(renderer.sharedMaterials);
        selectedMaterials.Remove(material);

        renderer.materials = selectedMaterials.ToArray();
    }
}

//TODO: clicked Shader dot ���ݾ� ���� �ö󰡴� ����
//TODO: Outline woodbine �̳� bigflower�� �Ⱥ��̴°�

//TODO: clicked ���¿��� ���� ���� ������ �� �ְ� �ϰ�, ��Ŭ���� �ϸ� clicked�� �����ǵ���