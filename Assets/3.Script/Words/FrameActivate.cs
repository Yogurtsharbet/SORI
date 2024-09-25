using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using WordTag = System.String;
using WordKey = System.UInt16;

public class FrameActivate : MonoBehaviour {
    private static FrameActivate instance;
    private WordFunction wordFunction;

    private List<(Word, Word, Word)> activeFunction;
    private SelectData selectData;

    public List<WordTag> targetTag { get; private set; }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        wordFunction = FindObjectOfType<WordFunction>();
        activeFunction = new List<(Word, Word, Word)>();
        targetTag = new List<WordTag>();
    }

    public static bool CheckMovable(WordTag tag) {
        //TODO: 이동관련 동사 추가시 TAG 추가
        foreach(var each in instance.activeFunction) {
            if (each.Item1.Tag == tag) {
                if (each.Item3 == null) {
                    if (each.Item2.Tag == "MOVE") return true;
                }
                else if (each.Item3.Tag == "MOVE") return true; 
            }
        }
        return false;
    }

    public static void ClearFunction() {
        instance.activeFunction.Clear();
    }

    public static void AppendFunction(Word wordA, Word wordB, Word wordC = null) {
        // noun-noun 들어오면 Change
        // verb-verb 들어오면 wordCard
        // noun-verb 들어오면 verb property
        instance.activeFunction.Add((wordA, wordB, wordC));
        instance.AppendTag();
    }

    private void AppendTag() {
        // AisB 에서 item1 이 Noun. 동사적용 대상일 경우 Tag Append
        var last = activeFunction[activeFunction.Count - 1];
        if (last.Item3 == null) {
            if (last.Item1.IsNoun)
                targetTag.Add(last.Item1.Tag);
        }
        else {  // AtoBisC

        }
    }

    public new static bool CompareTag(WordTag tag) {
        // tag가 activeFunction의 Item1 에 있는지
        return instance.targetTag.Contains(tag);
    }

    public static void Activate (Word word) {

    }

    public static void Activate(SelectData selectData) {
        instance.selectData = selectData;
        instance.ActivateFunction();
    }

    private void ActivateFunction() {
        //activeFunction Queue에 있는 모든 Function을 실행
        //Select된 오브젝트를 Queue의 word와 비교. 선택된 tag에 붙은 모든 verb / sentence를 activate
        foreach (var eachFunction in activeFunction) {
            if (eachFunction.Item3 == null) {
                for (int i = 0; i < selectData.clickedObject.Count; i++) {
                    var clicked = selectData.clickedObject[i];
                    if (eachFunction.Item1.Tag == clicked.tag) {
                        Function(clicked, eachFunction.Item2, GetIndicator(clicked));
                    }
                }

            }
            else {

            }
        }
    }

    private GameObject GetIndicator(GameObject clicked) {
        if (!CheckMovable(clicked.tag)) return null;

        for (int i = 0; i < selectData.Indicator.Count; i++) {
            var indicator = selectData.Indicator[i];
            if (Vector3.Distance(
                indicator.transform.position, clicked.GetComponent<Collider>().bounds.center) < 0.1f) {
                selectData.Indicator.RemoveAt(i);
                return clicked;
            }
        }
        return null;
    }

    private void Function(GameObject target, Word word, GameObject indicator = null) {
        wordFunction.Excute(new WordFunctionData(target, word, indicator));
    }


    
}

    //private void Function(GameObject target, GameObject indicator) {
    //    WordType verbProperty = Word.CheckWordProperty(wordB)[0];
    //    switch (verbProperty) {
    //        case WordType.isMovable:
    //            FunctionMove(target.transform, GetIndicatePosition(target, indicator, Rank));
    //            break;
    //        case WordType.isChangable:
    //            break;
    //        case WordType.isInteractive:
    //            break;
    //        case WordType.isBreakable:
    //            break;
    //    }
    //}

    //private void FunctionMove(Transform origin, Vector3 destiny) {
    //    if (origin.CompareTag("Player")) origin = origin.parent;
    //    Rigidbody rigid = origin.GetComponent<Rigidbody>();
    //    if(rigid == null) {
    //        rigid = origin.gameObject.AddComponent<Rigidbody>();
    //        rigid.freezeRotation = true;
    //    }

    //    Vector3 direction = destiny - origin.position;
    //    direction.y = 0;

    //    Sequence sequence = DOTween.Sequence();
    //    sequence.Append(rigid.DOMove(destiny, 2f))
    //            .Join(origin.DOLookAt(origin.position + direction, 2f))
    //            .Play();
    //    //TODO: 일부 rigid가 뚫고 지나가는 문제 (파악못함)
    //}
