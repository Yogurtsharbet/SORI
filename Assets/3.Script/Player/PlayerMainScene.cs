using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainScene : MonoBehaviour {
    private Animator playerAnimator;

    private void Awake() {
        playerAnimator = GetComponent<Animator>();
    }

    private void Start() {
        StartCoroutine(PlayerAnimationState());
    }

    private IEnumerator PlayerAnimationState() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(10, 20));
            switch (Random.Range(0, 1)) {
                case 0: playerAnimator.Play("Yawn"); break;
                case 1: StartCoroutine(PlayerIdleState(1)); break;
                case 2: StartCoroutine(PlayerIdleState(2)); break;
            }
        }
    }

    private IEnumerator PlayerIdleState(float state) {
        playerAnimator.SetFloat("IdleState", state);
        playerAnimator.Play("IdleState");
        yield return new WaitForSeconds(5f);
        playerAnimator.SetFloat("IdleState", 0);
    }

    private void ResetIdleAnimation() { return; }
}