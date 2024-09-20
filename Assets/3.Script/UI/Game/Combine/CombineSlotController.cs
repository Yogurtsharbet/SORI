using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSlotController : MonoBehaviour {
    private int slotIndex;
    public int SlotIndex => slotIndex;
    private List<GameObject> dataObject = new List<GameObject>();

    private void Awake() {
        if (gameObject.transform.parent.name.Contains("00")) {
            slotIndex = 0;
        }else if (gameObject.transform.parent.name.Contains("01")) {
            slotIndex = 1;
        }
        else {
            slotIndex = 2;
        }

        if (gameObject.transform.childCount == 1) {
            dataObject.Add(gameObject.transform.GetChild(0).gameObject);
        }
        else {
            dataObject.Add(gameObject.transform.GetChild(0).gameObject);
            dataObject.Add(gameObject.transform.GetChild(1).gameObject);
            dataObject.Add(dataObject[1].transform.GetChild(0).gameObject);
            dataObject.Add(dataObject[1].transform.GetChild(1).gameObject);
        }

        for (int i = 0; i < dataObject.Count; i++) {
            dataObject[i].SetActive(false);
        }
    }

    public void OpenFrame(FrameType subType) {
        dataObject[1].SetActive(true);
        for (int i = 0; i < dataObject.Count; i++) {
            if (dataObject[i].name.Equals($"SlotType0{(int)subType}")) {
                dataObject[i].SetActive(true);
                return;
            }
        }
    }


}