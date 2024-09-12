using UnityEngine;
using UnityEngine.EventSystems;

// [UI] 조합 - 문장 목록 매니저
public class SentencesManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private ActiveSentenceController activeSentenceController;
    private SentenceSlotController[] sentenceSlotControllers;

    private void Awake() {
        activeSentenceController = FindObjectOfType<ActiveSentenceController>();
        sentenceSlotControllers = GetComponentsInChildren<SentenceSlotController>();
    }

    private void Start() {
        //TODO: 10개가 아니라 동적 무한 생성 필요
        for(int i = 0; i < 10; i++) {
            sentenceSlotControllers[i].CloseSlot();
        }
        sentenceSlotControllers[0].SetSlotSentence(new Frame(FrameType.AisB));
        sentenceSlotControllers[0].OpenSlot();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        activeSentenceController.OnTargetPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData) {
        activeSentenceController.OnTargetPointerExit();
    }

    public void SetSlotSentence(int index, Frame frame) {
        //TODO: Frame 신규 획득
        sentenceSlotControllers[index].SetSlotSentence(frame);
        sentenceSlotControllers[index].OpenSlot();
    }

    public Frame GetSlotSentence(int index) {
        return sentenceSlotControllers[index].thisFrame;
    }
}
