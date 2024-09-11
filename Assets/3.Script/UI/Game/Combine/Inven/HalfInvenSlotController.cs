using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [UI] 상점, 조합창 - 인벤토리 슬롯 컨트롤러
public class HalfInvenSlotController : CommonInvenSlotController, IEndDragHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler {
    private HalfInvenManager halfInvenManager;
    private CombineSlotManager combineSlotManager;
    private PlayerInvenController playerInven;

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
        playerInven = FindObjectOfType<PlayerInvenController>();

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

    public void OnBeginDrag(PointerEventData eventData) {
        originalParent = wordText.rectTransform.parent as RectTransform;
        originalPosition = wordText.rectTransform.anchoredPosition;
        wordText.transform.SetParent(canvas.transform, true);
        wordText.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out position);
        wordText.rectTransform.anchoredPosition = position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        wordText.transform.SetParent(originalParent, true);
        wordText.rectTransform.anchoredPosition = originalPosition;
        if (halfInvenManager.IsCombineMode) {
            if (combineSlotManager.CanCombine) {
                int sentenceSlotNum = checkSentenceSlot(eventData);
                if (sentenceSlotNum != -1) {
                    //조합창으로 드래그
                    Word invenWord = playerInven.GetWordIndex(key);
                    if (combineSlotManager.CheckIsSlotExist(sentenceSlotNum)) {
                        combineSlotManager.SwitchingWordToInven(sentenceSlotNum, invenWord);
                    }
                    else {
                        combineSlotManager.SetSlotWords(sentenceSlotNum, invenWord);
                        playerInven.RemoveItemIndex(key);
                    }
                }
                else {
                    int invenSlotNum = checkAnotherSlot(eventData);
                    if (invenSlotNum != -1) {
                        //인벤 내부 스위칭
                        halfInvenManager.SetInvenSwitching(key, invenSlotNum);
                    }
                }
            }
        }
    }

    private int checkSentenceSlot(PointerEventData eventData) {
        if (RectTransformUtility.RectangleContainsScreenPoint(combineSlotManager.GetSlotRectTransform(0), eventData.position, eventData.pressEventCamera)) {
            return 0;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(combineSlotManager.GetSlotRectTransform(1), eventData.position, eventData.pressEventCamera)) {
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
