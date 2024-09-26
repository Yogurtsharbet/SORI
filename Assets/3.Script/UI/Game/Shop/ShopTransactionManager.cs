using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WordKey = System.UInt16;

// 거래 현황
public class ShopTransactionManager : MonoBehaviour {
    Dictionary<(WordKey, WordRank), int> buyList = new Dictionary<(WordKey, WordRank), int>();
    Dictionary<(WordKey, WordRank), int> sellList = new Dictionary<(WordKey, WordRank), int>();

    public int GetWordPrice(Word word) {
        int price;

        int buyCount = buyList[(word.Key, word.Rank)];
        int sellCount = sellList[(word.Key, word.Rank)];

        int defaultPrice = word.GetPrice();

        float buyCal = buyCount * 2.5f * 100;
        float sellCal = sellCount * 1.2f * 100;

        price = (int)defaultPrice - (int)sellCal + (int)buyCal;

        return price;
    }

    public void BuyWord(Word word) {
        buyList[(word.Key, word.Rank)]++;
    }

    public void SellWord(Word word) {
        sellList[(word.Key, word.Rank)]++;
    }
}
