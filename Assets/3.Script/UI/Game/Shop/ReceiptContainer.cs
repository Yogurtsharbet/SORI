using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [UI] 상점 - 영수증
public class ReceiptContainer : MonoBehaviour {
    private PlayerBehavior playerBehavior;

    #region UI 세팅
    private Text currentAmount; //현재 보유량
    private int buyCount;
    private Text buyCountText;
    private Text buyAmount;
    private int sellCount;
    private Text sellCountText;
    private Text sellAmount;
    #endregion

    private List<int> selectSlots = new List<int>();
    private void Awake() {
        playerBehavior = FindObjectOfType<PlayerBehavior>();
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
        //TODO: player behavior 연결하기
        //SetCurrentAmount(playerBehavior.StarCoin);
    }

    public void SetCurrentAmount(int num) {
        currentAmount.text = $"x{num}";
    }

    private void setBuyCount() {
        buyCountText.text = $"총 {buyCount}개";
    }

    private void setBuyAmount() {
        int totalBuy = sellCount * 1;
        buyAmount.text = $"x{totalBuy}";
    }

    private void setSellCount() {
        sellCountText.text = $"총 {sellCount}개";
    }

    private void setSellAmount(int num) {
        int totalSell = sellCount * 1;
        sellAmount.text = $"x{totalSell}";
    }

    public void SelectSlot(int key) {
        //있으면 삭제, 없으면 추가
        int index = -1;
        for (int i = 0; i < selectSlots.Count; i++) {
            if (selectSlots[i] == key) {
                index = i;
                break;
            }
        }

        if (index != -1) {
            selectSlots.Add(key);
        }
        else {
            selectSlots.RemoveAt(index);
        }
    }
}
