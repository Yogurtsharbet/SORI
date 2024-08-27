using UnityEngine;

public class PlayerBehavior : MonoBehaviour {
    private PlayerInputActions playerInputAction;
    private CameraControl cameraControl;
    private Animator playerAnimator;

    private bool isCombineMode;
    public bool IsCombineMode { get { return isCombineMode; } }

    private void Awake() {
        playerInputAction = new PlayerInputActions();
        playerAnimator = GetComponent<Animator>();

        playerInputAction.PlayerActions.Combine.performed += value => OnCombine();
    }

    private void OnEnable() {
        cameraControl = FindObjectOfType<CameraControl>();
        playerInputAction.Enable();
    }

    private void OnDisable() {
        playerInputAction.Disable();
    }

    private void OnCombine() {
        CombineMode();
    }

    private void CombineMode() {
        isCombineMode = !isCombineMode;
        playerAnimator.SetBool("isCombineMode", isCombineMode);
        cameraControl.ChangeCamera();
    }
}
