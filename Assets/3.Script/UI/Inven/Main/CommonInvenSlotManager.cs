using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonInvenSlotManager : MonoBehaviour {
    private GameObject[] slotObjects = new GameObject[20];
    protected PlayerInvenController playerInvenController;
    private InvenSlotController[] invenSlotControllers;
    private InvenSlotSelectController[] invenSelectControllers;

    private List<Word> tempInven = new List<Word>();
    public void SetTempInven(List<Word> list) { tempInven = list; }
    public List<Word> TempInven { get { return tempInven; } }

    private int selectInvenIndex = -1;
    public int SelectInvenIndex { get { return selectInvenIndex; } }
    public void SetSelectInvenIndex(int num) { selectInvenIndex = num; }

    private int prevSelectInvenIndex = -1;

    private void Awake() {
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

    public void SetInvenSaveTemp() {
        List<Word> tempList = playerInvenController.Inven;
        for (int i = 0; i < tempList.Count; i++) {
            tempInven.Add(tempList[i]);
        }
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

    //ΩΩ∑‘ π¯»£∑Œ RectTransform return
    private RectTransform GetInvenSlotRectTransfor(int num) {
        return slotObjects[num].GetComponent<RectTransform>();
    }

    //ΩΩ∑‘ º±≈√
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
}
