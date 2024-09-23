using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordDragTarget : MonoBehaviour {
    public RectTransform ThisRectTransform { get; private set; }
    private CombineManager combineManager;
    private CombineSlotController combineSlotController;
    private FrameDragTarget frameDragTarget;

    private void Awake() {
        ThisRectTransform = GetComponent<RectTransform>();
        combineManager = FindObjectOfType<CombineManager>();
        combineSlotController = gameObject.GetComponent<CombineSlotController>();
        if(gameObject.TryGetComponent<FrameDragTarget>(out FrameDragTarget fr)) {
            frameDragTarget = fr;
        }
        else {
            frameDragTarget = null;
        }
    }
    public bool IsFrameActive() {
        return combineManager.BaseFrame.IsActive;
    }

    public void ChangeLocale(Vector3 scale) {
        ThisRectTransform.localScale = scale;
    }

    public Word OpenCombineWord(Word word) {
        int slotIndex = combineSlotController.OpenWord(word);
        Word newWord = combineManager.GetWord(slotIndex);
        combineManager.SetWord(slotIndex, word);
        return newWord;
    }

    public Word OpenCombineWord(Word word, WordDragTarget parent) {
        int slotIndex = combineSlotController.OpenWord(word); 
        Word newWord = combineManager.GetWord(parent.combineSlotController.SlotIndex, slotIndex);
        combineManager.SetWord(parent.combineSlotController.SlotIndex, slotIndex, word);
        return newWord;
    }

    public void SwitchingFrameToWord(Word word) {
        int slotIndex = combineSlotController.OpenWord(word);
        combineManager.SwitchingFrameToWord(slotIndex, word);
        frameDragTarget.DeleteFrame();
    }

    public bool IsSlotExistFrame() {
        if(frameDragTarget == null) {
            return false;
        }
        else {
            return frameDragTarget.IsExistFrame;
        }
    }
}
