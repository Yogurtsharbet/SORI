using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [UI] 조합 - 문장 슬롯 컨트롤러. 문장 리스트 중 한 문장 틀
public class FrameSlotController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private CombineManager combineManager;
    private CombineContainer combineContainer;
    private RectTransform combineFieldRectTransform;    //문장 슬롯 영역

    private Canvas canvas;
    private RectTransform originalParent;
    private Vector2 originalPosition;
    private RectTransform rectTransform;            //해당 gameobject transform

    //TODO: 프레임 타입별로 끄고 키기
    private GameObject[] frameByType = new GameObject[4];

    private int key;
    public Frame thisFrame { get; private set; }        //조합 문장 정보
    private FrameType thisFrameType;                        //현재 프레임 타입

    private void Awake() {
        combineManager = FindObjectOfType<CombineManager>();
        combineContainer = FindObjectOfType<CombineContainer>();
        Image img = combineContainer.GetComponentInChildren<Image>();
        combineFieldRectTransform = img.GetComponent<RectTransform>();
        for (int i = 0; i < 4; i++) {
            frameByType[i] = gameObject.transform.GetChild(i).gameObject;
        }

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
        combineContainer.OpenCombineField();
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

        //기본 베이스 Frame Open 체크
        if (combineManager.BaseFrameOpen) {
            FrameType baseFrameType = combineManager.BaseFrame.Type;
            if(baseFrameType == FrameType.AisB || baseFrameType == FrameType.AtoBisC) {
            //1,2번 프레임 > 3,4번 프레임만 추가 가능
                if (thisFrameType != FrameType.AisB && thisFrameType != FrameType.AtoBisC) {
                    //1번, 2번 프레임의 슬롯 rectTransform 가져와서 비교

                }
            }else if(baseFrameType == FrameType.AandB) {
            //3번 프레임 > 1,2번 프레임만 추가가능
                if(thisFrameType !=FrameType.AandB && thisFrameType != FrameType.NotA) {
                    //3번 프레임 슬롯 rectTransform 가져와서 비교
                }
            }
            
        }
        else {
            if (RectTransformUtility.RectangleContainsScreenPoint(combineFieldRectTransform, eventData.position, eventData.pressEventCamera)) {
                combineManager.OpenCombineSlot(key, thisFrame);
            }
        }


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
    }

    public bool IsActive() {
        return thisFrame.IsActive;
    }
}
