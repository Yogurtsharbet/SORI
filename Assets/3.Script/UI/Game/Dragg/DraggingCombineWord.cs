using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggingCombineWord : DraggingObject, IEndDragHandler, IDragHandler, IBeginDragHandler {
    protected Text wordText;

    private HalfInvenSlotController halfInvenSlot;
    private Vector3 originalScale;
    private WordDragCombineTarget previousTarget = null;


    private void Awake() {
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas cn in canvases) {
            if (cn.name.Equals("GameCanvas")) {
                canvas = cn;
            }
        }
        rectTransform = gameObject.GetComponent<RectTransform>();
        halfInvenSlot = gameObject.GetComponent<HalfInvenSlotController>();
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

        PointerEventData pointerData = new PointerEventData(EventSystem.current) {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        List<WordDragCombineTarget> tempList = new List<WordDragCombineTarget>();

        foreach (var result in results) {
            if (result.gameObject.TryGetComponent(out WordDragCombineTarget target)) {
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
        wordText.transform.SetParent(originalParent, true);
        wordText.rectTransform.anchoredPosition = originalPosition;

        PointerEventData pointerData = new PointerEventData(EventSystem.current) {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        List<WordDragCombineTarget> tempList = new List<WordDragCombineTarget>();

        foreach (var result in results) {
            if (result.gameObject.TryGetComponent(out WordDragCombineTarget target)) {
                tempList.Add(target);
            }
        }

        if (tempList.Count == 1) {
            if (tempList[0].IsFrameActive()) {
                string contents = "이미 조합된 문장틀입니다.\n새 문장을 만드시려면 새 문장틀을 선택해주세요.";
                DialogManager.Instance.OpenDefaultDialog(contents, DialogType.FAIL);
                tempList[0].ChangeLocale(originalScale);
                return;
            }

            if (!tempList[0].IsSlotExistFrame()) {
                hitObject(tempList[0]);
            }
            else {
                tempList[0].SwitchingFrameToWord(halfInvenSlot.ThisWord);
                halfInvenSlot.DeleteWord();
            }
            tempList[0].ChangeLocale(originalScale);
        }
        else if (tempList.Count == 2) {
            hitObject(tempList[0], tempList[1]);
            tempList[0].ChangeLocale(originalScale);
        }
        else {
            //TODO: 다른 SLOT일때 스위칭 혹은 원래대로

            tempList[0].ChangeLocale(originalScale);
        }
    }

    //count가 1개 - baseframe의 word, 2개 - subFrame의 word
    private void hitObject(WordDragCombineTarget target) {
        Word slotWord = target.OpenCombineWord(halfInvenSlot.ThisWord);
        if (slotWord != null) {
            halfInvenSlot.AddWord(slotWord);
        }
        halfInvenSlot.DeleteWord();
    }

    private void hitObject(WordDragCombineTarget target, WordDragCombineTarget targetParent) {
        Word slotWord = target.OpenCombineWord(halfInvenSlot.ThisWord, targetParent);
        if (slotWord != null) {
            halfInvenSlot.AddWord(slotWord);
        }
        halfInvenSlot.DeleteWord();
    }

}
