using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WordKey = System.UInt16;

// 거래 현황
public class ShopTransactionManager : MonoBehaviour {
    Dictionary<(WordKey, WordRank), int> transactions = new Dictionary<(WordKey, WordRank), int>();

    public int GetWordPrice(Word word) {
        if (transactions.ContainsKey((word.Key, word.Rank))) {
            return transactions[(word.Key, word.Rank)];
        }
        else {
            return word.GetPrice();
        }
    }

    public void BuyWord(Word word) {
        if (transactions.ContainsKey((word.Key, word.Rank))) {
            transactions.Add((word.Key, word.Rank), word.GetPrice());
        }

        int price = 0;
        float cal = transactions[(word.Key, word.Rank)] * 1.15f;

        switch (word.Rank) {
            case WordRank.NORMAL:
                price = Mathf.Clamp((int)cal, 30, 270);
                break;
            case WordRank.EPIC:
                price = Mathf.Clamp((int)cal, 200, 600);
                break;
            case WordRank.LEGEND:
                price = Mathf.Clamp((int)cal, 530, 970);
                break;
            case WordRank.UNIQUE:
                price = Mathf.Clamp((int)cal, 960, 1440);
                break;
            case WordRank.SPECIAL:
                price = Mathf.Clamp((int)cal, 1490, 2010);
                break;
        }

        transactions[(word.Key, word.Rank)] = price;
    }

    public void SellWord(Word word) {
        if (transactions.ContainsKey((word.Key, word.Rank))) {
            transactions.Add((word.Key, word.Rank), word.GetPrice());
        }

        int price = 0;
        float cal = transactions[(word.Key, word.Rank)] * 0.92f;

        switch (word.Rank) {
            case WordRank.NORMAL:
                price = Mathf.Clamp((int)cal, 30, 270);
                break;
            case WordRank.EPIC:
                price = Mathf.Clamp((int)cal, 200, 600);
                break;
            case WordRank.LEGEND:
                price = Mathf.Clamp((int)cal, 530, 970);
                break;
            case WordRank.UNIQUE:
                price = Mathf.Clamp((int)cal, 960, 1440);
                break;
            case WordRank.SPECIAL:
                price = Mathf.Clamp((int)cal, 1490, 2010);
                break;
        }

        transactions[(word.Key, word.Rank)] = price;
    }
}
