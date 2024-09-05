using UnityEngine;

public class SynthesisManager : MonoBehaviour {

    private SynthesisSlotController[] slotControllers;

    private void Awake() {
        slotControllers = FindObjectsOfType<SynthesisSlotController>();
    }

    public RectTransform GetSlotRectTransform(int num) {
        return slotControllers[num].GetComponent<RectTransform>();
    }

    public bool GetExistFromIndex(int index) {
        return slotControllers[index].GetWordExist();
    }

    public Word GetSlotWordFromIndex(int index) {
        return slotControllers[index].GetSlotWord();
    }

    public void SlotItemChangeFromIndex(int index, Word word) {
        slotControllers[index].RemoveSlotWord();
        slotControllers[index].SetSlotWord(word);
    }
}
