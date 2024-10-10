using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [UI] 상점 - 영수증
public class ReceiptManager : MonoBehaviour {
    private PlayerBehavior playerBehavior;
    private ShopSlotManager shopSlotManager;
    private HalfInvenManager halfInvenManager;
    private PlayerInvenController playerInvenController;

    #region UI 세팅
    private int current;
    private Text currentAmount; //현재 보유량
    private int buyCount = 0;
    private Text buyCountText;
    private int buyPrice = 0;
    private Text buyAmount;
    private int sellCount = 0;
    private Text sellCountText;
    private int sellPrice = 0;
    private Text sellAmount;
    #endregion

    private void Awake() {
        playerInvenController = FindObjectOfType<PlayerInvenController>();
        playerBehavior = FindObjectOfType<PlayerBehavior>();
        shopSlotManager = FindObjectOfType<ShopSlotManager>();
        halfInvenManager = FindObjectOfType<HalfInvenManager>();

        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text txt in texts) {
            if (txt.name.Equals("CurrentAmount")) {
                currentAmount = txt;
            }
            else if (txt.name.Equals("BuyCount")) {
                buyCountText = txt;
            }
            else if (txt.name.Equals("BuyAmount")) {
                buyAmount = txt;
            }
            else if (txt.name.Equals("SellCount")) {
                sellCountText = txt;
            }
            else if (txt.name.Equals("SellAmount")) {
                sellAmount = txt;
            }
        }
    }
    private void OnEnable() {
        SetCurrentAmount();
    }

    private void Start() {
        UpdateReciptData();
    }

    public void UpdateReciptData() {
        SetCurrentAmount();

        //구입
        buyCount = shopSlotManager.GetSelectCount();
        setBuyCount();
        buyPrice = shopSlotManager.GetSelectPrice();
        buyAmount.text = $"x{buyPrice}";

        //판매
        sellCount = halfInvenManager.GetSelectCount();
        setSellCount();
        sellPrice = halfInvenManager.GetSelectPrice();
        sellAmount.text = $"x{sellPrice}";
        setSellCount();
    }

    public void SetCurrentAmount() {
        current = playerBehavior.StarCoin;
        currentAmount.text = $"x{current}";
    }

    private void setBuyCount() {
        buyCountText.text = $"총 {buyCount}개";
    }

    private void setSellCount() {
        sellCountText.text = $"총 {sellCount}개";
    }

    //구매 확정버튼 클릭
    public void ShopConfirm() {
        if (current <= 0) {
            string contents = "가지고 있는 별이 부족합니다.";
            DialogManager.Instance.OpenDefaultDialog(contents, DialogType.FAIL);
        }
        else if (buyPrice <= sellPrice + current) {
            if ((playerInvenController.ExistInvenCount() - sellCount + buyCount) <= playerInvenController.InvenOpenCount) {
                playerBehavior.EarnStarCoin(sellPrice);
                playerBehavior.UseStarCoin(buyPrice);
                halfInvenManager.SelectItemSell();
                List<Word> buyList = shopSlotManager.BuyItems();
                foreach (Word newWord in buyList) {
                    playerInvenController.AddNewItem(newWord);
                }
                UpdateReciptData();

                string contents = "거래가 완료되었습니다.";
                DialogManager.Instance.OpenDefaultDialog(contents, DialogType.SUCCESS);
            }
            else {
                string contents = "인벤토리에 넣을 칸이 부족합니다.\n단어를 팔거나 버리고 계속해주세요.";
                DialogManager.Instance.OpenDefaultDialog(contents, DialogType.FAIL);
            }
        }
        else {
            string contents = "가지고 있는 별이 부족합니다.";
            DialogManager.Instance.OpenDefaultDialog(contents, DialogType.FAIL);
        }
    }
}
