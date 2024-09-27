using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using WordTag = System.String;
using WordKey = System.UInt16;
using UnityEditor;

public enum CommonType {
    NOUN, VERB, SENTENCE
}

public class CommonWord {
    public List<WordKey> keys = new List<WordKey>();
    public CommonType type;
    public bool IsValid { get; private set; }

    public CommonWord(Frame frame) {
        IsValid = FrameValidity.CheckValidity(frame);
        if (!IsValid) return;

        if (frame.Type == FrameType.AisB || frame.Type == FrameType.AtoBisC)
            type = CommonType.SENTENCE;
        else if (frame.Type == FrameType.AandB &&
            frame.wordA.IsNoun && frame.wordB.IsNoun)
            type = CommonType.NOUN;
        else
            type = CommonType.VERB;

        for (int i = 0; i < frame.BlankCount; i++)
            keys.Add(frame.GetWord(i).Key);
    }

    public CommonWord(Word word) {
        switch (word.Type) {
            case WordType.NOUN:
                type = CommonType.NOUN; break;
            case WordType.VERB:
            case WordType.ADJ:
                type = CommonType.VERB; break;
        }
        IsValid = true;
        keys.Add(word.Key);
    }

}

// Frame 이던 Word 이던 common Class 로 변형한다.
// commom Class 는 3가지 타입. => 명사, 동형용사, 문장
// 또한 속성 => valid, 가지고 있는 단어의 key
// Mount Frame의 경우 Mount Valid 검사시에 common Class로 변형된다.
// Word 또한 common Class로 감싸짐
// Base Frame 또한 변형된 common Class들을 valid Check 해서
// Activate에 commonClass 형태로 return.


public class FrameValidity : MonoBehaviour {
    private static FrameValidity instance;
    private Frame frame;
    private CommonWord[] commonWord;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public static CommonWord GetCommonWord(int index) {
        return instance.commonWord[index];
    }

    public static bool Check(Frame frame) {
        //조합창 만들기 버튼 누를 때 호출되어야 함
        if (instance == null) 
            instance = FindObjectOfType<FrameValidity>();
        instance.frame = frame;
        instance.commonWord = new CommonWord[frame.BlankCount];
        FrameActivate.ClearFunction();
        return instance.CheckFrame();
        //return 받은 쪽에서 true 이면 FrameActivate의 Activate 호출해야함
    }
    private bool CheckFrame() {
        if (!CheckBlank()) return false;
        if (!GetCommonWords()) return false;
        if (!CheckBaseFrame()) return false;
        return true;
    }


    private bool CheckBlank() {
        for (int i = 0; i < frame.BlankCount; i++) {
            if (frame.GetFrame(i) == null && frame.GetWord(i) == null) return false;
            else if (frame.GetFrame(i) != null) {
                Frame eachFrame = frame.GetFrame(i);
                for (int j = 0; j < eachFrame.BlankCount; j++)
                    if (eachFrame.GetWord(j) == null) return false;
            }
        }
        return true;
    }

    private bool GetCommonWords() {
        for (int i = 0; i < frame.BlankCount; i++) {
            if (frame.GetFrame(i) != null) commonWord[i] = new CommonWord(frame.GetFrame(i));
            else if (frame.GetWord(i) != null) commonWord[i] = new CommonWord(frame.GetWord(i));
            else return false;
        }
        foreach (var each in commonWord)
            if (!each.IsValid) return false;

        return true;
    }

