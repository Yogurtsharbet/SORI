using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [UI] 조합 - 문장 슬롯 컨트롤러. 문장 리스트 중 한 문장 틀
public class SentenceSlotController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private CombineContainer combineFieldController;
    private RectTransform combineFieldRectTransform;    //문장 슬롯 영역

    private Canvas canvas;
    private RectTransform originalParent;
    private Vector2 originalPosition;
    private RectTransform rectTransform;            //해당 gameobject transform

    private int key;
    public Frame thisFrame { get; private set; }                  //조합 문장 정보

    private void Awake() {
        combineFieldController = FindObjectOfType<CombineContainer>();
        Image img = combineFieldController.GetComponentInChildren<Image>();
        combineFieldRectTransform = img.GetComponent<RectTransform>();

        //TODO: 게임씬으로 분리시  CANVAS 분리 필요
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
