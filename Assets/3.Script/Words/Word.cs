using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using WordKey = System.UInt16;
using WordTag = System.String;

// [Word] 단어 - 단어 기본 클래스
[System.Serializable]
public class Word {
    // Member Variables
    public int _key;
    public string _tag;
    public string _name;
    public WordRank _rank;
    public WordType _type;

    private static WordType[] allType = (WordType[])Enum.GetValues(typeof(WordType));
    private static WordRank[] allRank = (WordRank[])Enum.GetValues(typeof(WordRank));
    //private static WordKey[] allKey = (WordKey[])Enum.GetValues(typeof(WordKey));

    // Properties
    public WordKey Key { get { return (WordKey)_key; } }
    public WordTag Tag { get { return _tag; } }
    public string Name { get { return _name; } }
    public WordRank Rank { get { return _rank; } }
    public WordType Type { get { return _type; } }
    public bool IsPersist { get { return (_type/* & WordType.isPersist*/) != 0; } }

    public Color RankColor { get { return RankColors[Rank]; } }
    public Color TypeColor { get { return TypeColors[Type]; } }

    public string RankText {
        get {
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
    }
    public string TypeText {
        get {
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

    private Dictionary<WordRank, Color> RankColors = new Dictionary<WordRank, Color>() {
        { WordRank.NORMAL, new Color(0.87f, 0.87f,  0.87f) },   //일반
        { WordRank.EPIC, new Color(0.18f, 0.24f, 0.61f) },      //희귀
        { WordRank.LEGEND, new Color(1f, 0.8f, 0.19f) },        //전설
        { WordRank.UNIQUE, new Color(0.51f, 0.31f, 0.82f) },    //유일
        { WordRank.SPECIAL, new Color(0.86f, 0.33f, 0.47f) }    //특수
    };
    private Dictionary<WordType, Color> TypeColors = new Dictionary<WordType, Color>() {
        { WordType.NOUN, new Color(0.51f, 0.31f, 0.82f) },      //명사
        { WordType.VERB,  new Color(0.86f, 0.33f, 0.47f) },     //동사
        { WordType.ADJ, new Color(1f, 0.8f, 0.19f) }            //형용사
    };

    // Propetry 구조 변경으로 인한 메서드 삭제. 240917.
    //private static WordType GetWordType(Word word) {
    //    // Return Noun, Verb, Adj 
    //    foreach (WordType each in allType)
    //        if ((each & word._type) != 0) return each;

    //    return WordType.VERB;
    //}

    public static List<WordType> CheckWordProperty(Word word) {
        // Return Word Property
        List<WordType> types = new List<WordType>();
        foreach (WordType each in allType) {
           // if ((each & (WordType.Null | WordType.NOUN | WordType.VERB | WordType.ADJ)) != 0) continue;
           // if (each == WordType.All) continue;

            if ((each & word._type) != 0) types.Add(each);
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

    private Word FindWordByKey(WordKey key) {
        if (key == WordData.RandomKey) 
            return WordData.words[Random.Range(0, WordData.words.Length)];

        foreach (Word word in WordData.words)
            if (word.Key == key) return word;

        throw new ArgumentException($"Key {key} is not valid key.");
    }

    // Create
    public static Word GetWord(WordKey key = WordData.RandomKey, WordRank rank = WordRank._Random) {
        return new Word(key, rank);
    }
    
    public static Word Create(WordKey key, WordTag tag, string name, WordRank rank, WordType type) {
        return new Word(key, tag, name, rank, type);
    }

    private Word(WordKey key, WordRank rank = WordRank._Random) {
        // 단어 리스트에서 키를 받아와 생성 ( 인게임에서 사용 )
        Word newWord = FindWordByKey(key);
        _key = newWord._key;
        _tag = newWord._tag;
        _name = newWord._name;
        _type = newWord._type;
        _rank = rank == WordRank._Random ? SelectRank(newWord._rank) : rank;
    }
    private Word(WordKey key, WordTag tag, string name, WordRank rank, WordType type) {
        // 단어 데이터 생성 ( 단어 리스트에 저장 )
        _key = key;
        _tag = tag;
        _name = name;
        _rank = rank;
        _type = type;
    }

    public bool IsNoun { get { return Type == WordType.NOUN; } }
    public bool IsVerb { get { return Type == WordType.VERB; } }
    public bool IsAdj { get { return Type == WordType.ADJ; } }

    public int GetPrice() {
        switch (Rank) {
            case WordRank.NORMAL:   return 150;
            case WordRank.EPIC:     return 400;
            case WordRank.LEGEND:   return 750;
            case WordRank.UNIQUE:   return 1000;
            case WordRank.SPECIAL:  return 1500;
            default: return 0;
        }
    }
}
