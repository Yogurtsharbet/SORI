using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [UI] 조합 - 인벤토리 매니저
public class HalfInvenManager : CommonInvenSlotManager {
    private HalfInvenSlotController[] halfInvenSlot;

    private bool isCombineMode = false;     //문장 조합모드
    public bool IsCombineMode => isCombineMode;

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
            halfInvenSlot[index].SetKey(index);
            index++;
        }

        playerInvenController.InvenChanged += updateSlot;
    }

    public void SetCombineMode(bool yn) {
        isCombineMode = yn;
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
