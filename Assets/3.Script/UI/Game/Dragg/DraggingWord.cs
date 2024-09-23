using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggingWord : DraggingObject, IEndDragHandler, IDragHandler {
    private HalfInvenSlotController halfInvenSlot;
    private CombineManager combineManager;
    private Vector3 originalScale;
    private WordDragTarget previousTarget = null;

    private void Awake() {
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas cn in canvases) {
            if (cn.name.Equals("GameCanvas")) {
                canvas = cn;
            }
        }
        rectTransform = gameObject.GetComponent<RectTransform>();
        combineManager = FindObjectOfType<CombineManager>();
        halfInvenSlot = gameObject.GetComponent<HalfInvenSlotController>();
    }
    public void OnDrag(PointerEventData eventData) {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out position);
        rectTransform.anchoredPosition = position;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        PointerEventData pointerData = new PointerEventData(EventSystem.current) {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        List<WordDragTarget> tempList = new List<WordDragTarget>();

        foreach (var result in results) {
            if (result.gameObject.TryGetComponent(out WordDragTarget target)) {
                tempList.Add(target);
            }
        }

        if (tempList.Count > 0 && !tempList[0].IsSlotExistFrame()) {
            var currentTarget = tempList[0];

            if (previousTarget != currentTarget) {
                if (previousTarget != null) {
                    previousTarget.ChangeLocale(originalScale);
                }
                originalScale = currentTarget.ThisRectTransform.localScale;
                if (tempList.Count > 1) {
                    currentTarget.ChangeLocale(new Vector3(4f, 4f, 4f));
                }
                else {
                    currentTarget.ChangeLocale(new Vector3(2f, 2f, 2f));
                }
                previousTarget = currentTarget;
            }
        }
        else {
            if (previousTarget != null) {
                previousTarget.ChangeLocale(originalScale);
                previousTarget = null;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        gameObject.transform.SetParent(originalParent, true);
        rectTransform.anchoredPosition = originalPosition;

        PointerEventData pointerData = new PointerEventData(EventSystem.current) {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        List<WordDragTarget> tempList = new List<WordDragTarget>();

        foreach (var result in results) {
            if (result.gameObject.TryGetComponent(out WordDragTarget target)) {
                tempList.Add(target);
            }
        }

        if (!tempList[0].IsSlotExistFrame()) {
            if (tempList.Count == 1) {
                hitObject(tempList[0]);
            }
            else if (tempList.Count == 2) {
                hitObject(tempList[0], tempList[1]);
            }
        }
        else {
            tempList[0].SwitchingFrameToWord(halfInvenSlot.ThisWord);
            halfInvenSlot.DeleteWord();
        }

        tempList[0].ChangeLocale(originalScale);
    }

    //count가 1개 - baseframe의 word, 2개 - subFrame의 word
    private void hitObject(WordDragTarget target) {
        Word slotWord = target.OpenCombineWord(halfInvenSlot.ThisWord);
        if (slotWord != null) {
            halfInvenSlot.AddWord(slotWord);
        }
        halfInvenSlot.DeleteWord();
    }

    private void hitObject(WordDragTarget target, WordDragTarget targetParent) {
        Word slotWord = target.OpenCombineWord(halfInvenSlot.ThisWord, targetParent);
        if (slotWord != null) {
            halfInvenSlot.AddWord(slotWord);
        }
        halfInvenSlot.DeleteWord();
    }

}
