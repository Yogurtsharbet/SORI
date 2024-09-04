using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum WordType {
    Null = 0

        , NOUN = 1 << 0
        , VERB = 1 << 1
        , ADJ = 1 << 2
        , isMovable = 1 << 3
        , isChangable = 1 << 4
        , isInteractive = 1 << 5
        , isBreakable = 1 << 6
        , isAnimal = 1 << 7
        , isLiving = 1 << 8

    , All = int.MaxValue
}

public enum WordRank {
    _Random = 0

        , NORMAL = 1 << 0
        , EPIC = 1 << 1
        , LEGEND = 1 << 2
        , UNIQUE = 1 << 3
        , SPECIAL = 1 << 4

    , All = int.MaxValue
}

public enum WordKey {
    _Random,
    SORI,
    DOOR,
    KEY,
    FLOOR,
    BIRD,
    HP,
    TIME,
    MOVE
}

public class Word {
    // Member Variables
    protected readonly WordKey _key;
    protected readonly string _name;
    protected readonly WordRank _rank;
    protected WordType _type;

    private static WordType[] allType = (WordType[])Enum.GetValues(typeof(WordType));
    private static WordRank[] allRank = (WordRank[])Enum.GetValues(typeof(WordRank));
    private static WordKey[] allKey = (WordKey[])Enum.GetValues(typeof(WordKey));

    // Properties
    public WordKey Key { get { return _key; } }
    public string Name { get { return _name; } }
    public WordRank Rank { get { return _rank; } }
    public WordType Type { get { return GetWordType(this); } }

    public Color RankColor { get { return RankColors[Rank]; } }
    public Color TypeColor { get { return TypeColors[Type]; } }

    public string RankText {
        get {
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
    }
    public string TypeText {
        get {
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

    private Dictionary<WordType, Color> TypeColors = new Dictionary<WordType, Color>() {
        { WordType.NOUN, new Color(0.51f, 0.31f, 0.82f) },      //���
        { WordType.VERB,  new Color(0.86f, 0.33f, 0.47f) },     //����
        { WordType.ADJ, new Color(1f, 0.8f, 0.19f) }            //�����
    };
    private Dictionary<WordRank, Color> RankColors = new Dictionary<WordRank, Color>() {
        { WordRank.NORMAL, new Color(0.87f, 0.87f,  0.87f) },   //�Ϲ�
        { WordRank.EPIC, new Color(0.18f, 0.24f, 0.61f) },      //���
        { WordRank.LEGEND, new Color(1f, 0.8f, 0.19f) },        //����
        { WordRank.UNIQUE, new Color(0.51f, 0.31f, 0.82f) },    //����
        { WordRank.SPECIAL, new Color(0.86f, 0.33f, 0.47f) }    //Ư��
    };

    public static WordType GetWordType(Word word) {
        // Return Noun, Verb, Adj 
        foreach (WordType each in allType) 
            if ((each & word.Type) != 0) return each;

        return WordType.Null;
    }
    public static List<WordType> CheckWordProperty(Word word) {
        // Return Word Property
        List<WordType> types = new List<WordType>();
        foreach (WordType each in allType) {
            if ((each & (WordType.Null | WordType.NOUN | WordType.VERB | WordType.ADJ)) != 0) continue;
            if (each == WordType.All) continue;

            if ((each & word.Type) != 0) types.Add(each);
        }
        return types;
    }
    private WordRank SelectRank(WordRank targetRank, bool isRandom = true) {
        List<WordRank> availableRank = new List<WordRank>();

        foreach (WordRank eachRank in allRank) {
            if (eachRank == WordRank._Random || eachRank == WordRank.All) continue;
            if ((eachRank & targetRank) != 0) availableRank.Add(eachRank);
        }

        if (isRandom)
            return availableRank[Random.Range(0, availableRank.Count)];
        else
            return availableRank[0];
    }

    // Word List
    private Word[] words = new Word[] {
        new Word(WordKey.SORI, "�Ҹ�", WordRank.NORMAL | WordRank.EPIC,
                    WordType.NOUN | WordType.isMovable | WordType.isChangable |
                    WordType.isInteractive | WordType.isLiving),
        new Word(WordKey.DOOR, "��", WordRank.NORMAL | WordRank.EPIC,
                    WordType.isMovable | WordType.isChangable |
                    WordType.isInteractive | WordType.isBreakable),
        new Word(WordKey.MOVE, "�����δ�", WordRank.EPIC | WordRank.LEGEND,
                    WordType.VERB | WordType.isMovable),
    };

    private Word FindWordByKey(WordKey key) {
        foreach(Word word in words) 
            if (word.Key == key) return word;
        return null;
    }

    // Create
    public static Word GetWord(WordKey key = WordKey._Random, WordRank rank = WordRank._Random) {
        return new Word(
            key == WordKey._Random ? (WordKey)Random.Range(1, allKey.Length) : key, rank);
    }

    public Word(WordKey key, WordRank rank = WordRank._Random) {
        // �ܾ� ����Ʈ���� Ű�� �޾ƿ� ���� ( �ΰ��ӿ��� ��� )
        Word newWord = FindWordByKey(key);
        _key = newWord.Key;
        _name = newWord.Name;
        _type = newWord.Type;
        _rank = rank == WordRank._Random ? SelectRank(rank) : rank;
    }

    public Word(WordKey key, string name, WordRank rank, WordType type) {
        // �ܾ� ������ ���� ( �ܾ� ����Ʈ�� ���� )
        _key = key;
        _name = name;
        _rank = rank;
        _type = type;
    }

}
