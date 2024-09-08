using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System.Collections.Generic;

public class SelectObject : MonoBehaviour {
    private CameraControl cameraControl;
    private CinemachineBrain cameraBrain;
    private DefaultInputActions inputAction;

    private Renderer prevSelected;
    private Renderer nowSelected;

    private Vector3 mousePosition;
    private RaycastHit rayHit;
    private LayerMask layerMask;

    private Camera currentCamera { get { return cameraBrain.OutputCamera; } }

    [SerializeField] private Material outlineShader;
    private List<Material> selectedMaterials = new List<Material>();

    private void Awake() {
        cameraControl = FindObjectOfType<CameraControl>();
        cameraBrain = FindObjectOfType<CinemachineBrain>();
        inputAction = new DefaultInputActions();
        inputAction.UI.Point.performed += value => OnPoint(value.ReadValue<Vector2>());

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
        FindObject();
    }

    private void FindObject() {
        var camera = CameraControl.Instance.cameraStatus;
        if (camera == CameraControl.CameraStatus.SelectView ||
            camera == CameraControl.CameraStatus.SelectTopView) {
            if (Physics.Raycast(currentCamera.ScreenPointToRay(mousePosition),
                out rayHit, maxDistance: float.MaxValue, layerMask)) {
                nowSelected = rayHit.collider.GetComponent<Renderer>();
                if (prevSelected == null) prevSelected = nowSelected;

                if (prevSelected != nowSelected) {
                    ApplyOutline(nowSelected);
                    RemoveOutline(prevSelected);
                    prevSelected = nowSelected;
                }
            }
        }
        else {
            if (prevSelected != null) {
                RemoveOutline(prevSelected);
                prevSelected = null;
            }
            if (nowSelected != null) {
                RemoveOutline(nowSelected);
                nowSelected = null;
            }
        }
    }

    private void OnPoint(Vector2 value) {
        mousePosition = value;
    }

    private void ApplyOutline(Renderer renderer) {
        selectedMaterials.Clear();
        selectedMaterials.AddRange(renderer.sharedMaterials);
        selectedMaterials.Add(outlineShader);

        renderer.materials = selectedMaterials.ToArray();
    }

    private void RemoveOutline(Renderer renderer) {
        selectedMaterials.Clear();
        selectedMaterials.AddRange(renderer.sharedMaterials);
        selectedMaterials.Remove(outlineShader);

        renderer.materials = selectedMaterials.ToArray();
    }
}