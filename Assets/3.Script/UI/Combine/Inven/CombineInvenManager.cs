using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombineInvenManager : CommonInvenSlotManager {
    private CombineInvenController[] combineInvenController;

    private void Awake() {
        Button[] slotButtons = GetComponentsInChildren<Button>();
        combineInvenController = new CombineInvenController[slotButtons.Length];
        invenSelectControllers = new InvenSlotSelectController[slotButtons.Length];
        playerInvenController = FindObjectOfType<PlayerInvenController>();
        slotObjects = new GameObject[slotButtons.Length];

        int index = 0;
        foreach (Button btn in slotButtons) {
            slotObjects[index] = btn.gameObject;
            combineInvenController[index] = slotObjects[index].GetComponentInChildren<CombineInvenController>();
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
            combineInvenController[i].CloseSlot();
        }
        for (int i = 0; i < playerInvenController.InvenOpenCount; i++) {
            combineInvenController[i].OpenSlot();
        }
    }

    private void OnDestroy() {
        playerInvenController.InvenChanged -= updateSlot;
    }

    private void updateSlot(List<Word> inventory) {
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i] != null) {
                combineInvenController[i].SetSlotWord(inventory[i]);
            }
            else {
                combineInvenController[i].SetSlotWord(null);
            }
        }
    }

    public RectTransform GetSlotRectTransform(int num) {
        return combineInvenController[num].GetComponent<RectTransform>();
    }
}
