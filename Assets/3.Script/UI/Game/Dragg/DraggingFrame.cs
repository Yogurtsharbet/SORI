using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DraggingFrame : DraggingObject, IEndDragHandler, IDragHandler, IBeginDragHandler {
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

        if (tempList.Count > 0) {
            hitObject(tempList[0], tempList.Count);
        }
    }

    private void hitObject(FrameDragTarget target, int num) {
        if (num == 1) {
            //base frame
            target.OpenCombineFiled(frameListSlotController.thisFrame);
            frameListSlotController.FrameSlotDelete();
        }
        else if (num == 2) {
            if (target.IsFrameActive()) {
                string contents = "이미 조합된 문장틀입니다.\n새 문장을 만드시려면 새 문장틀을 선택해주세요.";
                DialogManager.Instance.OpenDefaultDialog(contents, DialogType.FAIL);
                return;
            }

            bool isAddFrameToSlot = target.OpenCombineSlot(frameListSlotController.key, frameListSlotController.thisFrame);
            if (isAddFrameToSlot) {
                frameListSlotController.FrameSlotDelete();
            }
            else {
                string contents = "해당 위치에는 넣을 수 없습니다.\n다른곳에 추가해주세요.";
                DialogManager.Instance.OpenDefaultDialog(contents, DialogType.FAIL);
            }
        }
        else {
            string contents = "해당 위치에는 넣을 수 없습니다.\n다른곳에 추가해주세요.";
            DialogManager.Instance.OpenDefaultDialog(contents, DialogType.FAIL);
        }
    }
}
