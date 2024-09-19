using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [UI] 상점 - 판매 슬롯 컨트롤러 
public class ShopSlotManager :MonoBehaviour {
    [SerializeField] private GameObject shopSlotPrefab;
    private List<GameObject> shopSlotObjects = new List<GameObject>();
    private ShopSlotController[] shopSlotControllers;

    private List<int> SelectShopIndex = new List<int>();

    private void Awake() {
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

    public void SelectShopItem(int num) {
        bool isExist = false;
        for (int i = 0; i < 9; i++) {
            if (SelectShopIndex[i] == num) {
                isExist = true;
                SelectShopIndex.Remove(i);
                break;
            }
        }
        if (!isExist) {
            SelectShopIndex.Add(num);
        }
    }
}
