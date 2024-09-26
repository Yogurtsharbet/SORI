using System.Collections.Generic;
using UnityEngine;

// [UI] 인벤토리 - 인벤 슬롯 목록 매니저
public class InvenSlotManager : CommonInvenSlotManager {
    private SynthesisManager synthesisManager;
    private InvenSlotController[] invenSlotControllers;

    private void Awake() {
        synthesisManager = FindObjectOfType<SynthesisManager>();
        invenSlotControllers = new InvenSlotController[20];
        invenSelectControllers = new InvenSlotSelectController[20];
        playerInvenController = FindObjectOfType<PlayerInvenController>();

        for (int i = 0; i < 20; i++) {
            Vector3 position;
            if (i < 4) {
                position = new Vector3(1090f + (i * 155f), 785f, 0);
            }
            else if (i < 8) {
                position = new Vector3(1090f + (i - 4) * 155f, 650f, 0);
            }
            else if (i < 12) {
                position = new Vector3(1090f + (i - 8) * 155f, 515f, 0);
            }
            else if (i < 16) {
                position = new Vector3(1090f + (i - 12) * 155f, 380f, 0);
            }
            else {
                position = new Vector3(1090f + (i - 16) * 155f, 250f, 0);
            }
            GameObject newSlotObject = Instantiate(invenSlotPrefab, position, Quaternion.identity, gameObject.transform);
            newSlotObject.name = $"slot{i}";
            slotList.Add(newSlotObject);
            invenSlotControllers[i] = newSlotObject.GetComponentInChildren<InvenSlotController>();
            invenSelectControllers[i] = newSlotObject.GetComponentInChildren<InvenSlotSelectController>();
            invenSlotControllers[i].SetKey(i);
        }

        playerInvenController.InvenChanged += updateSlot;

        for (int i = 0; i < 20; i++) {
            invenSlotControllers[i].CloseSlot();
        }
    }

    private void Update() {
        //삭제 confirm 체크
        if (waitConfirm) {
            if (DialogManager.Instance.GetIsConfirmed() == 1) {
                RemoveWord();
                waitConfirm = false;
            }
            else if (DialogManager.Instance.GetIsConfirmed() == 0) {
                DialogManager.Instance.ResetIsConfirm();
                waitConfirm = false;
            }
        }
    }

    private void OnDestroy() {
        playerInvenController.InvenChanged -= updateSlot;
    }

    private void updateSlot(List<Word> inventory) {
        for (int i = 0; i < playerInvenController.InvenOpenCount; i++) {
            invenSlotControllers[i].OpenSlot();
        }

        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i] != null) {
                invenSlotControllers[i].SetSlotWord(inventory[i]);
            }
            else {
                invenSlotControllers[i].SetSlotWord(null);
            }
        }
    }
}
