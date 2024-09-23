using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombineSlotController : MonoBehaviour {
    private Text wordText;
    private int slotIndex;
    public int SlotIndex => slotIndex;
    private List<GameObject> dataObject = new List<GameObject>();

    private int childCount = 0;


    private void Awake() {
        if (gameObject.transform.parent.name.Contains("00")) {
            slotIndex = 0;
        }
        else if (gameObject.transform.parent.name.Contains("01")) {
            slotIndex = 1;
        }
        else {
            slotIndex = 2;
        }
        childCount = gameObject.transform.childCount;
        if (childCount == 1) {
            dataObject.Add(gameObject.transform.GetChild(0).gameObject);
        }
        else {
            dataObject.Add(gameObject.transform.GetChild(0).gameObject);    //word
            dataObject.Add(gameObject.transform.GetChild(1).gameObject);    //sentence
            dataObject.Add(dataObject[1].transform.GetChild(0).gameObject); //frame01
            dataObject.Add(dataObject[1].transform.GetChild(1).gameObject); //frame02
        }

        wordText = dataObject[0].GetComponentInChildren<Text>();

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

    public void CloseFrame() {
        dataObject[1].SetActive(false);
        dataObject[2].SetActive(false);
        dataObject[3].SetActive(false);
    }

    public int OpenWord(Word word) {
        dataObject[0].SetActive(true);
        wordText.text = word.Name;
        return slotIndex;
    }
}