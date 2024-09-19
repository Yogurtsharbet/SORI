using System;
using System.Collections.Generic;
using UnityEngine;

// [UI] �κ��丮 - ���� ��� ���� ���� �Ŵ���
public class CommonInvenSlotManager :MonoBehaviour {

    [SerializeField] protected GameObject invenSlotPrefab;

    protected List<GameObject> slotList = new List<GameObject>();

    protected PlayerInvenController playerInvenController;
    protected InvenSlotSelectController[] invenSelectControllers;

    private List<int> selectInvens = new List<int>();       //������ �κ� index

    private int prevSelectInvenIndex = -1;

    protected bool waitConfirm = false;

    //���� ��ȣ�� RectTransform return
    public RectTransform GetInvenSlotRectTransfor(int num) {
        return slotList[num].GetComponent<RectTransform>();
    }

    //���� ����
    public void SelectSlot(int num) {
        bool isExist = false;
        for (int i = 0; i < selectInvens.Count; i++) {
            if (selectInvens[i] == num) {
                isExist = true;
                invenSelectControllers[num].DisEnable();
                selectInvens.RemoveAt(i);
                break;
            }
        }

        if (!isExist) {
            invenSelectControllers[num].Enable();
            selectInvens.Add(num);
        }
    }

    //�κ����� ����Ī
    public void SetInvenSwitching(int index, int targetIndex) {
        playerInvenController.SwitchingItem(index, targetIndex);
    }

    //�κ� ���� ���� open
    public void RemoveSelecConfirm() {
        if (selectInvens.Count > 0) {
            waitConfirm = true;
            string contents = "���� �����Ͻðڽ��ϱ�?";
            DialogManager.Instance.OpenConfirmDialog(contents, DialogType.WARNING);
        }
    }

    //�κ� ���� Ȯ��
    public void RemoveWord() {
        for (int i = 0; i < selectInvens.Count; i++) {
            playerInvenController.RemoveItemIndex(selectInvens[i]);
        }
    }
}
