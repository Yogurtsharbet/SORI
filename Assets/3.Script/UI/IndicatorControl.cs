using UnityEngine;
using UnityEngine.InputSystem;
using DTT.AreaOfEffectRegions;

public class IndicatorControl : MonoBehaviour {
    private Transform playerTransform;
    private DefaultInputActions inputAction;
    private ArcRegion arcArrow;

    private Quaternion targetRotation;
    private Vector3 mousePosition;
    public float angleToMouse;

    public float currentY = 0f;
    private float rotateSpeed = 18f;

    public Vector3 indicatePosition;
    public Vector3 mousePoint;
    public bool isInstantiated;

    private void Awake() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        arcArrow = GetComponent<ArcRegion>();

        inputAction = new DefaultInputActions();
        inputAction.UI.Point.performed += value => OnPoint(value.ReadValue<Vector2>());
    }

    private void OnEnable() {
        inputAction.Enable();
    }

    private void OnDisable() {
        fixedAngle = 0;
        isInstantiated = false;
        inputAction.Disable();
    }

    private Vector3 directionToMouse;
    private float fixedAngle;

    private void Update() {
        currentY += Time.deltaTime * rotateSpeed;
        if (currentY > 360f) currentY -= 360f;

        targetRotation = Quaternion.LookRotation(Camera.main.transform.forward) * Quaternion.AngleAxis(310f, Vector3.right)
        * Quaternion.AngleAxis(currentY, Vector3.up);

        transform.rotation = targetRotation;

        if (isInstantiated) {
            arcArrow.Angle = angleToMouse - currentY + 180 - playerTransform.eulerAngles.y;
        }
        else {
            CalcArrowAngle();
            CalcIndicatePosition();
        }
    }

    public void SetPlayerTransform() {
        playerTransform = FindObjectOfType<PlayerBehavior>().transform;
    }

    public void SetInstantitate(Transform original) {
        var origin = original.GetComponent<IndicatorControl>();
        isInstantiated = true;
        transform.position = original.position;
        transform.rotation = original.rotation;
        currentY = origin.currentY;
        CalcArrowAngle();
        mousePoint = origin.mousePoint;
        indicatePosition = origin.indicatePosition;
        angleToMouse = origin.angleToMouse;
        arcArrow.Angle = origin.arcArrow.Angle;
        rotateSpeed = 5f;
        gameObject.SetActive(true);
    }

    private void CalcArrowAngle() {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane plane = new Plane(transform.up, transform.position);
        if (plane.Raycast(ray, out float distance)) {
            mousePoint = ray.GetPoint(distance);
            directionToMouse = (mousePoint - transform.position).normalized;

            angleToMouse = Mathf.Atan2(directionToMouse.x, directionToMouse.z) * Mathf.Rad2Deg;
            arcArrow.Angle = angleToMouse - currentY + 180 - playerTransform.eulerAngles.y;
        }
    }
    //TODO: mouseWorldPos 동기화 안됨 SetInstantiate 에서

    private void CalcIndicatePosition () {
        indicatePosition = transform.position +
                    Quaternion.LookRotation(directionToMouse == Vector3.zero ? Vector3.one : directionToMouse)
                    * Quaternion.AngleAxis(10f, Vector3.up) * Vector3.forward * 20f;
        if (Physics.Raycast(indicatePosition, Vector3.down, out RaycastHit rayHit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            indicatePosition = rayHit.point;
        else if (Physics.Raycast(indicatePosition, Vector3.up, out rayHit, Mathf.Infinity, LayerMask.GetMask("Ground")))
            indicatePosition = rayHit.point;
    }

    private void OnPoint(Vector2 value) {
        mousePosition = value;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        // 해당 방향으로 Gizmo 그리기
        Vector3 gizmoPosition = indicatePosition;
        Gizmos.DrawWireSphere(gizmoPosition, 3f);


    }

}
