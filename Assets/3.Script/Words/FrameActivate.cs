using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using WordTag = System.String;
using WordKey = System.UInt16;

public class FrameActivate : MonoBehaviour {
    private static FrameActivate instance;
    private Queue<(Word, Word)> activeFunction;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        activeFunction = new Queue<(Word, Word)>();
    }

    public static void ClearFunction() {
        instance.activeFunction.Clear();
    }
    public static void AppendFunction(Word wordA, Word wordB) {
        instance.activeFunction.Enqueue((wordA, wordB));
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

    //private Vector3 GetIndicatePosition(GameObject target, GameObject indicator, FrameRank rank) {
    //    Vector3 position = indicator.GetComponent<IndicatorControl>().indicatePosition;

    //    float distance = 1f;
    //    switch (rank) {
    //        case FrameRank.EPIC:
    //            distance = 2f; break;
    //        case FrameRank.LEGEND:
    //            distance = 4f; break;
    //    }
    //    position = position * distance;
    //    Debug.Log(position);
    //    return position;
    //}

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
}
