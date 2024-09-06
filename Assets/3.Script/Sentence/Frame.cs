using System;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public enum FrameType {
    _Random, AisB, AtoBisC, AandB, NotA
}

public enum FrameRank {
    _Random, NORMAL, EPIC, LEGEND
}

public class Frame : MonoBehaviour {
    private static FrameType[] allType = (FrameType[])Enum.GetValues(typeof(FrameType));
    private static FrameRank[] allRank = (FrameRank[])Enum.GetValues(typeof(FrameRank));

    private FrameType _type;
    private FrameRank _rank;

    private int blankCount = 0;
    private Word[] blankWord;
    private bool isActive = false;          //활성화 여부
    private bool isCompelete = false;       //문장 완성 여부
    private bool isPersistence = false;     //영구성

    public FrameType Type { get { return _type; } }
    public FrameRank Rank { get { return _rank; } }

    public int BlankCount { get { return blankCount; } }
    public bool IsActive { get { return isActive; } }
    public bool IsCompelete { get { return isCompelete; } }
    public bool IsPersistence { get { return isPersistence; } }

    public void SetBlankCount(int count) {
        blankCount = count;
    }
    public void SetActive(bool yn) {
        isActive = yn;
    }
    public void SetCompelete(bool yn) {
        isCompelete = yn;
    }
    public void SetPersistenct(bool yn) {
        isPersistence = yn;
    }

    public Frame(FrameType type = FrameType._Random, FrameRank rank = FrameRank._Random) {
        _type = type == FrameType._Random ? (FrameType)Random.Range(1, allType.Length) : type;
        _rank = rank == FrameRank._Random ? (FrameRank)Random.Range(1, allRank.Length) : rank;

        switch (_type) {
            case FrameType.NotA:
                blankCount = 1; break;

            case FrameType.AisB:
            case FrameType.AandB:
                blankCount = 2; break;

            case FrameType.AtoBisC:
                blankCount = 3; break;
        }
        blankWord = new Word[blankCount];
    }

    public bool SetWord(Word word) {
        for(int i = 0; i < blankCount;i++) {
            if (blankWord[i] == null) {
                blankWord[i] = word;
                return true;
            }
        }
        return false;
    }

    public bool CheckSentenceValidity() {
        //TODO: 단어카드를 문장틀에 끌어다 놨을 때마다 호출해서 유효성 검사를 할 것!
        for (int i = 0; i < blankCount; i++)
            if (blankWord[i] == null) return false;

        switch (_type) {
            case FrameType.AisB:
                isCompelete = CheckAisB(); break;
            case FrameType.AtoBisC:
                break;
            case FrameType.AandB:
                break;
            case FrameType.NotA:
                break;
        }

        return isCompelete;
    }

    private bool CheckAisB() {
        Word wordA = blankWord[0];
        Word wordB = blankWord[1];

        if (wordA.IsNoun && wordB.IsVerb)
            return CheckNounisVerb(wordA, wordB);

        return false;
    }

    private bool CheckNounisVerb(Word noun, Word verb) {
        var nounProperty = Word.CheckWordProperty(noun);
        var verbProperty = Word.CheckWordProperty(verb);

        foreach (WordType type in verbProperty)
            if (!nounProperty.Contains(type)) return false;
        return true;
    }

    public void Activate(GameObject target) {
        switch (_type) {
            case FrameType.AisB:
                Function(target);
                
                break;
            case FrameType.AtoBisC:
                break;
            case FrameType.AandB:
                break;
            case FrameType.NotA:
                break;
            default: return;
        }
    }

    private void Function(GameObject target) {
        WordType verbProperty = Word.CheckWordProperty(blankWord[1])[0];
        switch (verbProperty) {
            case WordType.isMovable: FunctionMove(target.transform, Vector3.right);
                break;
            case WordType.isChangable:
                break;
            case WordType.isInteractive:
                break;
            case WordType.isBreakable:
                break;
        }
    }

    private void FunctionMove(Transform origin, Vector3 destiny) {
        origin.DOMove(destiny, 2f, true);
    }
}
