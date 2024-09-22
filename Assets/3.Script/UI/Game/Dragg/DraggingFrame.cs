using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DraggingFrame : DraggingObject, IEndDragHandler, IDragHandler {
    private FrameListSlotController frameListSlotController;

    private void Awake() {
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas cn in canvases) {
            if (cn.name.Equals("GameCanvas")) {
                canvas = cn;
            }
        }
        rectTransform = gameObject.GetComponent<RectTransform>();
        frameListSlotController = gameObject.GetComponentInChildren<FrameListSlotController>();

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
        List<FrameDragTarget> tempList = new List<FrameDragTarget>();

        foreach (var result in results) {
            if (result.gameObject.TryGetComponent(out FrameDragTarget target)) {
                tempList.Add(target);
            }
        }

        hitObject(tempList[0], tempList.Count);
    }

    private void hitObject(FrameDragTarget target, int num) {
        if (num == 1) {
            target.OpenCombineFiled(frameListSlotController.key, frameListSlotController.thisFrame);
            frameListSlotController.FrameSlotDelete();
        }
        else if (num == 2) {
            bool isAddFrameToSlot = target.OpenCombineSlot(frameListSlotController.key, frameListSlotController.thisFrame);
            if (isAddFrameToSlot) {
                frameListSlotController.FrameSlotDelete();
            }
            else {
                string contents = "해당 위치에는 넣을 수 없습니다.\n다른곳에 추가해주세요.";
                DialogManager.Instance.OpenDefaultDialog(contents, DialogType.FAIL);
            }
        }
        else if (num == 3) {
            string contents = "해당 위치에는 넣을 수 없습니다.\n다른곳에 추가해주세요.";
            DialogManager.Instance.OpenDefaultDialog(contents, DialogType.FAIL);
        }
        else {
            Debug.Log("ERROR - RAYCAST DRAG TARGET EMPTY");
        }
    }
}
