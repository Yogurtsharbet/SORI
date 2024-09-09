using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //합성창 index에 있는 단어와 인벤 단어 스위칭
    public void SwitchingInvenToSynthesisSlot(int invenIndex, int index) {
        Word tempWord = playerInvenController.GetWordIndex(invenIndex);
        playerInvenController.RemoveItemIndex(invenIndex);
        playerInvenController.AddItem(synthesisManager.GetSlotWordFromIndex(index), invenIndex);
        synthesisManager.SlotItemChangeFromIndex(index, tempWord, invenIndex);
    }

    //단어 아이템 소모 하기전 행동 취소로 인벤 초기화
    public void TempInvenToPlayerInven() {
        playerInvenController.SetInvenReset(TempInven);
    }

    //선택한 index슬롯에 단어 추가
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
