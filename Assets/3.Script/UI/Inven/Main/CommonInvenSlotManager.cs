using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonInvenSlotManager : MonoBehaviour {
    protected GameObject[] slotObjects = new GameObject[20];
    protected PlayerInvenController playerInvenController;
    protected InvenSlotSelectController[] invenSelectControllers;

    private List<Word> tempInven = new List<Word>();
    private int selectInvenIndex = -1;
    public List<Word> TempInven { get { return tempInven; } }
    public int SelectInvenIndex { get { return selectInvenIndex; } }

    public void SetTempInven(List<Word> list) { tempInven = list; }
    public void SetSelectInvenIndex(int num) { selectInvenIndex = num; }

    private int prevSelectInvenIndex = -1;

    public void SetInvenSaveTemp() {
        List<Word> tempList = playerInvenController.Inven;
        for (int i = 0; i < tempList.Count; i++) {
            tempInven.Add(tempList[i]);
        }
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

    //¿Œ∫•≥¢∏Æ Ω∫¿ßƒ™
    public void SetInvenSwitching(int index, int targetIndex) {
        playerInvenController.SwitchingItem(index, targetIndex);
        SetInvenSaveTemp();
    }
}
