using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [UI] 상점 - 판매 슬롯 컨트롤러 
public class ShopSlotManager : MonoBehaviour {
    private ShopSlotController[] shopSlot;

    private List<Word> products = new List<Word>();

    private void Awake() {
        shopSlot = GetComponentsInChildren<ShopSlotController>();
        for(int i=0; i < 9; i++) {
            shopSlot[i].SetKey(i);
            shopSlot[i].gameObject.SetActive(false);
        }
    }

    private void Start() {
        initProduct();
    }

    private void initProduct() {
        for(int i = 0; i < 2; i++) {
            products.Add(Word.GetWord());
        }
        CheckProduct();
    }

    private void CheckProduct() {
        for(int i =0; i< 9; i++) {
            if(i < products.Count) {
                shopSlot[i].gameObject.SetActive(true);
            }
            else {
                shopSlot[i].gameObject.SetActive(false);
            }
        }
    }

    
}
