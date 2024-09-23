using UnityEngine;
using UnityEngine.EventSystems;

// [UI] 문장 목록 - 문장목록 컨테이너 마우스  hover 이벤트
public class FrameListContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private ActiveStatusController activeSentenceController;

    private void Awake() {
        activeSentenceController = FindObjectOfType<ActiveStatusController>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        activeSentenceController.OnTargetPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData) {
        activeSentenceController.OnTargetPointerExit();
    }
}
