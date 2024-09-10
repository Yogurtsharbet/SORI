using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HalfInvenSlotController : CommonInvenSlotController, IEndDragHandler, IPointerClickHandler {
    private HalfInvenManager halfInvenManager;
    private CombineSlotManager combineSlotManager;

    private void Awake() {
        //TODO: 게임씬으로 분리시  CANVAS 분리 필요
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas cn in canvases) {
            if (cn.name.Equals("GameCanvas")) {
                canvas = cn;
            }
        }

        closeController = GetComponentInChildren<InvenSlotCloseController>();
        halfInvenManager = FindObjectOfType<HalfInvenManager>();
        combineSlotManager = FindObjectOfType<CombineSlotManager>();

        wordText = GetComponentInChildren<Text>();
        wordText.text = string.Empty;
        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image img in images) {
            if (img.name.Equals("Type")) {
                typeIcon = img;
            }
            else if (img.name.Equals("Rank")) {
                rankOutIcon = img;
            }
            else if (img.name.Equals("RankColor")) {
                rankInnerIcon = img;
            }
            else if (img.name.Equals("Continue")) {
                continueIcon = img;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        wordText.transform.SetParent(originalParent, true);
        wordText.rectTransform.anchoredPosition = originalPosition;

        int sentenceSlotNum = checkSentenceSlot(eventData);
        if (sentenceSlotNum != -1) {

        }
        else {
            int invenSlotNum = checkAnotherSlot(eventData);
            if (invenSlotNum != -1) {
                //인벤 내부 스위칭
                halfInvenManager.SetInvenSwitching(key, invenSlotNum);
            }
        }
    }

    private int checkSentenceSlot(PointerEventData eventData) {
        if (RectTransformUtility.RectangleContainsScreenPoint(combineSlotManager.GetSlotRectTransform(0), eventData.position, eventData.pressEventCamera)) {
            return 0;
        }else if (RectTransformUtility.RectangleContainsScreenPoint(combineSlotManager.GetSlotRectTransform(1), eventData.position, eventData.pressEventCamera)) {
            return 1;
        }
        else {
            return -1;
        }
    }

    private int checkAnotherSlot(PointerEventData eventData) {
        int slotIndex = -1;
        for (int i = 0; i < 21; i++) {
            if (RectTransformUtility.RectangleContainsScreenPoint(halfInvenManager.GetSlotRectTransform(i), eventData.position, eventData.pressEventCamera)) {
                slotIndex = i;
                break;
            }
        }
        return slotIndex;
    }

    public void OnPointerClick(PointerEventData eventData) {
        //TODO: 클릭했을때 상점이 열려있으면 삭제할건지 dialog
    }
}
