using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DraggingObject : MonoBehaviour, IBeginDragHandler {
    protected Canvas canvas;
    protected RectTransform originalParent;
    protected Vector2 originalPosition;
    protected RectTransform rectTransform;            //해당 gameobject transform

    private void Awake() {
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas cn in canvases) {
            if (cn.name.Equals("GameCanvas")) {
                canvas = cn;
            }
        }
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        originalParent = rectTransform.parent as RectTransform;
        originalPosition = rectTransform.anchoredPosition;
        gameObject.transform.SetParent(canvas.transform, true);
        gameObject.transform.SetAsLastSibling();
    }
}
