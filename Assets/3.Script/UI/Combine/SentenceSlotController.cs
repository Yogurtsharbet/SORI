using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SentenceSlotController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private CombineFieldController combineFieldController;

    private Canvas canvas;
    private RectTransform originalParent;
    private Vector2 originalPosition;

    RectTransform rectTransform;

    private void Awake() {
        combineFieldController = FindObjectOfType<CombineFieldController>();
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

    }
}
