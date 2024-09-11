using System.Collections.Generic;
using UnityEngine;

// [UI] �κ��丮 - ���� ��� ���� ���� �Ŵ���
public class CommonInvenSlotManager : MonoBehaviour {
    protected GameObject[] slotObjects = new GameObject[20];
    protected PlayerInvenController playerInvenController;
    protected InvenSlotSelectController[] invenSelectControllers;

    protected List<Word> tempInven = new List<Word>();
    private int selectInvenIndex = -1;
    public List<Word> TempInven { get { return tempInven; } }
    public int SelectInvenIndex { get { return selectInvenIndex; } }

    public void SetTempInven(List<Word> list) { tempInven = list; }
    public void SetSelectInvenIndex(int num) { selectInvenIndex = num; }

    private int prevSelectInvenIndex = -1;

    //temp inven�� player inven ����
    public void SetTempInvenToPlayerInven() {
        List<Word> tempList = playerInvenController.Inven;

        if (tempInven.Count == 0) {
            for (int i = 0; i < tempList.Count; i++) {
                tempInven.Add(null);
            }
        }
        else {
            for (int i = 0; i < tempList.Count; i++) {
                tempInven[i] = null;
            }
        }

        for (int i = 0; i < tempList.Count; i++) {
            tempInven[i] = tempList[i];
        }
    }

    //���� ��ȣ�� RectTransform return
    public RectTransform GetInvenSlotRectTransfor(int num) {
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

    //�κ����� ����Ī
    public void SetInvenSwitching(int index, int targetIndex) {
        playerInvenController.SwitchingItem(index, targetIndex);
    }
}
