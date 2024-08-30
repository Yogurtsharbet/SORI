using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlotManager : MonoBehaviour {
    GameObject[] slotObjects = new GameObject[21];
    PlayerInvenController playerInvenController;

    private void Awake() {
        playerInvenController = FindObjectOfType<PlayerInvenController>();
        Image[] slotImages = GetComponentsInChildren<Image>();

        int index = 0;
        foreach (Image images in slotImages) {
            if (!images.name.Equals("SlotCloseIcon")) {
                slotObjects[index] = images.gameObject;
            }
            index++;
        }
    }

    private void Start() {
        playerInvenController.InvenChanged += updateSlot;
    }

    private void OnDestroy() {
        playerInvenController.InvenChanged -= updateSlot;
    }

    private void updateSlot(List<Word> inventory) {
        for(int i = 0; i < 21; i++) {
            slotObjects[i].GetComponent<InvenSlotController>().CloseSlot();
        }

        for(int i = 0; i < inventory.Count; i++) {
            slotObjects[i].GetComponent<InvenSlotController>().OpenSlot();
        }
    }

}
