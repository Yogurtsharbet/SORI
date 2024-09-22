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

    private FrameListManager frameListManager;
    private SubmitButtonController submitButton;

    //TODO: 하나로 합쳐지면 바꾸기
    private CombineContainer combineContainer;
    private HalfInvenContainer halfInvenContainer;

    private SelectControl selectControl;

    private GameObject[] frameObjects = new GameObject[4];

    private void Awake() {
        frameListManager = FindObjectOfType<FrameListManager>();
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

    public void OpenBaseFrameSlot(int key, Frame frame) {
        if (baseFrameOpen) {
            //이미 baseFrame이 open 되어있는 상태
            baseFrame.SetBase(false);
            frameListManager.AddFrame(baseFrame);
        }
        else {
            baseFrameOpen = true;
            gameObject.SetActive(true);
        }

        frame.SetBase(true);
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
            //TODO: 하위 슬롯 끄기
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

    //슬롯 닫을 때 초기화
    public void CloseCombineSlot() {
        if (baseFrameOpen) {
            baseFrame.SetBase(false);
            frameListManager.AddFrame(baseFrame);
            if (!baseFrame.IsActive) {
                for (int i = 0; i < 3; i++) {
                    if (baseFrame.GetFrame(i) != null) {
                        frameListManager.AddFrame(baseFrame.GetFrame(i));
                    }
                }
            }
            //TODO: 슬롯에 있는 워드 원래대로 돌리기
        }
        baseFrame = null;
        baseFrameOpen = false;
        gameObject.SetActive(false);
    }

    public void CancelCombineTemp() {
        gameObject.SetActive(false);
    }

    public void SetSubFrame(int index, Frame frame) {
        baseFrame.SetFrame(index, frame);
    }

    public void SetWord(int index, Word word) {
        baseFrame.SetWord(index, word);
    }

    public void SetWord(int frameIndex, int index, Word word) {
        Frame frame = baseFrame.GetFrame(frameIndex);
        frame.SetWord(index, word);
    }

    //문장 조합
    public void CombineSubmit() {
        if (baseFrame == null) return;
        string dialogContents = string.Empty;

        if (baseFrame.CheckSentenceValidity()) {
            //sentencesManager.SetSlotSentence(selectKey, selectedFrame);
            //selectKey = -1; //TODO: ESC로 돌아올 수 있으므로 selectKey가 저장되어야 할 필요 있음

            // for (int i = 0; i < selectedFrame.BlankCount; i++)
            //   SetSlotWords(i, null);
            CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);
            combineContainer.CloseCombineField();
            halfInvenContainer.CloseCombineInven();

            if (dialogContents != string.Empty)
                DialogManager.Instance.OpenDefaultDialog(dialogContents, DialogType.FAIL);
        }
    }
    public void Activate(GameObject target, GameObject indicator) {
        baseFrame.Activate(target, indicator);
    }
}
