using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using WordKey = System.UInt16;
using WordTag = System.String;

public class WordFunctionData {
    public GameObject target;
    public Word word;
    public GameObject indicator;

    public WordFunctionData(GameObject target, Word word, GameObject indicator = null) {
        this.target = target;
        this.word = word;
        this.indicator = indicator;
    }
}

public class WordFunction : MonoBehaviour {
    private Dictionary<WordKey, Action> functionList;
    private WordFunctionData function;

    public FrameRank frameRank = FrameRank.NORMAL;

    public void Excute(WordFunctionData data) {
        function = data;
        functionList[data.word.Key]();
    }

    private void Awake() {
        functionList = new Dictionary<WordKey, Action>();
    }

    private void Start() {
        functionList.Add(WordData.Search("MOVE").Key, Move);
    }

    private Vector3 GetIndicatePosition(GameObject indicator) {
        Vector3 position = indicator.GetComponent<IndicatorControl>().indicatePosition;

        /**
        //TODO: FrameRank에 따른 계산을 Select 할 때 적용시킬 것
        float distance = 1f;
        switch (frameRank) {
            case FrameRank.EPIC:
                distance = 2f; break;
            case FrameRank.LEGEND:
                distance = 4f; break;
        }
        position = position * distance;
        **/
        return position;
    }

    private void Move() {
        Transform targetTransform = function.target.transform;
        if (function.target.CompareTag("Player")) targetTransform = targetTransform.parent;

        Rigidbody rigid = targetTransform.GetComponent<Rigidbody>();
        if (rigid == null) {
            rigid = targetTransform.gameObject.AddComponent<Rigidbody>();
            rigid.freezeRotation = true;
        }

        Vector3 destiny = GetIndicatePosition(function.indicator);
        Vector3 direction = destiny - targetTransform.position;
        direction.y = 0;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(rigid.DOMove(destiny, 2f))
                .Join(targetTransform.DOLookAt(targetTransform.position + direction, 2f))
                .Play();
        function.indicator.SetActive(false);
    }
}
