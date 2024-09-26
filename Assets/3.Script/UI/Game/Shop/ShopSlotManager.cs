using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [UI] 상점 - 판매 슬롯 컨트롤러 
public class ShopSlotManager : MonoBehaviour {
    [SerializeField] private GameObject shopSlotPrefab;
    private List<GameObject> shopSlotObjects = new List<GameObject>();
    private ShopSlotController[] shopSlotControllers;
    private ShopTransactionManager shopTransactionManager;

    private List<int> selectShopIndex = new List<int>();

    public delegate void SelectShopItemDelegate(List<int> updatedList);
    public event SelectShopItemDelegate OnListChanged;

    private void Awake() {
        shopTransactionManager = FindObjectOfType<ShopTransactionManager>();
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
            shopSlotControllers[i].SetWord(Word.GetWord());
        }
        CheckProduct();
    }

    private void CheckProduct() {
        for (int i = 0; i < 9; i++) {
            if (shopSlotControllers[i].IsExistWord()) {
                shopSlotControllers[i].gameObject.SetActive(true);
            }
            else {
                shopSlotControllers[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 상점 아이템 선택
    /// 이미 선택 되었으면 list에서 제거, 없으면  list에 넣음
    /// </summary>
    /// <param name="index">index</param>
    public void SelectShopItem(int index) {
        bool isExist = false;
        for (int i = 0; i < 9; i++) {
            if (selectShopIndex[i] == index) {
                isExist = true;
                selectShopIndex.Remove(i);
                break;
            }
        }
        if (!isExist) {
            selectShopIndex.Add(index);
        }

        OnListChanged?.Invoke(selectShopIndex);
    }

    public int GetSelectPrice() {
        int totalPrice = 0;
        for (int i = 0; i < selectShopIndex.Count; i++) {
            totalPrice += shopTransactionManager.GetWordPrice(shopSlotControllers[i].ThisWord);
        }
        return totalPrice;
    }
}
