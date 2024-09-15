using System.Collections.Generic;
using UnityEngine;

// [UI] �κ��丮 - ���� ��� ���� ���� �Ŵ���
public class CommonInvenSlotManager : MonoBehaviour {

    [SerializeField] protected GameObject invenSlotPrefab;

    protected List<GameObject> slotList = new List<GameObject>();

    protected PlayerInvenController playerInvenController;
    protected InvenSlotSelectController[] invenSelectControllers;

    private int selectInvenIndex = -1;
    public int SelectInvenIndex { get { return selectInvenIndex; } }

    public void SetSelectInvenIndex(int num) { selectInvenIndex = num; }

    private int prevSelectInvenIndex = -1;

    //���� ��ȣ�� RectTransform return
    public RectTransform GetInvenSlotRectTransfor(int num) {
        return slotList[num].GetComponent<RectTransform>();
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
