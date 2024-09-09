using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineContainer : MonoBehaviour {
    private bool isCombined = false;
    private CombineSlotManager combineSlotManager;

    private Vector3 openPos = new Vector3(-246f, 27f, 0);
    private Vector3 closePos = new Vector3(246f, -864f, 0);

    private void Awake() {
        combineSlotManager = FindObjectOfType<CombineSlotManager>();
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenCombineField() {
        gameObject.SetActive(true);
        FunctionMove(gameObject.transform, openPos);
    }

    public void CloseCombineField() {
        FunctionMove(gameObject.transform, closePos);
        gameObject.SetActive(false);
    }

    public void OpenCombineSlot() {
        combineSlotManager.OpenCombineSlot();
    }

    private void FunctionMove(Transform origin, Vector3 destiny) {
        origin.DOLocalMove(destiny, 1.5f, true);
    }
}
