using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    private PlayerInputActions playerInputAction;
    private PlayerBehavior playerBehavior;
    private Collider playerCollider;
    private Animator playerAnimator;
    private Rigidbody playerRigid;

    private PlayerRunningParticle Dust;

    private Vector3 moveDirection;
    private bool isDash = false;
    private bool isJump = false;

    private float currentSpeed = 0f;
    private float moveSpeed = 8f;
    private float dashSpeed = 17f;
    private float incSpeedRate = 7f;
    private float decSpeedRate = 15f;

    private float rotateSpeed = 8f;
    private float jumpSpeed = 350f;

    private Vector3 pastFramePosition;
    private Vector3 currentFramePosition;
    private void Awake() {
        playerInputAction = new PlayerInputActions();
        playerCollider = GetComponentInChildren<Collider>();
        playerBehavior = GetComponent<PlayerBehavior>();
        playerAnimator = GetComponent<Animator>();
        playerRigid = GetComponent<Rigidbody>();

        Dust = GetComponent<PlayerRunningParticle>();

        playerInputAction.PlayerMovement.Move.performed += value => OnMove(value.ReadValue<Vector2>());
        playerInputAction.PlayerMovement.Dash.performed += value => OnDash();
        playerInputAction.PlayerMovement.Jump.performed += value => OnJump();

        pastFramePosition = currentFramePosition = transform.position;
    }

    private void OnEnable() {
        playerInputAction.Enable();
    }

    private void OnDisable() {
        playerInputAction.Disable();
    }

    private void OnMove(Vector2 value) {
        moveDirection = new Vector3(value.x, 0f, value.y);
    }

    private void OnDash() {
        isDash = !isDash;
    }

    private void OnJump() {
        if (isJump) return;
        isJump = true;
        Jump();
    }

    private void FixedUpdate() {
        if (CheckMovementValidity()) {
            Move();
            Rotate();
        }

        CheckJumpValidity();
        CheckWallThroughBack();
        CheckIdleAnimation();
        CheckResetDash();
    }

    private void CheckResetDash() {
        if (moveDirection == Vector3.zero) isDash = false;
    }

    private bool CheckMovementValidity() {
        // return true when player is able to move
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        return !stateInfo.IsTag("DisableMovement");
    }

    private bool isIdle = false;
    private float idleChangeTerm;
    private float idleChangeTime = 0;
    private int idleRepeatTime = 0;

    private void CheckIdleAnimation() {
        if (currentSpeed != 0) {
            playerAnimator.SetFloat("IdleState", 0);
            idleRepeatTime = 0;
            isIdle = false;
        }
        else if (!isIdle) {
            isIdle = true;
            idleChangeTime = Time.time;
            idleChangeTerm = Random.Range(15f, 25f);
        }

        if (isIdle && (Time.time > idleChangeTime + idleChangeTerm) && idleRepeatTime == 0) {
            playerAnimator.SetFloat("IdleState", Random.Range(1, 3));
            idleRepeatTime = Random.Range(2, 4);
        }
    }

    private void ResetIdleAnimation() {
        idleRepeatTime--;
        if (idleRepeatTime <= 0) {
            playerAnimator.SetFloat("IdleState", 0);
            idleRepeatTime = 0;
            idleChangeTime = Time.time;
            idleChangeTerm = Random.Range(15f, 25f);
        }
    }

    private void Move() {
        if (moveDirection == Vector3.zero)
            currentSpeed -= decSpeedRate * Time.deltaTime;
        else
            currentSpeed += incSpeedRate * Time.deltaTime;
        currentSpeed = ClampSpeed(currentSpeed);

        playerAnimator.SetFloat("MoveSpeed", currentSpeed);
        Dust.SetDustRate(isJump ? 0 : currentSpeed);

        Vector3 targetPosition = playerRigid.position + (moveDirection * Time.deltaTime * currentSpeed);
        playerRigid.MovePosition(targetPosition);
    }


    private float ClampSpeed(float speed) {
        if (speed > (isDash ? dashSpeed : moveSpeed))
            speed -= decSpeedRate * Time.deltaTime;
        else if (speed < 0) speed = 0;
        return speed;
    }

    private void Rotate() {
        if (moveDirection != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            playerRigid.rotation = Quaternion.Lerp(playerRigid.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }

    private void Jump() {
        if (CheckMovementValidity()) {
            playerRigid.AddForce(Vector3.up * Mathf.Sqrt(jumpSpeed), ForceMode.Impulse);
            Dust.Jump();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        CheckLandGround(collision);
        if (!collision.collider.isTrigger) CheckWallThroughFront();
    }

    private float checkJumpTolerance = 0.1f;
    private void CheckLandGround(Collision collision) {
        float colliderTop = collision.collider.bounds.center.y + (collision.collider.bounds.size.y / 2);
        float playerBottom = playerCollider.bounds.center.y - (playerCollider.bounds.size.y / 2);

        if (Mathf.Abs(playerBottom - colliderTop) <= checkJumpTolerance) {
            isJump = false;
            Dust.Jump();
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (Mathf.Abs(playerRigid.velocity.y) > 2.5f)
            isJump = true;
    }

    private void OnCollisionStay(Collision collision) {
        if (!(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))) {
            if (!collision.collider.isTrigger) {
                CheckWallThroughFront();
                CheckSlopeValidity();
            }
        }
        else {
            if (isJump) Dust.Jump();
            isJump = false;
        }
    }

    private void CheckWallThroughFront() {
        Vector3 rayOrigin = transform.position;
        rayOrigin.y += 2f;

        if (Physics.Raycast(rayOrigin, transform.forward, out RaycastHit rayHit, 1f)) {
            if (rayHit.collider.isTrigger) return;

            Debug.Log($"Collider {rayHit.collider.name} is in front of player");
            currentSpeed = 0;
            SetCollideAnimation();
        }
    }

    private void CheckSlopeValidity() {
        if (isJump) return;
        Vector3 rayOrigin = transform.position;
        rayOrigin += transform.forward * 2f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit rayHit, 2f)) {
            if (Vector3.Angle(Vector3.up, rayHit.normal) > 30f) {
                if (rayHit.collider.isTrigger ||
                    transform.position.y > rayHit.point.y) return;

                Debug.Log("Slope is Collided");
                currentSpeed = 0f;
            }
        }

    }

    private Vector3 checkRayBoxSize = new Vector3(2f, 0.03f, 2f);
    private void CheckJumpValidity() {
        playerAnimator.SetBool("isJump", isJump);
        if (Mathf.Abs(playerRigid.velocity.y) < 0.01f) {
            Vector3 rayStartPosition = transform.position;
            rayStartPosition.y += 0.6f;

            if (Physics.BoxCast(rayStartPosition, checkRayBoxSize, Vector3.down, transform.rotation, 0.7f)) {
                if (isJump) Dust.Jump();
                isJump = false;
            }
        }
    }

    private void CheckWallThroughBack() {
        currentFramePosition = transform.position;
        currentFramePosition.y += 1f;
        Vector3 direction = currentFramePosition - pastFramePosition;
        float distance = direction.magnitude;

        if (Physics.Raycast(pastFramePosition, direction, out RaycastHit rayHit, distance)) {
            if (rayHit.collider.isTrigger || rayHit.collider.CompareTag("Player") ||
                rayHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) return;

            Debug.Log($"Collider {rayHit.collider.name} is collided at nextFrame");
            currentSpeed = 0;
            SetCollideAnimation();
        }
        pastFramePosition = currentFramePosition;
    }

    private Queue<float> impactTime = new Queue<float>();
    private void SetCollideAnimation() {
        playerAnimator.SetTrigger("triggerImpact");
        impactTime.Enqueue(Time.time);

        if (impactTime.Count >= 3) {
            float impactTerm = Time.time - impactTime.Peek();
            if (impactTerm < 0.8f) {
                // impact 3 times in 0.8sec will be processed to just one impact
                impactTime.Clear();
                playerBehavior.TakeDamage(0.7f);
            }
            else if (impactTerm < 9f) {
                impactTime.Clear();
                playerBehavior.TakeDamage(3f);
                playerAnimator.SetTrigger("triggerImpactHard");
            }
            else {
                impactTime.Dequeue();
                playerBehavior.TakeDamage(0.7f);
            }
        }
    }
    public void ClearImpactTime() {
        impactTime.Clear();
    }

    public void ClearCurretSpeed() {
        currentSpeed = 0f;
        moveDirection = Vector3.zero;
        Dust.SetDustRate(0);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, checkRayBoxSize);
    }

}
//TODO: 대시랑 점프가 동시에 안눌리는 버그
