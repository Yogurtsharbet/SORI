using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private FrameListManager frameListManager;
    private SubmitButtonController submitButton;
    private PlayerInvenController playerInvenController;

    private CombineContainer combineContainer;

    private SelectControl selectControl;

    private GameObject[] frameObjects = new GameObject[4];

    private void Awake() {
        frameListManager = FindObjectOfType<FrameListManager>();
        selectControl = FindObjectOfType<SelectControl>();
        submitButton = FindObjectOfType<SubmitButtonController>();

        //TODO: 하나로 합쳐지면 바꾸기
        combineContainer = FindObjectOfType<CombineContainer>();

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

    //서브 프레임이 올려져 있는데 해당위치에 단어와 스위칭
    //서브 프레임에 단어가 있는데 active 한 프레임이 아닐 시 해당 프레임도 인벤으로 돌려놓음
    public void SwitchingFrameToWord(int index, Word word) {
        Frame frame = baseFrame.GetFrame(index);
        for(int i = 0; i < 3; i++) {
            if (frame.IsActive) {
                break;
            }
            else {
                if(frame.GetWord(i) != null) {
                    playerInvenController.AddNewItem(frame.GetWord(i));
                    frame.SetWord(i, null);
                }
            }
        }
        frameListManager.AddFrame(frame);
        baseFrame.SetFrame(index, null);
        SetWord(index, word);
    }

    public void SetWord(int index, Word word) {
        baseFrame.SetWord(index, word);
    }

    public void SetWord(int frameIndex, int index, Word word) {
        Frame frame = baseFrame.GetFrame(frameIndex);
        frame.SetWord(index, word);
    }

    public Word GetWord(int index) {
        return baseFrame.GetWord(index);
    }

    public Word GetWord(int frameIndex, int index) {
        Frame frame = baseFrame.GetFrame(frameIndex);
        return frame.GetWord(index);
    }

    //문장 조합
    public void CombineSubmit() {
        string dialogContents = string.Empty;
        if (baseFrame == null) dialogContents = "프레임 없음";
        else if (FrameValidity.Check(baseFrame)) {

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

        }
        //TODO: Dialog -> FrameValidity 에서 띄워주는 게 좋을 듯?
        if (dialogContents != string.Empty)
            DialogManager.Instance.OpenDefaultDialog(dialogContents, DialogType.FAIL);
    }

    //public void Activate(GameObject target, GameObject indicator) {
    //    baseFrame.Activate(target, indicator);
    //}
}
