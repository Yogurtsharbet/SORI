using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [UI] �κ��丮 - �κ� ���� ��� �Ŵ���
public class InvenSlotManager : CommonInvenSlotManager {
    private SynthesisManager synthesisManager;
    private InvenSlotController[] invenSlotControllers;

    private void Awake() {
        synthesisManager = FindObjectOfType<SynthesisManager>();
        Button[] slotButtons = GetComponentsInChildren<Button>();
        invenSlotControllers = new InvenSlotController[slotButtons.Length];
        invenSelectControllers = new InvenSlotSelectController[slotButtons.Length];
        playerInvenController = FindObjectOfType<PlayerInvenController>();
        slotObjects = new GameObject[slotButtons.Length];

        int index = 0;
        foreach (Button btn in slotButtons) {
            slotObjects[index] = btn.gameObject;
            invenSlotControllers[index] = slotObjects[index].GetComponent<InvenSlotController>();
            invenSelectControllers[index] = slotObjects[index].GetComponentInChildren<InvenSlotSelectController>();
            invenSlotControllers[index].SetKey(index);
            index++;
        }

        playerInvenController.InvenChanged += updateSlot;
    }

    private void OnEnable() {
        SetInvenSaveTemp();
    }

    private void Start() {
        for (int i = 0; i < 20; i++) {
            invenSlotControllers[i].CloseSlot();
        }
        for (int i = 0; i < playerInvenController.InvenOpenCount; i++) {
            invenSlotControllers[i].OpenSlot();
        }
    }

    private void OnDestroy() {
        playerInvenController.InvenChanged -= updateSlot;
    }

    //�ռ�â index�� �ִ� �ܾ�� �κ� �ܾ� ����Ī
    public void SwitchingInvenToSynthesisSlot(int invenIndex, int index) {
        Word tempWord = playerInvenController.GetWordIndex(invenIndex);
        playerInvenController.RemoveItemIndex(invenIndex);
        playerInvenController.AddItem(synthesisManager.GetSlotWordFromIndex(index), invenIndex);
        synthesisManager.SlotItemChangeFromIndex(index, tempWord, invenIndex);
    }

    //�ܾ� ������ �Ҹ� �ϱ��� �ൿ ��ҷ� �κ� �ʱ�ȭ
    //public void TempInvenToPlayerInven() {
    //    List<Word> playerInven = playerInvenController.Inven;
    //    int playerInvenCount = 0;
    //    int tempInvenCount = 0;
    //    for (int i = 0; i < 20; i ++) {
    //        if(playerInven[i] != null) {
    //            playerInvenCount++;
    //        }
    //    }
    //    for(int i = 0; i <20; i++) {
    //        if(tempInven[i] != null) {
    //            tempInvenCount++;
    //        }
    //    }

    //    if(tempInvenCount != playerInvenCount) {
    //        playerInvenController.SetInvenReset(TempInven);
    //    }
    //}

    //������ index���Կ� �ܾ� �߰�
    public void SetWordAdd(int invenIndex, int index) {
        synthesisManager.SlotItemChangeFromIndex(index, playerInvenController.GetWordIndex(invenIndex), invenIndex);
        playerInvenController.RemoveItemIndex(invenIndex);
    }

    private void updateSlot(List<Word> inventory) {
        for (int i = 0; i < inventory.Count; i++) {
            if (inventory[i] != null) {
                invenSlotControllers[i].SetSlotWord(inventory[i]);
            }
            else {
                invenSlotControllers[i].SetSlotWord(null);
            }
        }
    }

    public RectTransform GetSlotRectTransform(int num) {
        return invenSlotControllers[num].GetComponent<RectTransform>();
    }
}
