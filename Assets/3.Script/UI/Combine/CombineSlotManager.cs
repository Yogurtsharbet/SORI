using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSlotManager : MonoBehaviour {
    private CombineSlotController[] combineSlotControllers;

    private void Awake() {
        combineSlotControllers = GetComponentsInChildren<CombineSlotController>();    
    }

    private void Start() {
        CloseCombineSlot();
        combineSlotControllers[0].CloseSlot();
        combineSlotControllers[1].CloseSlot();
    }

    public void OpenCombineSlot() {
        gameObject.SetActive(true);
    }

    public void CloseCombineSlot() {
        gameObject.SetActive(false);
    }

    public RectTransform GetSlotRectTransform(int num) {
        return combineSlotControllers[num].GetComponent<RectTransform>();
    }


}
