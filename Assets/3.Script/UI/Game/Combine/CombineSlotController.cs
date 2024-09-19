using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [UI] 조합 - 실제 조합 slot
public class CombineSlotController : MonoBehaviour {
    private RectTransform rectTransform;

    private void Awake() {
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    
}
