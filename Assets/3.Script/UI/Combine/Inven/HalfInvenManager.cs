using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HalfInvenManager : CommonInvenSlotManager {
    private HalfInvenController[] halfInvenController;

    private void Awake() {
        Button[] slotButtons = GetComponentsInChildren<Button>();
        halfInvenController = new HalfInvenController[slotButtons.Length];
        invenSelectControllers = new InvenSlotSelectController[slotButtons.Length];
        playerInvenController = FindObjectOfType<PlayerInvenController>();
        slotObjects = new GameObject[slotButtons.Length];

        int index = 0;
        foreach (Button btn in slotButtons) {
            slotObjects[index] = btn.gameObject;
            halfInvenController[index] = slotObjects[index].GetComponentInChildren<HalfInvenController>();
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
            halfInvenController[i].CloseSlot();
        }
        for (int i = 0; i < playerInvenController.InvenOpenCount; i++) {
            halfInvenController[i].OpenSlot();
        }
    }

    private void OnDestroy() {
        playerInvenController.InvenChanged -= updateSlot;
    }

    private void updateSlot(List<Word> inventory) {
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i] != null) {
                halfInvenController[i].SetSlotWord(inventory[i]);
            }
            else {
                halfInvenController[i].SetSlotWord(null);
            }
        }
    }

    public RectTransform GetSlotRectTransform(int num) {
        return halfInvenController[num].GetComponent<RectTransform>();
    }
}
