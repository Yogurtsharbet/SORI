using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [Player] �ŷ� - �ŷ� ��Ȳ
public class PlayerTransactionManager : MonoBehaviour {
    //(key, Rank), �ŷ� ����
    private Dictionary<(int, WordRank), int> buyStatus = new Dictionary<(int, WordRank), int>();
    private Dictionary<(int, WordRank), int> sellStatus = new Dictionary<(int, WordRank), int>();
}
