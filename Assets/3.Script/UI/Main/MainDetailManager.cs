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
        if(key == 0) {  //�ҷ�����
            title.text = "�ҷ�����";
            OpenDetailLoad();
        }
        else {          //�ɼ�
            title.text = "�ɼ�";
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
