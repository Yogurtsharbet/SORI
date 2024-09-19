using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [UI] 조합 - 조합 데이터 object 관리 컨트롤
public class CombineDataController : MonoBehaviour {
    private GameObject wordObject;
    private GameObject sentenceObject;

    private void Awake() {
        if (gameObject.transform.childCount >= 2) {
            wordObject = gameObject.transform.GetChild(0).gameObject;
            sentenceObject = gameObject.transform.GetChild(1).gameObject;
        }
        else {
            sentenceObject = null;
        }
    }

    public void OpenWord() {
        wordObject.SetActive(true);
    }

    public void CloseWord() {
        wordObject.SetActive(false);
    }

    public void OpenFrame() {
        if (sentenceObject != null)
            sentenceObject.SetActive(true);
    }

    public void CloseFrame() {
        if (sentenceObject != null)
            sentenceObject.SetActive(false);
    }
}
