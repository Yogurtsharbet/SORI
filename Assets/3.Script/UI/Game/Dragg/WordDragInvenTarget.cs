using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordDragInvenTarget : MonoBehaviour {
    private InvenSlotController invenSlotController;
    private SynthesisSlotController synthesisSlotController;

    private bool isInvenSlot = false;       //inven slot이면 true, 조합창 slot이면 false
    public bool IsInvenSlot => isInvenSlot;

    private void Awake() {

        if (gameObject.TryGetComponent<InvenSlotController>(out InvenSlotController invenSlot)) {
            invenSlotController = invenSlot;
            isInvenSlot = true;
            synthesisSlotController = null;
        }
        else {
            invenSlotController = null;
            if (gameObject.TryGetComponent<SynthesisSlotController>(out SynthesisSlotController syntheSlot)) {
                synthesisSlotController = syntheSlot;
                isInvenSlot = false;
            }
        }
    }

    public int InvenSlotKey() {
        return invenSlotController.Key;
    }

    public void AddWord(int invenKey) {
        synthesisSlotController.AddWord(invenKey);
    }
}
