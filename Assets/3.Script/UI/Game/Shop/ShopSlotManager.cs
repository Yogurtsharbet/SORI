using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [UI] ���� - �Ǹ� ���� ��Ʈ�ѷ� 
public class ShopSlotManager : MonoBehaviour {
    private ShopSlotController[] shopSlot;

    private List<Word> products = new List<Word>();

    private void Awake() {
        shopSlot = GetComponentsInChildren<ShopSlotController>();
    }

  
}
