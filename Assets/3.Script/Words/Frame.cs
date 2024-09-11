using System;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using DTT.AreaOfEffectRegions;

public enum FrameType {
    _Random, AisB, AtoBisC, AandB, NotA
}

public enum FrameRank {
    _Random, NORMAL, EPIC, LEGEND
}

public class Frame {
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

    public Word wordA { get { return blankWord[0] == null ? null : blankWord[0]; } }
    public Word wordB { get { return blankWord[1] == null ? null : blankWord[1]; } }
    public Word wordC { get { return blankWord[2] == null ? null : blankWord[2]; } }

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
        for (int i = 0; i < blankCount; i++) {
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

    public void Activate(GameObject target, GameObject indicator) {
        switch (_type) {
            case FrameType.AisB:
                Function(target, indicator);

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

    private Vector3 GetIndicatePosition(GameObject target, GameObject indicator, FrameRank rank) {
        Vector3 position = indicator.GetComponent<IndicatorControl>().indicatePosition;

        float distance = 1f;
        switch (rank) {
            case FrameRank.EPIC:
                distance = 2f; break;
            case FrameRank.LEGEND:
                distance = 4f; break;
        }
        position = position * distance;
        Debug.Log(position);
        return position;
    }

    private void Function(GameObject target, GameObject indicator) {
        WordType verbProperty = Word.CheckWordProperty(wordB)[0];
        switch (verbProperty) {
            case WordType.isMovable:
                FunctionMove(target.transform, GetIndicatePosition(target, indicator, Rank));
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
        if (origin.CompareTag("Player")) origin = origin.parent;
        Rigidbody rigid = origin.GetComponent<Rigidbody>();
        if(rigid == null) {
            rigid = origin.gameObject.AddComponent<Rigidbody>();
            rigid.freezeRotation = true;
        }

        Vector3 direction = destiny - origin.position;
        direction.y = 0;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(rigid.DOMove(destiny, 2f))
                .Join(origin.DOLookAt(origin.position + direction, 2f))
                .Play();
        //TODO: 일부 rigid가 뚫고 지나가는 문제 (파악못함)
    }

    public void SetWordA_DEBUGGING(WordKey key) {
        blankWord[0] = Word.GetWord(key);
    } 
}
