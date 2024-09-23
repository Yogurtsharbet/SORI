using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordDragTarget : MonoBehaviour {
    private CombineManager combineManager;
    private CombineSlotController combineSlotController;

    private void Awake() {
        combineManager = FindObjectOfType<CombineManager>();
        combineSlotController = gameObject.GetComponent<CombineSlotController>();
    }

    public void OpenCombineWord(Word word) {
        int slotIndex = combineSlotController.OpenWord(word);
        combineManager.SetWord(slotIndex, word);
    }

    public void OpenCombineWord(Word word, WordDragTarget parent) {
        int slotIndex = combineSlotController.OpenWord(word);
        combineManager.SetWord(parent.combineSlotController.SlotIndex, slotIndex, word);
    }
}
