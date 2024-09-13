using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [UI] ���� - ���� ���� ��Ʈ�ѷ�. ���� ����Ʈ �� �� ���� Ʋ
public class FrameSlotController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private FrameType frameType;

    private CombineContainer combineFieldController;
    private RectTransform combineFieldRectTransform;    //���� ���� ����

    private Canvas canvas;
    private RectTransform originalParent;
    private Vector2 originalPosition;
    private RectTransform rectTransform;            //�ش� gameobject transform

    //TODO: ������ Ÿ�Ժ��� ���� Ű��
    private GameObject[] frameByType;

    private int key;
    public Frame thisFrame { get; private set; }                  //���� ���� ����

    private void Awake() {
        combineFieldController = FindObjectOfType<CombineContainer>();
        Image img = combineFieldController.GetComponentInChildren<Image>();
        combineFieldRectTransform = img.GetComponent<RectTransform>();
        for (int i = 0; i < 4; i++) {
            frameByType[i] = gameObject.transform.GetChild(i).gameObject;
        }

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

    public void SetFrameData(Frame frame) {
        thisFrame = frame;
        frameType = frame.Type;
        switch (frameType) {
            case FrameType._Random:
                break;
            case FrameType.AisB:
                for (int i = 0; i < 4; i++) {
                    frameByType[i].SetActive(false);
                    if (i == 0) {
                        frameByType[i].SetActive(true);
                    }
                }
                break;
            case FrameType.AtoBisC:
                for (int i = 0; i < 4; i++) {
                    frameByType[i].SetActive(false);
                    if (i == 1) {
                        frameByType[i].SetActive(true);
                    }
                }
                break;
            case FrameType.AandB:
                for (int i = 0; i < 4; i++) {
                    frameByType[i].SetActive(false);
                    if (i == 2) {
                        frameByType[i].SetActive(true);
                    }
                }
                break;
            case FrameType.NotA:
                for (int i = 0; i < 4; i++) {
                    frameByType[i].SetActive(false);
                    if (i == 3) {
                        frameByType[i].SetActive(true);
                    }
                }
                break;
        }
    }
}
