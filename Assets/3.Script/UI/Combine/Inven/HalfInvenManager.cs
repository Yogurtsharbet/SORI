using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HalfInvenManager : CommonInvenSlotManager {
    private HalfInvenSlotController[] halfInvenSlot;

    private void Awake() {
        Button[] slotButtons = GetComponentsInChildren<Button>();
        halfInvenSlot = new HalfInvenSlotController[slotButtons.Length];
        invenSelectControllers = new InvenSlotSelectController[slotButtons.Length];
        playerInvenController = FindObjectOfType<PlayerInvenController>();
        slotObjects = new GameObject[slotButtons.Length];

        int index = 0;
        foreach (Button btn in slotButtons) {
            slotObjects[index] = btn.gameObject;
            halfInvenSlot[index] = slotObjects[index].GetComponentInChildren<HalfInvenSlotController>();
            invenSelectControllers[index] = slotObjects[index].GetComponentInChildren<InvenSlotSelectController>();
            index++;
        }

        playerInvenController.InvenChanged += updateSlot;
    }

    private void OnEnable() {
        SetInvenSaveTemp();
    }

    private void Start() {
        for (int i = 0; i < 20; i++) {
            halfInvenSlot[i].CloseSlot();
        }
        for (int i = 0; i < playerInvenController.InvenOpenCount; i++) {
            halfInvenSlot[i].OpenSlot();
        }
    }

    private void OnDestroy() {
        playerInvenController.InvenChanged -= updateSlot;
    }

    private void updateSlot(List<Word> inventory) {
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i] != null) {
                halfInvenSlot[i].SetSlotWord(inventory[i]);
            }
            else {
                halfInvenSlot[i].SetSlotWord(null);
            }
        }
    }

    public RectTransform GetSlotRectTransform(int num) {
        return halfInvenSlot[num].GetComponent<RectTransform>();
    }
}
