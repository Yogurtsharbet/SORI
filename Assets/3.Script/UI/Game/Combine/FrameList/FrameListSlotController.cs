using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [UI] 문장 목록 - 문장 슬롯 컨트롤러. 문장 리스트 중 한 문장 틀
public class FrameListSlotController : MonoBehaviour, IPointerClickHandler {
    private CombineManager combineManager;
    private CombineContainer combineContainer;
    private FrameListManager frameListManager;

    private GameObject[] frameByType = new GameObject[4];

    public int key { get; private set; }
    public Frame thisFrame { get; private set; }        //조합 문장 정보
    private FrameType thisFrameType;                        //현재 프레임 타입

    private void Awake() {
        frameListManager = FindObjectOfType<FrameListManager>();
        combineManager = FindObjectOfType<CombineManager>();
        combineContainer = FindObjectOfType<CombineContainer>();
        Image img = combineContainer.GetComponentInChildren<Image>();
        for (int i = 0; i < 4; i++) {
            frameByType[i] = gameObject.transform.GetChild(i).gameObject;
        }
    }

    public void SetKey(int num) {
        //총 frame을 10개로 하고 풀링 -> key를 전체 framelist index로 함
        key = num;
    }

    public void OpenSlot() {
        gameObject.SetActive(true);
    }

    public void CloseSlot() {
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) {
        combineContainer.OpenCombineField();
    }

    public void SetFrameData(Frame frame) {
        for (int i = 0; i < 4; i++)
            frameByType[i].SetActive(false);

        thisFrame = frame;
        thisFrameType = frame.Type;
        switch (thisFrameType) {
            case FrameType._Random:
                break;
            case FrameType.AisB:
                frameByType[0].SetActive(true);
                break;
            case FrameType.AtoBisC:
                frameByType[1].SetActive(true);
                break;
            case FrameType.AandB:
                frameByType[2].SetActive(true);
                break;
            case FrameType.NotA:
                frameByType[3].SetActive(true);
                break;
        }
    }

    public bool IsActive() {
        return thisFrame.IsActive;
    }

    public void FrameSlotDelete() {
        frameListManager.DeleteFrame(key);
    }
}
