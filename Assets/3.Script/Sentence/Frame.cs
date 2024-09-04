using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum FrameType {
    _Random, AisB, AtoBisC, AandB, NotA
}

public class Frame : MonoBehaviour {
    private static FrameType[] allType = (FrameType[])Enum.GetValues(typeof(FrameType));
    private FrameType _type;
    private int blankCount = 0;
    private Word[] blankWord;
    private bool isActive = false;          //활성화 여부
    private bool isCompelete = false;       //문장 완성 여부
    private bool isPersistence = false;     //영구성

    public FrameType Type { get { return _type; } }
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

    public Frame(FrameType type = FrameType._Random) {
        _type = type == FrameType._Random ? (FrameType)Random.Range(1, allType.Length) : type;
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

    public bool CheckValidity() {
        //TODO: 단어카드를 문장틀에 끌어다 놨을 때마다 호출해서 유효성 검사를 할 것!
        for (int i = 0; i < blankCount; i++)
            if (blankWord[i] == null) return false;

        switch (_type) {
            case FrameType.AisB:
                CheckAisB();
                break;
            case FrameType.AtoBisC:
                break;
            case FrameType.AandB:
                break;
            case FrameType.NotA:
                break;
        }
        return true;
    }

    private bool CheckAisB() {
        Word wordA = blankWord[0];
        Word wordB = blankWord[1];

        if (wordA.Type == WordType.NOUN && wordB.Type == WordType.VERB) {
            if(CheckNounisVerb(wordA, wordB)) {
                switch(verb) {
                    case __Move move:
                        //move.Function();
                        break;
                }
            }

        }

        return true;
    }



    private bool CheckNounisVerb(Word noun, Word verb) {
        if (Word.CheckWordProperty(noun).Contains(Word.CheckWordProperty(verb))) {

        }
        return true;
    }
}
