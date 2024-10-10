using System;
using System.Collections;
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
        if (function.word.IsNoun) Change();
        else functionList[data.word.Key]();
    }

    private void Awake() {
        functionList = new Dictionary<WordKey, Action>();
    }

    private void Start() {
        functionList.Add(WordData.Search("MOVE").Key, Move);
        functionList.Add(WordData.Search("FLY").Key, Fly);
        functionList.Add(WordData.Search("DISAPPEAR").Key, Disappear);
        functionList.Add(WordData.Search("CHANGE").Key, Change);

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
        if (function.target.TryGetComponent(out RustKeyMovement rustKey)) rustKey.isFloating = false;

        Collider collider = targetTransform.GetComponentInChildren<Collider>();
        Rigidbody rigid = targetTransform.GetComponent<Rigidbody>();
        if (rigid == null) {
            rigid = targetTransform.gameObject.AddComponent<Rigidbody>();
            rigid.isKinematic = true;
        }

        rigid.freezeRotation = true;
        var isTrigger = collider.isTrigger; collider.isTrigger = false;
        var isKinematic = rigid.isKinematic; rigid.isKinematic = false;
        var useGravity = rigid.useGravity; rigid.useGravity = true;

        Vector3 destiny = GetIndicatePosition(function.indicator);
        Vector3 direction = destiny - targetTransform.position;
        direction.y = 0; 


        Sequence sequence = DOTween.Sequence();
        sequence.Append(rigid.DOMove(destiny, 2f))
                //.Join(targetTransform.DORotate(lookAt, 2f))
                .OnComplete(() => AfterMove(rigid, isKinematic, useGravity, isTrigger))
                .Play();

        function.indicator.SetActive(false);
    }

    private void AfterMove(Rigidbody rigid, bool isKinematic, bool useGravity, bool isTrigger) {
        if (function.target.CompareTag("WALL"))
            function.target.GetComponent<Rigidbody>().freezeRotation = false;
        if (function.target.TryGetComponent(out RustKeyMovement rustKey))
            rustKey.InitRustKey();

        rigid.velocity = Vector3.zero;
        rigid.isKinematic = isKinematic;
        rigid.useGravity = useGravity;
        rigid.GetComponent<Collider>().isTrigger = isTrigger;

    }

    private void Fly() {
        Transform targetTransform = function.target.transform;
        if (function.target.CompareTag("Player")) targetTransform = targetTransform.parent;
        if (function.target.TryGetComponent(out RustKeyMovement rustKey)) rustKey.isFloating = false;

        Collider collider = targetTransform.GetComponent<Collider>();
        Rigidbody rigid = targetTransform.GetComponent<Rigidbody>();
        if (rigid == null) {
            rigid = targetTransform.gameObject.AddComponent<Rigidbody>();
            rigid.isKinematic = true;
        }
        
        rigid.freezeRotation = true;
        var isTrigger = collider.isTrigger;     collider.isTrigger = false;
        var isKinematic = rigid.isKinematic;    rigid.isKinematic = false;
        var useGravity = rigid.useGravity;      rigid.useGravity = true;

        Vector3 destiny = GetIndicatePosition(function.indicator);
        Vector3 direction = destiny - targetTransform.position;
        direction.y = 45;

        rigid.AddForce(direction, ForceMode.Impulse);
        StartCoroutine(AfterFly(rigid, isKinematic, useGravity, isTrigger));

        function.indicator.SetActive(false);
    }

    private IEnumerator AfterFly(Rigidbody rigid, bool isKinematic, bool useGravity, bool isTrigger) {
        yield return new WaitForSeconds(0.2f);
        while (rigid.velocity.magnitude > 0.05f) yield return null;
        
        rigid.velocity = Vector3.zero;
        rigid.isKinematic = isKinematic;
        rigid.useGravity = useGravity;
        rigid.GetComponent<Collider>().isTrigger = isTrigger;

        if (rigid.CompareTag("WALL"))
            rigid.freezeRotation = false;
        if (rigid.TryGetComponent(out RustKeyMovement rustKey)) {
            rustKey.transform.DOMove(transform.up * 7f, 1f);
            rustKey.InitRustKey();
        }
    }

    private void Disappear() {
        var collider = function.target.GetComponentInChildren<Collider>();
        if (collider == null) collider = function.target.transform.parent.GetComponent<Collider>();
        collider.gameObject.SetActive(false);
    }

    private void Change() {
        function.target.SetActive(false);
        var changeTarget = GameObject.FindGameObjectWithTag(function.word.Tag);
        Instantiate(changeTarget, function.target.transform.position, changeTarget.transform.rotation);
        //TODO: 바꾸기 애초에 Actiavte에서 clicked 를 여러개 들고와야함; word를 GameObject로 바꿀수없음
    }
}
