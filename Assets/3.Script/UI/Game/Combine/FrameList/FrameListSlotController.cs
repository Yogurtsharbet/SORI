using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [UI] 문장 목록 - 문장 슬롯 컨트롤러. 문장 리스트 중 한 문장 틀
public class FrameListSlotController : MonoBehaviour, IPointerClickHandler {
    private CombineContainer combineContainer;
    private List<(int, CombineSlotController)> combineSlotControllers = new List<(int, CombineSlotController)>();
    private FrameListManager frameListManager;

    private GameObject[] frameByType = new GameObject[4];

    public int key { get; private set; }
    public Frame thisFrame { get; private set; }            //조합 문장 정보
    private FrameType thisFrameType;                        //현재 프레임 타입

    private void Awake() {
        frameListManager = FindObjectOfType<FrameListManager>();
        combineContainer = FindObjectOfType<CombineContainer>();
        Image img = combineContainer.GetComponentInChildren<Image>();
        for (int i = 0; i < 4; i++) {
            frameByType[i] = gameObject.transform.GetChild(i).gameObject;
            CombineSlotController[] tempArr = frameByType[i].GetComponentsInChildren<CombineSlotController>();
            for (int j = 0; j < tempArr.Length; j++) {
                combineSlotControllers.Add((i, tempArr[j]));
            }
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

        List<CombineSlotController> slot = new List<CombineSlotController>();
        List<CombineSlotController> subSlot = new List<CombineSlotController>();

        //slot, subSlot 나눔
        for (int i = 0; i < combineSlotControllers.Count; i++) {
            if (combineSlotControllers[i].Item1 == (int)thisFrameType - 1) {
                if (combineSlotControllers[i].Item2.ChildCount == 1) {
                    slot.Add(combineSlotControllers[i].Item2);
                }
                else if (combineSlotControllers[i].Item2.ChildCount > 1) {
                    subSlot.Add(combineSlotControllers[i].Item2);
                }
            }
        }

        //frame이 존재할때
        for (int i = 0; i < 3; i++) {
            Frame tempFrame = thisFrame.GetFrame(i);
            if (tempFrame != null) {
                for (int j = 0; j < slot.Count; j++) {
                    if (slot[j].SlotIndex == i) {
                        slot[j].OpenFrame(tempFrame.Type);
                    }
                }
                for (int j = 0; j < 3; j++) {
                    if(tempFrame.GetWord(j) != null) {
                        for(int k =0; k < subSlot.Count; k++) {
                            if(subSlot[k].SlotIndex == j) {
                                subSlot[k].OpenWord(tempFrame.GetWord(j));
                            }
                        }
                    }
                }
            }

            //단어가 베이스 프레임의 슬롯에 있을때
            if (thisFrame.GetWord(i) != null) {
                for (int j = 0; j < slot.Count; j++) {
                    if (slot[j].SlotIndex == i) {
                        slot[j].OpenWord(thisFrame.GetWord(i));
                    }
                }
            }
        }
    }

    public bool IsActive() {
        return thisFrame.IsActive;
    }

    public void FrameSlotDelete() {
        frameListManager.DeleteFrame(key);
    }
}
