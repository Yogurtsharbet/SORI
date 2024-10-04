using DG.Tweening;
using UnityEngine;

public class CardAppearEffect : MonoBehaviour {
    public float animationDuration = 2f;

    private Vector3 targetPosition;
    private CanvasGroup canvasGroup;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();   
    }

    public void SetPostion(Vector3 pos) {
        transform.localPosition = pos;
        canvasGroup.alpha = 0f;
        targetPosition = new Vector3(pos.x, pos.y + 70f, pos.z);
    }

    public void ShowCard() {
        if (canvasGroup == null)
            return;

        Sequence cardSequence = DOTween.Sequence();
        cardSequence.Append(canvasGroup.DOFade(1f, animationDuration));
        cardSequence.Join(transform.DOLocalMoveY(targetPosition.y, animationDuration));
        cardSequence.Play();
    }
}
