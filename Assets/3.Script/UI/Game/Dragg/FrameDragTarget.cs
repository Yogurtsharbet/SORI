using UnityEngine;

public class FrameDragTarget : MonoBehaviour {
    private CombineManager combineManager;
    private CombineSlotController combineSlotController;

    public bool IsExistFrame { get; private set; }

    private void OnDisable() {
        IsExistFrame = false;
    }

    private void Awake() {
        combineManager = FindObjectOfType<CombineManager>();
        combineSlotController = gameObject.GetComponent<CombineSlotController>();
    }

    public bool IsFrameActive() {
        return combineManager.BaseFrame.IsActive;
    }

    public bool OpenCombineSlot(int key, Frame frame) {
        bool isOpenSlot = false;

        FrameType baseFrameType = combineManager.BaseFrame.Type;
        FrameType thisFrameType = frame.Type;

        if (baseFrameType == FrameType.AisB || baseFrameType == FrameType.AtoBisC) {
            //1,2번 프레임 > 3,4번 프레임만 추가 가능
            if (thisFrameType != FrameType.AisB && thisFrameType != FrameType.AtoBisC) {
                combineManager.SetSubFrame(combineSlotController.SlotIndex, frame);
                combineSlotController.OpenFrame(frame.Type);
                IsExistFrame = true;
                isOpenSlot = true;
            }
        }
        else if (baseFrameType == FrameType.AandB) {
            //3번 프레임 > 1,2번 프레임만 추가가능
            //둘중 한개의 슬롯에 단어가 있으면 단어만 추가 가능, 한개의 슬롯에 프레임이 있으면 프레임만 추가 가능
            for(int i=0; i < combineManager.BaseFrame.CountOfFrame(); i++) {
                if (combineManager.BaseFrame.GetWord(i) !=null) {
                    return false;
                }
            }
            if (thisFrameType != FrameType.AandB && thisFrameType != FrameType.NotA) {
                combineManager.SetSubFrame(combineSlotController.SlotIndex, frame);
                combineSlotController.OpenFrame(frame.Type);
                IsExistFrame = true;
                isOpenSlot = true;
            }
        }
        return isOpenSlot;
    }

    public void OpenCombineFiled(Frame frame) {
        combineManager.OpenBaseFrameSlot(frame);
        IsExistFrame = true;
    }

    public void DeleteFrame() {
        IsExistFrame = false;
        combineSlotController.CloseFrame();
    }
}
