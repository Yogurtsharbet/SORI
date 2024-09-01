using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainDetailManager : MonoBehaviour {
    private DetailLoadManager detailLoadManager;
    private DetailOptionManager detailOptionManager;
    private Text title;

    private void Awake() {
        detailLoadManager = FindObjectOfType<DetailLoadManager>();
        detailOptionManager = FindObjectOfType<DetailOptionManager>();
        title = GetComponentInChildren<Text>();
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenDetailData(int key) {
        if(key == 0) {  //불러오기
            title.text = "불러오기";
            OpenDetailLoad();
        }
        else {          //옵션
            title.text = "옵션";
            OpenDetailOption();
        }
    }

    public void OpenDetailLoad() {
        detailLoadManager.gameObject.SetActive(true);
        detailOptionManager.gameObject.SetActive(false);
    }

    public void OpenDetailOption() {
        detailLoadManager.gameObject.SetActive(false) ;
        detailOptionManager.gameObject.SetActive(true);
    }
}
