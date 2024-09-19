using System;
using System.Collections.Generic;
using UnityEngine;

// [UI] 인벤토리 - 슬롯 목록 관리 공통 매니저
public class CommonInvenSlotManager :MonoBehaviour {

    [SerializeField] protected GameObject invenSlotPrefab;

    protected List<GameObject> slotList = new List<GameObject>();

    protected PlayerInvenController playerInvenController;
    protected InvenSlotSelectController[] invenSelectControllers;

    private List<int> selectInvens = new List<int>();       //선택한 인벤 index

    private int prevSelectInvenIndex = -1;

    protected bool waitConfirm = false;

    //슬롯 번호로 RectTransform return
    public RectTransform GetInvenSlotRectTransfor(int num) {
        return slotList[num].GetComponent<RectTransform>();
    }

    //슬롯 선택
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

    //인벤끼리 스위칭
    public void SetInvenSwitching(int index, int targetIndex) {
        playerInvenController.SwitchingItem(index, targetIndex);
    }

    //인벤 삭제 컨펌 open
    public void RemoveSelecConfirm() {
        if (selectInvens.Count > 0) {
            waitConfirm = true;
            string contents = "정말 삭제하시겠습니까?";
            DialogManager.Instance.OpenConfirmDialog(contents, DialogType.WARNING);
        }
    }

    //인벤 삭제 확정
    public void RemoveWord() {
        for (int i = 0; i < selectInvens.Count; i++) {
            playerInvenController.RemoveItemIndex(selectInvens[i]);
        }
    }
}
