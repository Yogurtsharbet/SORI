using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour {
    private PlayerInputActions playerInputAction;
    private Rigidbody playerRigid;
    private Animator playerAnimator;

    private Vector3 moveDirection;
    private bool isDash = false;

    private float currentSpeed = 0f;
    private float moveSpeed = 7f;
    private float dashSpeed = 16f;
    private float incSpeedRate = 7f;
    private float decSpeedRate = 10f;

    private float rotateSpeed = 4f;

    private void Awake() {
        playerInputAction = new PlayerInputActions();
        playerAnimator = GetComponent<Animator>();
        playerRigid = GetComponent<Rigidbody>();

        playerInputAction.PlayerActions.Move.performed += value => OnMove(value.ReadValue<Vector2>());
        playerInputAction.PlayerActions.Dash.performed += value => OnDash();

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
    }

    private void OnMove(Vector2 value) {
        moveDirection = new Vector3(value.x, 0f, value.y);
    }

    private void OnDash() {
        isDash = !isDash;
    }

    private void Move() {
        playerAnimator.SetFloat("MoveSpeed", currentSpeed);
        if (moveDirection == Vector3.zero)
            currentSpeed -= decSpeedRate * Time.deltaTime;
        else
            currentSpeed += incSpeedRate * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, isDash ? dashSpeed : moveSpeed);

        Vector3 targetPosition = playerRigid.position + (moveDirection * Time.deltaTime * currentSpeed);
        playerRigid.MovePosition(targetPosition);
    }

    private void Rotate() {
        if (moveDirection != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized);
            playerRigid.rotation = Quaternion.Lerp(playerRigid.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

    }
}
