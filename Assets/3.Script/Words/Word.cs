using System;
using System.Collections.Generic;
using UnityEngine;

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
    Null = 0

        , NORMAL = 1 << 0
        , EPIC = 1 << 1
        , LEGEND = 1 << 2
        , UNIQUE = 1 << 3
        , SPECIAL = 1 << 4

    , All = int.MaxValue
}

public class Word {
    protected readonly string _name;
    public string Name { get { return _name; } }

    protected WordType _type;
    public WordType Type { get { return _type; } }
    private static WordType[] allRank = (WordType[])Enum.GetValues(typeof(WordType));


    protected readonly WordRank _rank;
    public WordRank Rank { get { return _rank; } }

    public Color TypeColor { get { return TypeColors[(int)GetWordType(this)]; } }
    public Color GetTypeColor(Word word) { return TypeColors[(int)GetWordType(word)]; }

    private Color[] TypeColors = new Color[] {
        new Color(0.61f, 0.49f, 0.78f),     //명사
        new Color(0.93f, 0.52f, 0.62f),     //동사
        new Color(0.93f, 0.8f, 0.41f)          //형용사
    };

    public Color RankColor { get { return RankColors[(int)Rank]; } }
    public Color GetRankColor(Word word) { return RankColors[(int)word.Rank]; }

    private Color[] RankColors = new Color[] {
        new Color(0.59f, 0.59f,  0.59f),    //일반
        new Color(0.38f,0.59f, 0.82f),      //희귀
        new Color(0.93f, 0.8f, 0.41f),      //전설
        new Color(0.61f, 0.49f, 0.78f),     //유일
        new Color(0.85f, 0.53f, 0.75f)      //특수
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

    public Word(string name, WordRank rank, bool isRandom = false) {
        _name = name;
        _rank = SelectRank(rank, isRandom);
    }

    public WordRank SelectRank(WordRank rank, bool isRandom) {
        List<WordRank> availableRank = new List<WordRank>();

        foreach (WordRank each in allRank) {
            if (each == WordRank.Null || each == WordRank.All) continue;
            if ((each & rank) != 0) availableRank.Add(each);
        }

        if (availableRank.Count == 0)
            return WordRank.Null;
        if (isRandom)
            return availableRank[UnityEngine.Random.Range(0, availableRank.Count)];
        else
            return availableRank[0];
    }

    public static WordType GetWordType(Word word) {
        // Return Noun, Verb, Adj 
        if (word is WordNoun) return WordType.NOUN;
        if (word is WordVerb) return WordType.VERB;
        if (word is WordAdj) return WordType.ADJ;
        return WordType.Null;
    }
    public static WordType CheckWordType(WordVerb verb) {
        // Return Word Property
        foreach (WordType each in allRank) {
            if ((each & (WordType.Null | WordType.NOUN | WordType.VERB | WordType.ADJ)) != 0) continue;
            if (each == WordType.All) continue;

            if ((each & verb.Type) != 0) return each;
        }
        return WordType.Null;
    }

    public static List<WordType> CheckWordType(WordNoun noun) {
        // Return Word Property
        List<WordType> types = new List<WordType>();
        foreach (WordType each in allRank) {
            if ((each & (WordType.Null | WordType.NOUN | WordType.VERB | WordType.ADJ)) != 0) continue;
            if (each == WordType.All) continue;

            if ((each & noun.Type) != 0) types.Add(each);
        }
        return types;
    }
}
