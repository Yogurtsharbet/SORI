using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelectEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler {
    private WordCardSelectContainer wordCardSelectContainer;

    private float verticalMoveAmount = 30f;
    private float moveTime = 0.1f;
    private float scaleAmount = 1.1f;

    private Vector3 startPos;
    private Vector3 startScale;

    public int key = -1;

    private void Awake() {
        wordCardSelectContainer = FindObjectOfType<WordCardSelectContainer>();
    }

    public void SetSelectInit(Vector3 pos) {
        startPos = pos;
        startScale = transform.localScale;
    }

    private IEnumerator MoveCardCo(Vector3 targetPos, Vector3 targetScale) {
        float elapsedTime = 0f;
        Vector3 initialPos = transform.localPosition;
        Vector3 initialScale = transform.localScale;

        while (elapsedTime < moveTime) {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveTime;

            transform.localPosition = Vector3.Lerp(initialPos, targetPos, t);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            yield return null;
        }

        transform.localPosition = targetPos;
        transform.localScale = targetScale;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        eventData.selectedObject = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData) {
        eventData.selectedObject = null;
    }

    public void OnSelect(BaseEventData eventData) {
        Vector3 endPos = startPos + new Vector3(0f, verticalMoveAmount, 0f);
        Vector3 endScale = startScale * scaleAmount;
        StartCoroutine(MoveCardCo(endPos, endScale));
        wordCardSelectContainer.LastSelected = gameObject;
        wordCardSelectContainer.LastSelectedIndex = key;
    }

    public void OnDeselect(BaseEventData eventData) {
        StartCoroutine(MoveCardCo(startPos, startScale));
    }

    public void ResetCard() {
        gameObject.transform.localScale = startScale;
        gameObject.transform.localPosition = startPos;
    }
}
