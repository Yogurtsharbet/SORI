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
            //1번, 2번 프레임의 슬롯 rectTransform 가져와서 비교
            if (thisFrameType != FrameType.AisB && thisFrameType != FrameType.AtoBisC) {
                combineManager.SetSubFrame(combineSlotController.SlotIndex, frame);
                combineSlotController.OpenFrame(frame.Type);
                IsExistFrame = true;
                isOpenSlot = true;
            }
        }
        else if (baseFrameType == FrameType.AandB) {
            //3번 프레임 > 1,2번 프레임만 추가가능
            //3번 프레임 슬롯 rectTransform 가져와서 비교
            if (thisFrameType != FrameType.AandB && thisFrameType != FrameType.NotA) {
                combineManager.SetSubFrame(combineSlotController.SlotIndex, frame);
                combineSlotController.OpenFrame(frame.Type);
                IsExistFrame = true;
                isOpenSlot = true;
            }
        }
        return isOpenSlot;
    }

    public void OpenCombineFiled(int key, Frame frame) {
        combineManager.OpenBaseFrameSlot(key, frame);
        IsExistFrame = true;
    }

    public void DeleteFrame() {
        IsExistFrame = false;
        combineSlotController.CloseFrame();
    }
}
