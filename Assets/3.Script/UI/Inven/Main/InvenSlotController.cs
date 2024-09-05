using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlotController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private InvenSlotCloseController closeController;
    private SynthesisManager synthesisManager;
    private InvenSlotManager invenSlotManager;

    private int key;

    private Text wordText;
    private Image typeIcon;
    private Image rankOutIcon;
    private Image rankInnerIcon;
    private Image continueIcon;

    private Word thisWord;

    private Canvas canvas;
    private RectTransform originalParent;
    private Vector2 originalPosition;

    private void Awake() {
        closeController = GetComponentInChildren<InvenSlotCloseController>();
        synthesisManager = FindObjectOfType<SynthesisManager>();
        invenSlotManager = FindObjectOfType<InvenSlotManager>();
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach(Canvas cn in canvases) {
            if (cn.name.Equals("GameCanvas")) {
                canvas = cn;
            }
        }

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
            }else if (img.name.Equals("Continue")) {
                continueIcon = img;
            }
        }
    }

    public void SetKey(int num) {
        this.key = num;
    }

    public void CheckWordExist() {
        if (thisWord != null) {
            ExistWord();
            SetWordData(thisWord);
        }
        else {
            NotExistWord();
        }
    }

    public void ExistWord() {
        wordText.enabled = true;
        typeIcon.enabled = true;
        rankInnerIcon.enabled = true;
        rankOutIcon.enabled = true;
        continueIcon.enabled = true;
    }

    public void NotExistWord() {
        wordText.enabled = false;
        typeIcon.enabled = false;
        rankInnerIcon.enabled = false;
        rankOutIcon.enabled = false;
        continueIcon.enabled = false;
    }

    public void SetSlotWord(Word word) {
        thisWord = word;
        CheckWordExist();
    }

    public void SetWordData(Word word) {
        wordText.text = word.Name;
        typeIcon.color = word.TypeColor;
        rankInnerIcon.color = word.RankColor;
        //TODO: �ܾ �����Ӽ� ������ continue icon enable
    }

    public void CloseSlot() {
        closeController.CloseEnable();
        CheckWordExist();
    }

    public void OpenSlot() {
        closeController.OpenDisEnable();
        CheckWordExist();
    }

    #region ���콺 event
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
        int synthesisSlotNum  = checkSynthesisSlot(eventData);

        wordText.transform.SetParent(originalParent, true);
        wordText.rectTransform.anchoredPosition = originalPosition;

        invenSlotManager.SetInvenSaveTemp();

        if (synthesisSlotNum != -1) {
            //�ռ�â���� �̵�
            // ! ���� �κ� �������־ ���� ���� ���� Ȥ��, �ܾ� �ռ�â�� ������ ���� �κ����� �ٲٱ�
            //�ܾ� ������ �ش� �������� �ܾ� �̵�. ���� �κ� �����ʹ� null�� �ʱ�ȭ
            if (synthesisManager.GetExistFromIndex(synthesisSlotNum)) {
                //�ռ�â�� �ܾ� �̹� ������ �ش� �ܾ�� �κ� ����Ī
                invenSlotManager.ChangeInvenToSynthesisSlot(synthesisSlotNum);
            }
            else {
                invenSlotManager.SetWordAdd(synthesisSlotNum);
            }
        }
        else {
            if(checkAnotherSlot(eventData) != -1) {
                //�κ� ���� ����Ī

            }
            else {
                //���� ����â

            }
        }
    }

    private int checkSynthesisSlot(PointerEventData eventData) {
        if(RectTransformUtility.RectangleContainsScreenPoint(synthesisManager.GetSlotRectTransform(0), eventData.position, eventData.pressEventCamera)) {
            return 0;
        }else if(RectTransformUtility.RectangleContainsScreenPoint(synthesisManager.GetSlotRectTransform(1), eventData.position, eventData.pressEventCamera)) {
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
        for( int i = 0; i < 21; i++) {
            if(RectTransformUtility.RectangleContainsScreenPoint(synthesisManager.GetSlotRectTransform(i), eventData.position, eventData.pressEventCamera)) {
                slotIndex = i;
                break;
            }
        }
        return slotIndex;
    }
    #endregion
}
