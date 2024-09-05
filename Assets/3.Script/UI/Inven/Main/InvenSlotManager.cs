using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlotManager : MonoBehaviour {
    private GameObject[] slotObjects = new GameObject[20];
    private InvenSlotController[] invenSlotControllers;
    private PlayerInvenController playerInvenController;
    private InvenSlotSelectController[] invenSelectControllers;
    private SynthesisManager synthesisManager;

    private List<Word> tempInven = new List<Word>();

    private int selectInvenIndex = -1;
    private int prevSelectInvenIndex = -1;
    public int SelectInvenIndex { get { return selectInvenIndex; } }
    public void SetSelectInvenIndex(int num) { selectInvenIndex = num; }

    public void SetTempInven(List<Word> list) { tempInven = list; }
    public List<Word> TempInven { get { return tempInven; } }

    private void Awake() {
        playerInvenController = FindObjectOfType<PlayerInvenController>();
        Button[] slotButtons = GetComponentsInChildren<Button>();
        slotObjects = new GameObject[slotButtons.Length];
        invenSlotControllers = new InvenSlotController[slotButtons.Length];
        invenSelectControllers = new InvenSlotSelectController[slotButtons.Length];
        synthesisManager = FindObjectOfType<SynthesisManager>();

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

    public void SetInvenSaveTemp() {
        tempInven = playerInvenController.Inven;
    }


    //슬롯 번호로 RectTransform return
    private RectTransform GetInvenSlotRectTransfor(int num) {
        return slotObjects[num].GetComponent<RectTransform>();
    }

    //슬롯 선택
    public void SelectSlot() {
        for (int i = 0; i < invenSelectControllers.Length; i++) {
            if (i == selectInvenIndex) {
                if (prevSelectInvenIndex != selectInvenIndex) {
                    invenSelectControllers[i].Enable();
                }
                else {
                    invenSelectControllers[i].DisEnable();
                }
            }
            else {
                invenSelectControllers[i].DisEnable();
            }
        }
        prevSelectInvenIndex = selectInvenIndex;
    }

    public void ChangeInvenToSynthesisSlot(int index) {
        Word tempWord = playerInvenController.GetWordIndex(selectInvenIndex);
        playerInvenController.RemoveItemIndex(selectInvenIndex);
        playerInvenController.AddItemIndex(synthesisManager.GetSlotWordFromIndex(index), selectInvenIndex);
        synthesisManager.SlotItemChangeFromIndex(index, tempWord);
    }

    //단어 아이템 소모 하기전 행동 취소로 인벤 초기화
    public void ResetInvenToTempInven() {
        playerInvenController.SetInvenReset(tempInven);
    }

    //선택한 index슬롯에 단어 추가
    public void SetWordAdd(int index) {
        synthesisManager.SlotItemChangeFromIndex(index, playerInvenController.GetWordIndex(selectInvenIndex));
    }

}
