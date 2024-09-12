using UnityEngine;
using UnityEngine.EventSystems;

// [UI] ���� ��� - ���� ��� �Ŵ���
public class SentencesManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private ActiveSentenceController activeSentenceController;
    private SentenceSlotController[] sentenceSlotControllers;

    private void Awake() {
        activeSentenceController = FindObjectOfType<ActiveSentenceController>();
        sentenceSlotControllers = GetComponentsInChildren<SentenceSlotController>();
    }

    private void Start() {
        //TODO: 10���� �ƴ϶� ���� ���� ���� �ʿ�
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
        //TODO: Frame �ű� ȹ��
        sentenceSlotControllers[index].SetSlotSentence(frame);
        sentenceSlotControllers[index].OpenSlot();
    }

    public Frame GetSlotSentence(int index) {
        return sentenceSlotControllers[index].thisFrame;
    }
}
