using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [UI] 조합 - 서브 프레임 (각 슬롯 내부의 프레임) 관리
public class CombineSubFrameController : MonoBehaviour {
    private GameObject[] types;

    private void Awake() {
        for (int i = 0; i < 2; i++) {
            types[i] = gameObject.transform.GetChild(i).gameObject;
        }
    }

    private void Start() {
        types[0].SetActive(false);
        types[1].SetActive(false);
    }

    public void OpenFirstType() {
        types[0].SetActive(true);
        types[1].SetActive(false);
    }

    public void OpenSecondType() {
        types[0].SetActive(false);
        types[1].SetActive(true);
    }


}