    private bool CheckBaseFrame() {
        if (frame.Type == FrameType.NotA) return false;
        for (int i = 0; i < frame.BlankCount; i++) {
            if (!commonWord[i].IsValid ||
                (frame.Type == FrameType.AandB && commonWord[i].type != CommonType.SENTENCE)) 
                return false;
        }
        if(frame.Type == FrameType.AisB) {
            if (commonWord[0].type == CommonType.NOUN) {

                // AisB - Noun is Noun
                if (commonWord[1].type == CommonType.NOUN) {        
                    foreach (var eachKey in commonWord[0].keys) {
                        var eachTag = Word.GetWord(eachKey).Tag;
                        if (!WordData.wordProperty["CHANGE"].Contains(eachTag)) return false;
                    }
                    foreach (var eachKeyA in commonWord[0].keys) {
                        Word eachWordA = Word.GetWord(eachKeyA);
                        foreach(var eachKeyB in commonWord[1].keys) {
                            Word eachWordB = Word.GetWord(eachKeyB);
                            FrameActivate.AppendFunction(eachWordA, eachWordB);
                        }
                        return true;
                    }
                }
                // AisB - Noun is Verb
                else {              
                    List<(Word, Word)> tempPair = new List<(Word, Word)>();
                    foreach(var eachKeyA in commonWord[0].keys) {
                        Word eachWordA = Word.GetWord(eachKeyA);
                        foreach(var eachKeyB in commonWord[1].keys) {
                            Word eachWordB = Word.GetWord(eachKeyB);
                            if (!WordData.wordProperty[eachWordB.Tag].Contains(eachWordA.Tag)) return false;
                            tempPair.Add((eachWordA, eachWordB));
                        }
                    }
                    foreach (var eachPair in tempPair)
                        FrameActivate.AppendFunction(eachPair.Item1, eachPair.Item2);
                    return true;
                }
            }

            // AisB - Verb is Verb
            else if (commonWord[1].type != CommonType.NOUN) {       
                List<(Word, Word)> tempPair = new List<(Word, Word)>();
                //TODO: ADD Destroy Tag func.
                foreach (var eachKeyB in commonWord[1].keys) {
                    Word eachWordB = Word.GetWord(eachKeyB);
                    if (eachWordB.Tag != "CHANGE" /* or destroy */) return false;
                    foreach(var eachKeyA in commonWord[0].keys) {
                        Word eachWordA = Word.GetWord(eachKeyA);
                        tempPair.Add((eachWordA, eachWordB));
                    }
                }
                foreach (var eachPair in tempPair)
                    FrameActivate.AppendFunction(eachPair.Item1, eachPair.Item2);
                return true;
            }
        }
        else if(frame.Type == FrameType.AtoBisC) {

        }

        return false;
    }


    public static bool CheckValidity(Frame frame) {
        return instance.CheckType(frame);
    }

    private bool CheckType(Frame frame = null) {
        // GetCommonWords 에서 CommonWord 생성할 때 호출됨.
        if (frame == null) frame = this.frame;
        switch (frame.Type) {
            case FrameType.AisB:
                return CheckAisB(frame);
            case FrameType.AtoBisC:
                //TODO: CheckAtoBisC(frame);
                break;
            case FrameType.AandB:
                return CheckAandB(frame);
            case FrameType.NotA:
                return CheckNotA(frame);
        }
        return false;
    }

    // 유효성 검사는 property로
    // 하고나면 active 정보는?
    //      -> Senetence 인 경우 commonwords 조사할 때 수행시키기로
    //      -> 그 외는 active 정보가 안생김
    // check가 끝나면 commonWords는?
    //      -> commonWord 조사 : sentence? acitvate. sentence가 생기는건 A and B 일 경우 뿐 / etc? 단어처럼 처리
    //      
    // 즉!!! CheckType을 수행하는 시점에서 MountFrame은 존재할 수 없음!!!!
    // CheckType 에서는 유효한지 검사 / Sentence의 경우 Activate Enqueue
    // 이후 CommonWord 생성자에서 CommonWord의 Type을 결정.

    private bool CheckAisB(Frame frame) {
        if (frame.wordA.IsNoun) {
            if (frame.wordB.IsNoun) {
                if (WordData.wordProperty["CHANGE"].Contains(frame.wordA.Tag)) {
                    FrameActivate.AppendFunction(frame.wordA, frame.wordB);
                    return true;
                }
            }
            else {
                if (WordData.wordProperty[frame.wordB.Tag].Contains(frame.wordA.Tag)) {
                    FrameActivate.AppendFunction(frame.wordA, frame.wordB);
                    return true;
                }
            }
        }
        else if (frame.wordB.Type != WordType.NOUN) { 
            //TODO: ADD Destroy Tag func.
            if (frame.wordB.Tag == "CHANGE" /* or Destroy */) {
                FrameActivate.AppendFunction(frame.wordA, frame.wordB);
                return true;
            }
        }
        return false;
    }

    private bool CheckAandB(Frame frame) {
        if (frame.wordA.Type != frame.wordB.Type) return false;
        return true;
    }

    private bool CheckNotA(Frame frame) {
        if (frame.wordA.IsNoun) return false;
        return true;
    }
}
