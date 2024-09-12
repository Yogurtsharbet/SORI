using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [UI] ���� - ���� ���� ��Ʈ�ѷ�. ���� ����Ʈ �� �� ���� Ʋ
public class SentenceSlotController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private CombineContainer combineFieldController;
    private RectTransform combineFieldRectTransform;    //���� ���� ����

    private Canvas canvas;
    private RectTransform originalParent;
    private Vector2 originalPosition;
    private RectTransform rectTransform;            //�ش� gameobject transform

    private int key;
    public Frame thisFrame { get; private set; }                  //���� ���� ����

    private void Awake() {
        combineFieldController = FindObjectOfType<CombineContainer>();
        Image img = combineFieldController.GetComponentInChildren<Image>();
        combineFieldRectTransform = img.GetComponent<RectTransform>();

        //TODO: ���Ӿ����� �и���  CANVAS �и� �ʿ�
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas cn in canvases) {
            if (cn.name.Equals("GameCanvas")) {
                canvas = cn;
            }
        }
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void OpenSlot() {
        gameObject.SetActive(true);
    }

    public void CloseSlot() {
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) {
        combineFieldController.OpenCombineField();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        originalParent = rectTransform.parent as RectTransform;
        originalPosition = rectTransform.anchoredPosition;
        gameObject.transform.SetParent(canvas.transform, true);
        gameObject.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out position);
        rectTransform.anchoredPosition = position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        gameObject.transform.SetParent(originalParent, true);
        rectTransform.anchoredPosition = originalPosition;

        if (RectTransformUtility.RectangleContainsScreenPoint(combineFieldRectTransform, eventData.position, eventData.pressEventCamera)) {
            combineFieldController.OpenCombineSlot(key, thisFrame);
        }
    }

    public void SetSlotSentence(Frame frame) {
        thisFrame = frame;
    }
}
