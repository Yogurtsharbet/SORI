using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenContainerManager : MonoBehaviour {

    private void Awake() {
    }

    public void OpenInventory() {
        gameObject.SetActive(true);
    }

    public void CloseInventory() {
        //TODO: temp inventory로 inventory 바꾸기
        //TODO: 합성창 안에 단어 삭제 하기
        gameObject.SetActive(false);
    }
}
