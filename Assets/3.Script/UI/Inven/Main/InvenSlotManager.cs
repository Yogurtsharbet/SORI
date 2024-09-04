using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlotManager : MonoBehaviour {
    private GameObject[] slotObjects = new GameObject[20];
    private PlayerInvenController playerInvenController;
    private InvenSlotSelectController[] invenSelectControllers;

    private int selectInvenIndex = 0;
    public int SelectInvenIndex { get { return selectInvenIndex; } }
    public void SetSelectInvenIndex(int num) { selectInvenIndex = num; }

    private void Awake() {
        playerInvenController = FindObjectOfType<PlayerInvenController>();
        invenSelectControllers = FindObjectsOfType<InvenSlotSelectController>();
        Button[] slotButtons = GetComponentsInChildren<Button>();

        int index = 0;
        foreach (Button btn in slotButtons) {
            slotObjects[index] = btn.gameObject;
            slotObjects[index].GetComponent<InvenSlotController>().SetKey(index);
            index++;
        }
    }

    private void Start() {
        playerInvenController.InvenChanged += updateSlot;

        for (int i = 0; i < 20; i++) {
            slotObjects[i].GetComponent<InvenSlotController>().CloseSlot();
        }

        for (int i = 0; i < playerInvenController.InvenOpenCount; i++) {
            slotObjects[i].GetComponent<InvenSlotController>().OpenSlot();
        }
    }

    private void OnDestroy() {
        playerInvenController.InvenChanged -= updateSlot;
    }

    private void updateSlot(List<Word> inventory) {
        //for (int i = 0; i < 21; i++) {
        //    slotObjects[i].GetComponent<InvenSlotController>().CloseSlot();
        //}

        //for (int i = 0; i < inventory.Count; i++) {
        //    slotObjects[i].GetComponent<InvenSlotController>().OpenSlot();
        //}
    }

    private RectTransform GetInvenSlotRectTransfor(int num) {
        return slotObjects[num].GetComponent<RectTransform>();
    }

    public void SelectSlot() {
        for(int i = 0; i < invenSelectControllers.Length; i++) {
            if(i == selectInvenIndex) {
                invenSelectControllers[i].Enable();
            }
            else {
                invenSelectControllers[i].DisEnable();
            }
        }
    }
}
