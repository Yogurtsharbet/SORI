using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [UI] 조합 - 반쪽 인벤토리 매니저
public class HalfInvenManager : CommonInvenSlotManager {
    private HalfInvenSlotController[] halfInvenSlot;
    private HalfInvenContainer halfInvenContainer;
    private ShopTransactionManager shopTransactionManager;
    private ReceiptManager receiptManager;

    private bool isCombineMode = false;     //문장 조합모드
    public bool IsCombineMode => isCombineMode;

    private void Awake() {
        receiptManager = FindObjectOfType<ReceiptManager>();
        halfInvenContainer = FindObjectOfType<HalfInvenContainer>();
        shopTransactionManager = FindObjectOfType<ShopTransactionManager>();
        halfInvenSlot = new HalfInvenSlotController[20];
        playerInvenController = FindObjectOfType<PlayerInvenController>();

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
            halfInvenSlot[i].SetKey(i);
        }

        playerInvenController.InvenChanged += updateSlot;

        for (int i = 0; i < 20; i++) {
            halfInvenSlot[i].CloseSlot();
        }
    }

    public void SetCombineMode(bool yn) {
        isCombineMode = yn;
    }

    private void updateSlot(List<Word> inventory) {
        for (int i = 0; i < playerInvenController.InvenOpenCount; i++) {
            halfInvenSlot[i].OpenSlot();
        }

        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i] != null) {
                halfInvenSlot[i].SetSlotWord(inventory[i]);
            }
            else {
                halfInvenSlot[i].SetSlotWord(null);
            }
        }
    }
    private void OnDestroy() {
        playerInvenController.InvenChanged -= updateSlot;
    }

    public void OpenInven() {
        halfInvenContainer.OpenHalfInven();
    }

    public void CloseInven() {
        halfInvenContainer.CloseHalfInven();
    }

    /// <summary>
    /// 선택한 아이템 가격 return
    /// </summary>
    /// <returns></returns>
    public int GetSelectPrice() {
        int totalPrice = 0;
        for (int i = 0; i < selectInvens.Count; i++) {
            totalPrice += shopTransactionManager.GetWordPrice(halfInvenSlot[selectInvens[i]].ThisWord);
        }
        return totalPrice;
    }

    public void SelectItemSell() {
        for(int i = 0; i<selectInvens.Count; i++) {
            playerInvenController.RemoveItemIndex(selectInvens[i]);
        }
        ResetSelectInvens();

    }

    public void UpdateRecipt() {
        receiptManager.UpdateReciptData();
    }
}
