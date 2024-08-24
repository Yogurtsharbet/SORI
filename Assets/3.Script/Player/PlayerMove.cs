using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour {
    private Rigidbody playerRigid;
    private Animator playerAnimator;

    private Vector3 moveDirection;
    private bool isDash = false;

    private float currentSpeed = 0f;
    private float moveSpeed = 7f;
    private float dashSpeed = 12f;
    private float incSpeedRate = 5f;
    private float decSpeedRate = 8f;

    private float rotateSpeed = 4f;

    private void Awake() {
        playerRigid = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        Move();
        Rotate();
    }

    private void OnMove(InputValue value) {
        Vector2 inputValue = value.Get<Vector2>();
        if (inputValue != null) 
            moveDirection = new Vector3(inputValue.x, 0f, inputValue.y);
    }

    private void OnDash(InputValue value) {
        isDash = value.isPressed;
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
