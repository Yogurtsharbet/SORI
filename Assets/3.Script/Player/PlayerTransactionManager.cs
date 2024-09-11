using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [Player] 거래 - 거래 현황
public class PlayerTransactionManager : MonoBehaviour {
    //(key, Rank), 거래 수량
    private Dictionary<(int, WordRank), int> buyStatus = new Dictionary<(int, WordRank), int>();
    private Dictionary<(int, WordRank), int> sellStatus = new Dictionary<(int, WordRank), int>();
}
