using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [UI] ���� - ���� �κ��丮 �Ŵ���
public class HalfInvenManager : CommonInvenSlotManager {
    private HalfInvenSlotController[] halfInvenSlot;
    private HalfInvenContainer halfInvenContainer;

    private bool isCombineMode = false;     //���� ���ո��
    public bool IsCombineMode => isCombineMode;

    private void Awake() {
        halfInvenContainer = FindObjectOfType<HalfInvenContainer>();
        halfInvenSlot = new HalfInvenSlotController[20];
        invenSelectControllers = new InvenSlotSelectController[20];

        for (int i = 0; i < 20; i++) {
            Vector3 position;
            if (i < 4) {
                position = new Vector3(1395f + (i * 140f), -222f, 0);
            }
            else if (i < 8) {
                position = new Vector3(1395f + (i - 4) * 140f, -342f, 0);
            }
            else if (i < 12) {
                position = new Vector3(1395f + (i - 8) * 140f, -462f, 0);
            }
            else if (i < 16) {
                position = new Vector3(1395f + (i - 12) * 140f, -582f, 0);
            }
            else {
                position = new Vector3(1395f + (i - 16) * 140f, -702f, 0);
            }
            GameObject newSlotObject = Instantiate(invenSlotPrefab, position, Quaternion.identity, gameObject.transform);
            newSlotObject.name = $"slot{i}";
            slotList.Add(newSlotObject);
            halfInvenSlot[i] = newSlotObject.GetComponentInChildren<HalfInvenSlotController>();
            invenSelectControllers[i] = newSlotObject.GetComponentInChildren<InvenSlotSelectController>();
            halfInvenSlot[i].SetKey(i);
        }

        playerInvenController = FindObjectOfType<PlayerInvenController>();
        playerInvenController.InvenChanged += updateSlot;
    }

    public void SetCombineMode(bool yn) {
        isCombineMode = yn;
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

    public void CloseInven() {
        halfInvenContainer.CloseCombineInven();
    }
}
