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
        //TODO: nowSelected 가 Word의 속성과 일치하는지

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
        //TODO: Frame에서 메서드를 만들어서, 여기서 clickedSelected를 보내기
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

//TODO: clicked Shader dot 조금씩 위로 올라가는 연출
//TODO: Outline woodbine 이나 bigflower에 안보이는거

//TODO: clicked 상태에서 방향 등을 설정할 수 있게 하고, 우클릭을 하면 clicked가 해제되도록