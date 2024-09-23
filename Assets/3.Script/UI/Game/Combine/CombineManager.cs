using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SlotType 01 > [ word , sentence03, sentence04 ] , [ word, sentence03, sentence04 ]
//SlotType 02 > [ word , sentence03, sentence04 ] , [ word, sentence03, sentence04 ]
//SlotType 03 > [ word ] , [ word ] / [ sentence01 , sentence02 ] , [ sentence01, sentence02 ]
//SlotType 04 > [ word ]

/*  
 *  1. sentence type에 따라 먼저 어떤 틀을 열건지 체크
 *  2. 틀을 연 후 드래그해 온것이 sentence인지 word인지 체크해서 열기
 *  3. 중간에 바뀌면 그전것 되돌리기
 * 
 */

// [UI] 조합 - 조합 매니저. 조합 창의 한 문장 틀
public class CombineManager : MonoBehaviour {
    //기본 프레임 open 여부
    private bool baseFrameOpen = false;
    public bool BaseFrameOpen => baseFrameOpen;

    //기본 프레임
    private Frame baseFrame;
    public Frame BaseFrame => baseFrame;

    //조합 가능여부
    private bool canCombine;
    public bool CanCombine => canCombine;

    //최대 들어갈수 있는 값
    //Frame [ Frame [word, word, word] , word ] , [ Frame [word, word, word] , word ] , [ Frame [word, word, word] , word ]

    private FrameListContainer sentencesManager;
    private SubmitButtonController submitButton;

    //TODO: 하나로 합쳐지면 바꾸기
    private CombineContainer combineContainer;
    private HalfInvenContainer halfInvenContainer;

    private SelectControl selectControl;

    private GameObject[] frameObjects = new GameObject[4];

    private void Awake() {
        sentencesManager = FindObjectOfType<FrameListContainer>();
        selectControl = FindObjectOfType<SelectControl>();
        submitButton = FindObjectOfType<SubmitButtonController>();

        //TODO: 하나로 합쳐지면 바꾸기
        combineContainer = FindObjectOfType<CombineContainer>();
        halfInvenContainer = FindObjectOfType<HalfInvenContainer>();

        for (int i = 0; i < 4; i++) {
            frameObjects[i] = gameObject.transform.GetChild(i).gameObject;
            frameObjects[i].SetActive(false);
        }
    }

    private void Start() {
        CloseCombineSlot();
    }

    public void OpenCombineSlot(int key, Frame frame) {
        baseFrameOpen = true;
        gameObject.SetActive(true);

        baseFrame = frame;
        //TODO: base frame 안에 setBase(true) 추가, 다른 프레임 switching 할때 false로 꺼줘야함
        activeFrameType((int)frame.Type - 1);

        if (baseFrame.IsActive) {       //문장틀에 문장이 들어가있는 상태
            canCombine = false;             //조합이 완료된 상태
            submitButton.ButtonToRemove();
        }
        else {
            canCombine = true;              //조합하기를 눌러야 하는 상태
            submitButton.ButtonToSubmit();
        }
    }

    private void activeFrameType(int num) {
        for (int i = 0; i < 4; i++) {
            if (i == num) {
                frameObjects[i].SetActive(true);
            }
            else {
                frameObjects[i].SetActive(false);
            }
        }
    }

    public void CloseCombineSlot() {
        //TODO: 슬롯에 있는 워드 원래대로 돌리기 -> 취소
        baseFrame = null;
        baseFrameOpen = false;
        gameObject.SetActive(false);
    }

    public void CancelCombineTemp() {
        gameObject.SetActive(false);
    }

    //public void SetSlotWords(int index, Word word) {
    //    selectedFrame.SetWord(index, word);
    //    combineSlotControllers[index].SetSlotWord(word);
    //}

    //public RectTransform GetSlotRectTransform(int num) {
    //    return combineSlotControllers[num].GetComponent<RectTransform>();
    //}

    //public void SetSlotWords(int index, Word word) {
    //    combineSlotControllers[index].SetSlotWord(word);
    //}

    //public bool CheckIsSlotExist(int index) {
    //    if (combineSlotControllers[index].SlotWord != null) {
    //        return true;
    //    }
    //    else {
    //        return false;
    //    }
    //}

    //public Word SwitchingWordToInven(int index, Word word) {
    //    Word slotWord = combineSlotControllers[index].SlotWord;
    //    combineSlotControllers[index].SetSlotWord(word);
    //    return slotWord;
    //}

    //문장 조합
    public void CombineSubmit() {
        if (baseFrame == null) return;
        if (!FrameValidity.Check(baseFrame)) return;
        //string dialogContents = string.Empty;

        bool isUnselectable = false;
        foreach (var eachKeyA in FrameValidity.GetCommonWord(0).keys) {
            Word eachWordA = Word.GetWord(eachKeyA);
            if (WordData.wordProperty["UNSELECT"].Contains(eachWordA.Tag))
                isUnselectable = true;
        }

        if (isUnselectable) FrameActivate.Activate();
        else {
            CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);
        }

        combineContainer.CloseCombineField();
        halfInvenContainer.CloseCombineInven();

        //TODO: Dialog -> FrameValidity 에서 띄워주는 게 좋을 듯?
        //if (dialogContents != string.Empty)
        //    DialogManager.Instance.OpenDefaultDialog(dialogContents, DialogType.FAIL);
    }
}
