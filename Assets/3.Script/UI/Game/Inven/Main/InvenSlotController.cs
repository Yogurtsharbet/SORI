using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [UI] �κ��丮 - �κ��丮 ���� ��Ʈ�ѷ�
public class InvenSlotController : CommonInvenSlotController, IEndDragHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler {
    private SynthesisManager synthesisManager;
    private InvenSlotManager invenSlotManager;

    private void Awake() {
        synthesisManager = FindObjectOfType<SynthesisManager>();
        //TODO: ���Ӿ����� �и���  CANVAS �и� �ʿ�
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas cn in canvases) {
            if (cn.name.Equals("GameCanvas")) {
                canvas = cn;
            }
        }

        closeController = GetComponentInChildren<InvenSlotCloseController>();
        invenSlotManager = FindObjectOfType<InvenSlotManager>();

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

    public void OnPointerClick(PointerEventData eventData) {
        invenSlotManager.SelectSlot(key);
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

        int synthesisSlotNum = checkSynthesisSlot(eventData);
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
            int invenSlotNum = checkAnotherSlot(eventData);
            if (invenSlotNum != -1) {
                //�κ� ���� ����Ī
                invenSlotManager.SetInvenSwitching(key, invenSlotNum);
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
            if (RectTransformUtility.RectangleContainsScreenPoint(invenSlotManager.GetSlotRectTransform(i), eventData.position, eventData.pressEventCamera)) {
                slotIndex = i;
                break;
            }
        }
        return slotIndex;
    }
}
