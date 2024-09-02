using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCombine : MonoBehaviour {
    private GameObject[] targetUI;
    private Animator canvasAnimator;

    //TODO: COMBINE UI LOGIC

    private void Awake() {

        canvasAnimator = GetComponent<Animator>();

    }

    public void SetCombine() {
        canvasAnimator.Play("CombineMode");
    }
    public void UnsetCombine() {
        canvasAnimator.Play("CombineMode_Reverse");
    }
}