using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [UI] 조합 - 조합 각 타입별 매니저
public class CombineTypeFrameManager : MonoBehaviour {
    private CombineManager combineManager;
    private CombineDataController[] combineData;

    private Word[] thisWord;
    private Frame[] thisFrame;

    private void Awake() {
        combineManager = FindObjectOfType<CombineManager>();
        combineData = GetComponentsInChildren<CombineDataController>();
    }

    private void OnEnable() {
        if (combineManager.BaseFrame != null) {
            FrameType baseType = combineManager.BaseFrame.Type;
            if (baseType == FrameType.AandB || baseType == FrameType.AisB) {
                thisWord = new Word[2];
                thisFrame = new Frame[2];
                for (int i = 0; i < 2; i++) {
                    thisWord[i] = combineManager.BaseFrame.GetWord(i);
                    thisFrame[i] = combineManager.BaseFrame.GetFrame(i);
                }
            }
            else if (baseType == FrameType.AtoBisC) {
                thisWord = new Word[3];
                thisFrame = new Frame[3];
                for (int i = 0; i < 3; i++) {
                    thisWord[i] = combineManager.BaseFrame.GetWord(i);
                    thisFrame[i] = combineManager.BaseFrame.GetFrame(i);
                }
            }
            else {
                thisWord = new Word[1];
                thisWord[0] = combineManager.BaseFrame.GetWord(0);
                thisFrame = null;
            }
            OpenField();
        }
    }

    private void OpenField() {
        if (thisFrame != null) {
            for (int i = 0; i < thisWord.Length; i++) {
                if (thisWord[i] != null) {
                    combineData[i].OpenWord();
                }
                else {
                    combineData[i].CloseWord();
                }
                if (thisFrame[i] != null) {
                    combineData[i].OpenFrame();
                }
                else {
                    combineData[i].CloseFrame();
                }
            }
        }
        else {
            combineData[0].OpenWord();
        }
    }


}
