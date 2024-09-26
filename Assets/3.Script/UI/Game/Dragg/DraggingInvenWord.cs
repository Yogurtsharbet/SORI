using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggingInvenWord : DraggingObject, IEndDragHandler, IDragHandler, IBeginDragHandler {
    protected Text wordText;
    private InvenSlotManager invenSlotManager;
    private InvenSlotController invenSlotController;

    private void Awake() {
        invenSlotManager = FindObjectOfType<InvenSlotManager>();
        invenSlotController = gameObject.GetComponent<InvenSlotController>();

        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas cn in canvases) {
            if (cn.name.Equals("GameCanvas")) {
                canvas = cn;
            }
        }
        rectTransform = gameObject.GetComponent<RectTransform>();
        wordText = gameObject.GetComponentInChildren<Text>();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        originalParent = wordText.rectTransform.parent as RectTransform;
        originalPosition = wordText.rectTransform.anchoredPosition;
        wordText.transform.SetParent(canvas.transform, true);
        wordText.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPosition);
        wordText.rectTransform.anchoredPosition = localPosition;

    }

    public void OnEndDrag(PointerEventData eventData) {
        wordText.transform.SetParent(originalParent, true);
        wordText.rectTransform.anchoredPosition = originalPosition;

        PointerEventData pointerData = new PointerEventData(EventSystem.current) {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        List<WordDragInvenTarget> tempList = new List<WordDragInvenTarget>();

        foreach (var result in results) {
            if (result.gameObject.TryGetComponent(out WordDragInvenTarget target)) {
                tempList.Add(target);
            }
        }

        if (tempList.Count > 0) {
            addSynthesisSlot(tempList[0]);
        }
    }

    private void addSynthesisSlot(WordDragInvenTarget target) {
        if (target.IsInvenSlot) {
            //인벤 슬롯일때
            invenSlotManager.SetInvenSwitching(invenSlotController.Key, target.InvenSlotKey());
        }
        else {
            //조합창 슬롯일때
            target.AddWord(invenSlotController.Key);
        }
    }

}
