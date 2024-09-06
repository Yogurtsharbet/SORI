using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraZooming : MonoBehaviour {
    private DefaultInputActions inputAction;
    private CinemachineVirtualCamera Camera;
    private CinemachineTransposer transposer;

    private const float defaultZoom = 250;
    private const float MaxZoom = 280;
    private const float MinZoom = 120;
    private float currentZoom;
    
    private float lastScrolledTime = 0;
    private float RecenteringTime = 2f;
    public float RecenteringSpeed = 2f;

    private void Awake() {
        inputAction = new DefaultInputActions();
        Camera = GetComponent<CinemachineVirtualCamera>();
        transposer = Camera.GetCinemachineComponent<CinemachineTransposer>();

        inputAction.UI.ScrollWheel.performed += value => OnScroll(value.ReadValue<Vector2>());
        currentZoom = defaultZoom;
    }

    private void OnEnable() {
        inputAction.Enable();
    }

    private void OnDisable() {
        inputAction.Disable();
    }

    private void OnScroll(Vector2 value) {
        currentZoom -= value.y * 0.05f;
        currentZoom = Mathf.Clamp(currentZoom, MinZoom, MaxZoom);
        lastScrolledTime = Time.time;
    }

    private void LateUpdate() {
        if(Time.time > lastScrolledTime + RecenteringTime) {
            currentZoom = Mathf.Lerp(currentZoom, defaultZoom, Time.deltaTime * 0.2f * RecenteringSpeed);
        }
        transposer.m_FollowOffset =
            Vector3.Slerp(transposer.m_FollowOffset, new Vector3(0, currentZoom, -currentZoom), Time.deltaTime);
    }
}