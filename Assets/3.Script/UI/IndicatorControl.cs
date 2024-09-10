using UnityEngine;
using UnityEngine.InputSystem;
using DTT.AreaOfEffectRegions;

public class IndicatorControl : MonoBehaviour {
    private DefaultInputActions inputAction;
    private ArcRegion arcArrow;

    private Quaternion targetRotation;
    private Vector3 mousePosition;

    private float currentY = 0f;
    private float rotateSpeed = 10f;

    public Vector3 indicatePosition;

    private void Awake() {
        arcArrow = GetComponent<ArcRegion>();

        inputAction = new DefaultInputActions();
        inputAction.UI.Point.performed += value => OnPoint(value.ReadValue<Vector2>());

    }

    private void OnEnable() {
        inputAction.Enable();
    }

    private void OnDisable() {
        inputAction.Disable();
    }

    private Vector3 directionToMouse;

    private void Update() {
        currentY += Time.deltaTime * rotateSpeed;
        if (currentY > 360f) currentY -= 360f;

        targetRotation = Quaternion.LookRotation(Camera.main.transform.forward) * Quaternion.AngleAxis(330f, Vector3.right)
        * Quaternion.AngleAxis(currentY, Vector3.up);

        transform.rotation = targetRotation;

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane plane = new Plane(transform.up, transform.position);
        if (plane.Raycast(ray, out float distance)) {
            Vector3 mouseWorldPos = ray.GetPoint(distance);
            directionToMouse = (mouseWorldPos - transform.position).normalized;

            float angleToMouse = Mathf.Atan2(directionToMouse.x, directionToMouse.z) * Mathf.Rad2Deg;
            arcArrow.Angle = angleToMouse - currentY + 150;
        }
    }

    private void OnPoint(Vector2 value) {
        mousePosition = value;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        indicatePosition = Quaternion.LookRotation(directionToMouse) * Quaternion.AngleAxis(10f, Vector3.up) * Vector3.forward;
        if (Physics.Raycast(indicatePosition, Vector3.down, out RaycastHit rayHit, Mathf.Infinity, LayerMask.NameToLayer("Ground"))) {
            indicatePosition = rayHit.point;
        }

        // 해당 방향으로 Gizmo 그리기
        Vector3 gizmoPosition = transform.position + indicatePosition * 20f;
        Gizmos.DrawWireSphere(gizmoPosition, 3f);


    }

}
