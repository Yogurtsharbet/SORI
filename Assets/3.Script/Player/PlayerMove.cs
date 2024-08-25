using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

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

    private void Awake() {
        playerInputAction = new PlayerInputActions();
        playerCollider = GetComponentInChildren<Collider>();
        playerAnimator = GetComponent<Animator>();
        playerRigid = GetComponent<Rigidbody>();

        playerInputAction.PlayerActions.Move.performed += value => OnMove(value.ReadValue<Vector2>());
        playerInputAction.PlayerActions.Dash.performed += value => OnDash();
        playerInputAction.PlayerActions.Jump.performed += value => OnJump();
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

    private float checkJumpTolerance = 0.1f;
    private void OnCollisionEnter(Collision collision) {
        float colliderTop = collision.collider.bounds.center.y + (collision.collider.bounds.size.y / 2);
        float playerBottom = playerCollider.bounds.center.y - (playerCollider.bounds.size.y / 2);

        if (Mathf.Abs(playerBottom - colliderTop) <= checkJumpTolerance)
            isJump = false;
    }

    private void OnCollisionExit(Collision collision) {
        if (Mathf.Abs(playerRigid.velocity.y) > 1.7f)
            isJump = true;
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
    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position, checkRayBoxSize);
    }
}

