using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SentencesManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    private ActiveSentenceController activeSentenceController;

    private void Awake() {
        activeSentenceController = FindObjectOfType<ActiveSentenceController>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        activeSentenceController.OnTargetPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData) {
        activeSentenceController.OnTargetPointerExit();
    }
}
