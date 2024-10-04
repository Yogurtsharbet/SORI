using System.Collections.Generic;
using UnityEngine;

// [UI] 인벤토리 - 슬롯 목록 관리 공통 매니저
public class CommonInvenSlotManager : MonoBehaviour {

    [SerializeField] protected GameObject invenSlotPrefab;

    protected List<GameObject> slotList = new List<GameObject>();

    protected PlayerInvenController playerInvenController;
    protected InvenSlotSelectController[] invenSelectControllers;

    protected List<int> selectInvens = new List<int>();       //선택한 인벤 index

    protected bool waitConfirm = false;

    /// <summary>
    /// 슬롯 선택
    /// </summary>
    /// <param name="num">slot key int</param>
    public void SelectSlot(int num) {
        bool isExist = false;
        for (int i = 0; i < selectInvens.Count; i++) {
            if (selectInvens[i] == num) {
                isExist = true;
                if (invenSelectControllers != null)
                    invenSelectControllers[num].DisEnable();
                selectInvens.RemoveAt(i);
                break;
            }
        }

        if (!isExist) {
            if (invenSelectControllers != null)
                invenSelectControllers[num].Enable();
            selectInvens.Add(num);
        }
    }

    public void ResetSelectInvens() {
        for (int i = 0; i < invenSelectControllers.Length; i++) {
            invenSelectControllers[i].DisEnable();
        }
        selectInvens.RemoveRange(0, selectInvens.Count);
    }

    public int GetSelectCount() {
        return selectInvens.Count;
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
        for (int i = 0; i < selectInvens.Count; i++) {
            invenSelectControllers[selectInvens[i]].DisEnable();
        }
        selectInvens.RemoveRange(0, selectInvens.Count);
    }
}
