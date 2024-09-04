using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlotCloseController : MonoBehaviour {
    private Image[] closeImages;

    private void Awake() {
        closeImages = GetComponentsInChildren<Image>();
    }
    
    public void CloseEnable() {
        closeImages[0].enabled = true;
        closeImages[1].enabled = true;
    }

    public void OpenDisEnable() {
        closeImages[0].enabled = false;
        closeImages[1].enabled = false;
    }
}
