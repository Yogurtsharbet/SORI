using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineFieldController : MonoBehaviour {
    private bool isCombined = false;
    private CombineSlotManager combineSlotManager;

    private void Awake() {
        combineSlotManager = FindObjectOfType<CombineSlotManager>();
    }

    public void OpenCombineField() {
        gameObject.SetActive(true);
    }

    public void CloseCombineField() {
        gameObject.SetActive(false);
    }
}
