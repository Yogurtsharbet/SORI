using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [UI] 상점 - 판매 슬롯 컨트롤러 
public class ShopSlotManager : MonoBehaviour {
    [SerializeField] private GameObject shopSlotPrefab;
    private List<GameObject> shopSlotObjects = new List<GameObject>();
    private ShopSlotController[] shopSlotControllers;
    private ShopTransactionManager shopTransactionManager;
    private ReceiptManager receiptManager;

    private List<Word> shopWords = new List<Word>();
    private List<int> selectShopIndex = new List<int>();

    private void Awake() {
        shopTransactionManager = FindObjectOfType<ShopTransactionManager>();
        receiptManager = FindObjectOfType<ReceiptManager>();
        shopSlotControllers = new ShopSlotController[9];
        for (int i = 0; i < 9; i++) {
            Vector3 position;
            if (i < 3) {
                position = new Vector3(185f + (i * 200f), -250f, 0f);
            }
            else if (i < 6) {
                position = new Vector3(185f + (i - 3) * 200f, -450f, 0f);
            }
            else {
                position = new Vector3(185f + (i - 6) * 200f, -650f, 0f);
            }
            GameObject shopObj = Instantiate(shopSlotPrefab, position, Quaternion.identity, gameObject.transform);
            shopObj.name = $"slot{i}";
            shopSlotObjects.Add(shopObj);
            shopSlotControllers[i] = shopObj.GetComponentInChildren<ShopSlotController>();
            shopSlotControllers[i].SetKey(i);
            shopSlotControllers[i].gameObject.SetActive(false);
        }
    }

    private void Start() {
        initProduct();
    }

    private void initProduct() {
        for (int i = 0; i < 5; i++) {
            shopWords.Add(Word.GetWord());
        }
        CheckProduct();
    }

    private void CheckProduct() {
        for (int i = 0; i < shopSlotControllers.Length; i++) {
            if (i < shopWords.Count) {
                shopSlotControllers[i].gameObject.SetActive(true);
                shopSlotControllers[i].SetWord(shopWords[i]);
            }
            else {
                shopSlotControllers[i].gameObject.SetActive(false);
                shopSlotControllers[i].SetWord(null);
            }
        }
    }

    /// <summary>
    /// 상점 아이템 선택
    /// 이미 선택 되었으면 list에서 제거, 없으면  list에 넣음
    /// </summary>
    /// <param name="index">index</param>
    public void SelectShopItem(int index) {
        if (selectShopIndex.Remove(index))
            shopSlotControllers[index].ActiveSelect(false);
        else {
            selectShopIndex.Add(index);
            shopSlotControllers[index].ActiveSelect(true);
        }
        receiptManager.UpdateReciptData();
    }


    public int GetSelectCount() {
        return selectShopIndex.Count;
    }

    public int GetSelectPrice() {
        int totalPrice = 0;
        for (int i = 0; i < selectShopIndex.Count; i++) {
            totalPrice += shopTransactionManager.GetWordPrice(shopWords[selectShopIndex[i]]);
        }
        return totalPrice;
    }

    public List<Word> BuyItems() {
        List<Word> buyWords = new List<Word>();
        for (int i = 0; i < selectShopIndex.Count; i++) {
            buyWords.Add(shopWords[selectShopIndex[i]]);
        }
        for (int i = 0; i < selectShopIndex.Count; i++) {
            shopWords.Remove(buyWords[i]);
        }
        resetShopSelects();
        CheckProduct();
        return buyWords;
    }

    public int GetWordPrice(Word word) {
        return shopTransactionManager.GetWordPrice(word);
    }

    private void resetShopSelects() {
        for(int i = 0; i < shopSlotControllers.Length; i++) {
            shopSlotControllers[i].ActiveSelect(false);
        }
        selectShopIndex.RemoveRange(0, selectShopIndex.Count);
    }
}
