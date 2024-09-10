using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSlotManager : MonoBehaviour {
    private ShopSlotController[] shopSlot;

    private List<Word> products = new List<Word>();

    private void Awake() {
        shopSlot = GetComponentsInChildren<ShopSlotController>();
    }

  
}
