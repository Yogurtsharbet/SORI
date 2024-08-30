using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlotController : MonoBehaviour {
    private bool isSlotOpen;
    private Image closeIcon;

    private void Awake() {
        Image[] images = GetComponentsInChildren<Image>();
        foreach(Image img in images) {
            if (img.name.Equals("SlotCloseIcon")) {
                closeIcon = img;
            }
        }
        CloseSlot();
    }

    //���� ����
    public void OpenSlot() {
        closeIcon.enabled = false;
        isSlotOpen = true;
    }

    //���� close
    public void CloseSlot() {
        closeIcon.enabled = true;
        isSlotOpen = false;
    }

}
