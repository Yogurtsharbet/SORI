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

        SetInvenSaveTemp();
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
        List<Word> tempList = playerInvenController.Inven;
        for (int i = 0; i < tempList.Count; i++) {
            tempInven.Add(tempList[i]);
        }
    }

    //���� ��ȣ�� RectTransform return
    private RectTransform GetInvenSlotRectTransfor(int num) {
        return slotObjects[num].GetComponent<RectTransform>();
    }

    //���� ����
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

    //�ռ�â index�� �ִ� �ܾ�� �κ� �ܾ� ����Ī
    public void SwitchingInvenToSynthesisSlot(int invenIndex, int index) {
        Word tempWord = playerInvenController.GetWordIndex(invenIndex);
        playerInvenController.RemoveItemIndex(invenIndex);
        playerInvenController.AddItem(synthesisManager.GetSlotWordFromIndex(index), invenIndex);
        synthesisManager.SlotItemChangeFromIndex(index, tempWord, invenIndex);
    }

    //�ܾ� ������ �Ҹ� �ϱ��� �ൿ ��ҷ� �κ� �ʱ�ȭ
    public void TempInvenToPlayerInven() {
        playerInvenController.SetInvenReset(tempInven);
    }

    //������ index���Կ� �ܾ� �߰�
    public void SetWordAdd(int invenIndex, int index) {
        synthesisManager.SlotItemChangeFromIndex(index, playerInvenController.GetWordIndex(invenIndex), invenIndex);
        playerInvenController.RemoveItemIndex(invenIndex);
    }
}