using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggingWord : DraggingObject, IEndDragHandler, IDragHandler {
    private HalfInvenSlotController halfInvenSlot;

    private RectTransform dropZone; // 드롭 영역
    private Vector3 zoomScale = new Vector3(2f, 2f, 2f);
    private Vector3 originalScale;

    private void Awake() {
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas cn in canvases) {
            if (cn.name.Equals("GameCanvas")) {
                canvas = cn;
            }
        }
        rectTransform = gameObject.GetComponent<RectTransform>();

        //dropZone = FindObjectOfType<CombineManager>().gameObject.GetComponent<RectTransform>();
        //originalScale = dropZone.localScale;
        halfInvenSlot = gameObject.GetComponent<HalfInvenSlotController>();
    }
    public void OnDrag(PointerEventData eventData) {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out position);
        rectTransform.anchoredPosition = position;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        //TODO: 드래그 할때 서브 frame일때 확대 처리
        // 드롭 영역 위에 올라갔는지 확인
        //if (RectTransformUtility.RectangleContainsScreenPoint(dropZone, Input.mousePosition, canvas.worldCamera)) {
        //    dropZone.localScale = zoomScale;  // 확대 적용
        //}
        //else {
        //    dropZone.localScale = originalScale;  // 원래 크기 유지
        //}
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

        if (tempList.Count == 1) {
            hitObject(tempList[0]);
        }
        else if (tempList.Count == 2) {
            hitObject(tempList[0],tempList[1]);
        }
    }

    //count가 1개 - baseframe의 word, 2개 - subFrame의 word
    private void hitObject(WordDragTarget target) {
            target.OpenCombineWord(halfInvenSlot.ThisWord);
    }

    private void hitObject(WordDragTarget target, WordDragTarget targetParent) {
        target.OpenCombineWord(halfInvenSlot.ThisWord, targetParent);
    }
}
