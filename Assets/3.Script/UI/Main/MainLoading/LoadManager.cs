using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour {
    private int selectIndex = 0;
    public int SelectIndex => selectIndex;

    public void SetSelectIndex(int num) {
        selectIndex = num;
    }
}
