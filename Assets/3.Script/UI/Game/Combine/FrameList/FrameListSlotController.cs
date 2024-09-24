using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [UI] 문장 목록 - 문장 슬롯 컨트롤러. 문장 리스트 중 한 문장 틀
public class FrameListSlotController : MonoBehaviour, IPointerClickHandler {
    private CombineContainer combineContainer;
    private FrameListManager frameListManager;
    private List<(int, CombineSlotController)> combineSlotControllers = new List<(int, CombineSlotController)>();

    private GameObject[] frameByType = new GameObject[4];

    public int key { get; private set; }
    public Frame thisFrame { get; private set; }            //조합 문장 정보
    private FrameType thisFrameType;                        //현재 프레임 타입

    private void Awake() {
        frameListManager = FindObjectOfType<FrameListManager>();
        combineContainer = FindObjectOfType<CombineContainer>();
        for (int i = 0; i < 4; i++) {
            frameByType[i] = gameObject.transform.GetChild(i).gameObject;
            CombineSlotController[] tempSlots = frameByType[i].GetComponentsInChildren<CombineSlotController>();
            for (int j = 0; j < tempSlots.Length; j++) {
                combineSlotControllers.Add((i, tempSlots[j]));
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

        frameByType[((int)thisFrameType - 1)].SetActive(true);

        if (thisFrameType != FrameType.NotA) {
            List<CombineSlotController> tempSlots = new List<CombineSlotController>();
            for (int i = 0; i < combineSlotControllers.Count; i++) {
                if (combineSlotControllers[i].Item1 == ((int)thisFrameType - 1)) {
                    tempSlots.Add(combineSlotControllers[i].Item2);
                }
            }

            int wordCount = 0;
            for (int i = 0; i < 3; i++) {
                if (thisFrame.GetWord(i) != null) {
                    for (int j = 0; j < tempSlots.Count; j++) {
                        if (tempSlots[j].SlotIndex == i && tempSlots[j].ChildCount > 1) {
                            tempSlots[j].OpenWord(thisFrame.GetWord(i));
                            wordCount++;
                        }
                        if (wordCount >= thisFrame.CountOfFrame())
                            break;
                    }
                }
            }

            if (wordCount < thisFrame.CountOfFrame()) {

                for (int i = 0; i < 3; i++) {
                    Frame tempFrame = thisFrame.GetFrame(i);
                    if (tempFrame == null) continue;

                    foreach (var slot in tempSlots) {
                        if (slot.SlotIndex != i || slot.ChildCount <= 1) continue;

                        slot.OpenFrame(tempFrame.Type);

                        for (int k = 0; k < 3; k++) {
                            Word word = tempFrame.GetWord(k);
                            if (word == null) continue;

                            foreach (var subSlot in tempSlots) {
                                if (subSlot.SlotIndex == k && subSlot.ChildCount < 2 && subSlot.ParentType == tempFrame.Type) {
                                    subSlot.OpenWord(word);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        else {

        }
    }

    public bool IsActive() {
        return thisFrame.IsActive;
    }

    public void FrameSlotDelete() {
        frameListManager.DeleteFrame(key);
    }
}
