using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerMove : MonoBehaviour {
    private PlayerInputActions playerInputAction;
    private Collider playerCollider;
    private Animator playerAnimator;
    private Rigidbody playerRigid;

    private Vector3 moveDirection;
    private bool isDash = false;
    private bool isJump = false;

    private float currentSpeed = 0f;
    private float moveSpeed = 8f;
    private float dashSpeed = 17f;
    private float incSpeedRate = 7f;
    private float decSpeedRate = 15f;

    private float rotateSpeed = 8f;
    private float jumpSpeed = 250f;

    private Vector3 pastFramePosition;
    private Vector3 currentFramePosition;
    private void Awake() {
        playerInputAction = new PlayerInputActions();
        playerCollider = GetComponentInChildren<Collider>();
        playerAnimator = GetComponent<Animator>();
        playerRigid = GetComponent<Rigidbody>();

        playerInputAction.PlayerActions.Move.performed += value => OnMove(value.ReadValue<Vector2>());
        playerInputAction.PlayerActions.Dash.performed += value => OnDash();
        playerInputAction.PlayerActions.Jump.performed += value => OnJump();

        pastFramePosition = currentFramePosition = transform.position;
    }

    private void OnEnable() {
        playerInputAction.Enable();
    }

    private void OnDisable() {
        playerInputAction.Disable();
    }

    private void FixedUpdate() {
        Move();
        Rotate();

        CheckJumpValidity();
        CheckWallThroughBack();
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

    private void Move() {
        playerAnimator.SetFloat("MoveSpeed", currentSpeed);
        if (moveDirection == Vector3.zero)
            currentSpeed -= decSpeedRate * Time.deltaTime;
        else
            currentSpeed += incSpeedRate * Time.deltaTime;
        currentSpeed = ClampSpeed(currentSpeed);

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
        playerRigid.AddForce(Vector3.up * Mathf.Sqrt(jumpSpeed), ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision) {
        CheckLandGround(collision);
        CheckWallThroughFront();
    }

    private float checkJumpTolerance = 0.1f;
    private void CheckLandGround(Collision collision) {
        float colliderTop = collision.collider.bounds.center.y + (collision.collider.bounds.size.y / 2);
        float playerBottom = playerCollider.bounds.center.y - (playerCollider.bounds.size.y / 2);

        if (Mathf.Abs(playerBottom - colliderTop) <= checkJumpTolerance)
            isJump = false;
    }

    private void OnCollisionExit(Collision collision) {
        if (Mathf.Abs(playerRigid.velocity.y) > 1.7f)
            isJump = true;
    }

    private void OnCollisionStay(Collision collision) {
        if (!(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))) {
            CheckWallThroughFront();
            CheckSlopeValidity();
        }
    }

    private void CheckWallThroughFront() {
        Vector3 rayOrigin = transform.position;
        rayOrigin.y += 2f;

        if (Physics.Raycast(rayOrigin, transform.forward, out RaycastHit rayHit, 1f)) {
            Debug.Log($"Collider {rayHit.collider.name} is in front of player");
            currentSpeed = 0;
        }
    }

    private void CheckSlopeValidity() {
        if (isJump) return;
        Vector3 rayOrigin = transform.position;
        rayOrigin += transform.forward * 2f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit rayHit, 2f)) {
            if (Vector3.Angle(Vector3.up, rayHit.normal) > 40f) {
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

            if (Physics.BoxCast(rayStartPosition, checkRayBoxSize, Vector3.down, transform.rotation, 0.7f))
                isJump = false;
        }
    }


    private void CheckWallThroughBack() {
        currentFramePosition = transform.position;
        currentFramePosition.y += 1f;
        Vector3 direction = currentFramePosition - pastFramePosition;
        float distance = direction.magnitude;

        if (Physics.Raycast(pastFramePosition, direction, out RaycastHit rayHit, distance)) {
            currentSpeed = 0;
            Debug.Log($"Collider {rayHit.collider.name} is collided at nextFrame");
        }
        pastFramePosition = currentFramePosition;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, checkRayBoxSize);
    }
}

