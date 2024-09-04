using UnityEngine;

public class SynthesisManager : MonoBehaviour {

    private SynthesisSlotController[] slotControllers;

    private void Awake() {
        slotControllers = FindObjectsOfType<SynthesisSlotController>();
    }

    public RectTransform GetSlotRectTransform(int num) {
        return slotControllers[num].GetComponent<RectTransform>();
    }

}
