using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTargetController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private TooltipController tooltipController;

    private void Awake() {
        tooltipController = FindObjectOfType<TooltipController>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        tooltipController.OpenTooltip(eventData);
    }

    public void OnPointerExit(PointerEventData eventData) {
        tooltipController.CloseTooltip();
    }
}
