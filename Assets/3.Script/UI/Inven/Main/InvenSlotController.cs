using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlotController : CommonInvenSlotController, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    
    private SynthesisManager synthesisManager;

    private Canvas canvas;
    private RectTransform originalParent;
    private Vector2 originalPosition;

    private void Awake() {
        synthesisManager = FindObjectOfType<SynthesisManager>();
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas cn in canvases) {
            if (cn.name.Equals("GameCanvas")) {
                canvas = cn;
            }
        }      
    }

    public void OnPointerClick(PointerEventData eventData) {
        invenSlotManager.SetSelectInvenIndex(key);
        invenSlotManager.SelectSlot();
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
        int synthesisSlotNum = checkSynthesisSlot(eventData);

        wordText.transform.SetParent(originalParent, true);
        wordText.rectTransform.anchoredPosition = originalPosition;

        if (synthesisSlotNum != -1) {
            //�ռ�â���� �̵�
            // ! ���� �κ� �������־ ���� ���� ���� Ȥ��, �ܾ� �ռ�â�� ������ ���� �κ����� �ٲٱ�
            //�ܾ� ������ �ش� �������� �ܾ� �̵�. ���� �κ� �����ʹ� null�� �ʱ�ȭ
            if (synthesisManager.GetExistFromIndex(synthesisSlotNum)) {
                //�ռ�â�� �ܾ� �̹� ������ �ش� �ܾ�� �κ� ����Ī
                invenSlotManager.SwitchingInvenToSynthesisSlot(key, synthesisSlotNum);
            }
            else {
                invenSlotManager.SetWordAdd(key, synthesisSlotNum);
            }
        }
        else {
            if (checkAnotherSlot(eventData) != -1) {
                //�κ� ���� ����Ī

            }
            else {
                //���� ����â

            }
        }
    }

    private int checkSynthesisSlot(PointerEventData eventData) {
        if (RectTransformUtility.RectangleContainsScreenPoint(synthesisManager.GetSlotRectTransform(0), eventData.position, eventData.pressEventCamera)) {
            return 0;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(synthesisManager.GetSlotRectTransform(1), eventData.position, eventData.pressEventCamera)) {
            return 1;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(synthesisManager.GetSlotRectTransform(2), eventData.position, eventData.pressEventCamera)) {
            return 2;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(synthesisManager.GetSlotRectTransform(3), eventData.position, eventData.pressEventCamera)) {
            return 3;
        }
        else {
            return -1;
        }
    }

    private int checkAnotherSlot(PointerEventData eventData) {
        int slotIndex = -1;
        for (int i = 0; i < 21; i++) {
            if (RectTransformUtility.RectangleContainsScreenPoint(synthesisManager.GetSlotRectTransform(i), eventData.position, eventData.pressEventCamera)) {
                slotIndex = i;
                break;
            }
        }
        return slotIndex;
    }
}
