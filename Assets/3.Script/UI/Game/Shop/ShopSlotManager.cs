using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [UI] 상점 - 판매 슬롯 컨트롤러 
public class ShopSlotManager : MonoBehaviour {
    private ShopSlotController[] shopSlot;

    private List<Word> products = new List<Word>();

    private void Awake() {
        shopSlot = GetComponentsInChildren<ShopSlotController>();
    }

  
}
