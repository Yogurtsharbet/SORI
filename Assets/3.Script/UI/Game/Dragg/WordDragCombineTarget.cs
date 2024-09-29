using UnityEngine;

public class WordDragCombineTarget : MonoBehaviour {
    public RectTransform ThisRectTransform { get; private set; }
    private CombineManager combineManager;
    private CombineSlotController combineSlotController;
    private FrameDragTarget frameDragTarget;

    private void Awake() {
        ThisRectTransform = GetComponent<RectTransform>();
        combineManager = FindObjectOfType<CombineManager>();
        combineSlotController = gameObject.GetComponent<CombineSlotController>();
        if (gameObject.TryGetComponent<FrameDragTarget>(out FrameDragTarget fr)) {
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

    public bool IsFrameTypeThree() {
        if (combineSlotController.ParentType == FrameType.AandB)
            return true;
        else return false;
    }

    //3타입 프레임에 이미 단어 있는지 확인

    public bool IsFrameAlreadyExist() {
        for(int i=0; i < combineManager.BaseFrame.CountOfFrame(); i++) {
            if (combineManager.BaseFrame.GetFrame(i)!=null) {
                return true;
            }
        }
        return false;
    }

    public bool IsSubFrameAlreadyExist() {
        Frame frame = combineManager.BaseFrame.GetFrame(combineSlotController.SlotIndex);
        for (int i = 0; i < frame.CountOfFrame(); i++) {
            if (frame.GetFrame(i) != null) {
                return true;
            }
        }
        return false;
    }

    public Word OpenCombineWord(Word word) {
        int slotIndex = combineSlotController.OpenWord(word);
        Word newWord = combineManager.GetWord(slotIndex);
        combineManager.SetWord(slotIndex, word);
        return newWord;
    }

    public Word OpenCombineWord(Word word, WordDragCombineTarget parent) {
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
        if (frameDragTarget == null) {
            return false;
        }
        else {
            return frameDragTarget.IsExistFrame;
        }
    }
}
