using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FrameType {
    AisB, AtoBisC, AandB, NotA
}

public class Frame : MonoBehaviour {
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

    public Frame(FrameType type) {
        _type = type;
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

    public bool CheckAisB() {

        Word wordA = blankWord[0];
        Word wordB = blankWord[1];

        if (wordA is WordNoun noun && wordB is WordVerb verb) {
            if(CheckNounisVerb(noun, verb)) {
                switch(verb) {
                    case __Move move:
                        move.Function();
                        break;
                }
            }

        }

        return true;
    }



    public bool CheckNounisVerb(WordNoun noun, WordVerb verb) {
        if (Word.CheckWordType(noun).Contains(Word.CheckWordType(verb))) {

        }
        return true;
    }
}
