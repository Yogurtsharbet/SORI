using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WordType {
    NOUN = 0,
    VERB,
    ADJ
}

public enum WordRank {
    NORMAL = 0,
    EPIC,
    LEGEND,
    UNIQUE,
    SPECIAL
}

public class Word : MonoBehaviour {

    private WordType _type;
    public WordType Type { get { return _type; } }

    private string _name;

    public string Name { get { return _name; } }

    private WordRank _rank;
    public WordRank Rank { get { return _rank; } }

    public Color GetRankColor(WordRank rank) { return RankColors[(int)rank]; }

    private Color[] RankColors = new Color[] {
        new Color(0.87f, 0.87f,  0.87f),    //일반
        new Color(0.18f, 0.24f, 0.61f),     //희귀
        new Color(1f, 0.8f, 0.19f),         //전설
        new Color(0.51f, 0.31f, 0.82f),     //유일
        new Color(0.86f, 0.33f, 0.47f)      //특수
    };

    public Color GetTypeColor(WordType type) { return TypeColors[(int)type]; }

    private Color[] TypeColors = new Color[] {
        new Color(0.51f, 0.31f, 0.82f),     //명사
        new Color(0.86f, 0.33f, 0.47f),     //동사
        new Color(1f, 0.8f, 0.19f)          //형용사
    };

    public string GetRankText() {
        switch (_rank) {
            case WordRank.NORMAL:
                return "일반";
            case WordRank.EPIC:
                return "희귀";
            case WordRank.LEGEND:
                return "전설";
            case WordRank.UNIQUE:
                return "유일";
            case WordRank.SPECIAL:
                return "특수";
            default:
                return "";
        }
    }

    public string GetTypeText() {
        switch (_type) {
            case WordType.NOUN:
                return "명사";
            case WordType.VERB:
                return "동사";
            case WordType.ADJ:
                return "형용사";
            default:
                return "";
        }
    }
}