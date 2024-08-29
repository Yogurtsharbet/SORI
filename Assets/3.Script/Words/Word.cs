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
        new Color(0.87f, 0.87f,  0.87f),    //�Ϲ�
        new Color(0.18f, 0.24f, 0.61f),     //���
        new Color(1f, 0.8f, 0.19f),         //����
        new Color(0.51f, 0.31f, 0.82f),     //����
        new Color(0.86f, 0.33f, 0.47f)      //Ư��
    };

    public Color GetTypeColor(WordType type) { return TypeColors[(int)type]; }

    private Color[] TypeColors = new Color[] {
        new Color(0.51f, 0.31f, 0.82f),     //���
        new Color(0.86f, 0.33f, 0.47f),     //����
        new Color(1f, 0.8f, 0.19f)          //�����
    };

    public string GetRankText() {
        switch (_rank) {
            case WordRank.NORMAL:
                return "�Ϲ�";
            case WordRank.EPIC:
                return "���";
            case WordRank.LEGEND:
                return "����";
            case WordRank.UNIQUE:
                return "����";
            case WordRank.SPECIAL:
                return "Ư��";
            default:
                return "";
        }
    }

    public string GetTypeText() {
        switch (_type) {
            case WordType.NOUN:
                return "���";
            case WordType.VERB:
                return "����";
            case WordType.ADJ:
                return "�����";
            default:
                return "";
        }
    }
}