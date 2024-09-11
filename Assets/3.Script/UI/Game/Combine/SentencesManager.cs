using UnityEngine;
using UnityEngine.EventSystems;

// [UI] 문장 목록 - 문장 목록 매니저
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
        sentenceSlotControllers[0].OpenSlot();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        activeSentenceController.OnTargetPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData) {
        activeSentenceController.OnTargetPointerExit();
    }

    public void SetSlotSentence(int index, Sentence sentence) {
        sentenceSlotControllers[index].SetSlotSentence(sentence);
    }
}
