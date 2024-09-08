using UnityEngine;
using UnityEngine.EventSystems;

public class SentencesManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private ActiveSentenceController activeSentenceController;
    private SentenceSlotController[] sentenceSlotControllers;

    private void Awake() {
        activeSentenceController = FindObjectOfType<ActiveSentenceController>();
        sentenceSlotControllers = GetComponentsInChildren<SentenceSlotController>();
    }

    private void Start() {
        for(int i = 0; i < 10; i++) {
            sentenceSlotControllers[i].CloseSlot();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        activeSentenceController.OnTargetPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData) {
        activeSentenceController.OnTargetPointerExit();
    }
}
