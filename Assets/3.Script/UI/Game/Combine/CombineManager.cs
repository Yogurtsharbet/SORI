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
    private WordFunction wordFunction;

    private GameObject[] frameObjects = new GameObject[4];

    private void Awake() {
        frameListManager = FindObjectOfType<FrameListManager>();
        selectControl = FindObjectOfType<SelectControl>();
        submitButton = FindObjectOfType<SubmitButtonController>();
        playerInvenController = FindObjectOfType<PlayerInvenController>();
        combineContainer = FindObjectOfType<CombineContainer>();
        wordFunction = FindObjectOfType<WordFunction>();

        for (int i = 0; i < 4; i++) {
            frameObjects[i] = gameObject.transform.GetChild(i).gameObject;
            frameObjects[i].SetActive(false);
        }
    }

    private void Start() {
        CloseCombineSlot();
    }

    public void OpenBaseFrameSlot(int key, Frame frame) {
        //이미 baseFrame이 open 되어있는 상태
        if (baseFrameOpen) {
            resetBaseFrame();
        }
        else {
            baseFrameOpen = true;
            gameObject.SetActive(true);
        }

        frame.SetBase(true);
        baseFrame = frame;
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

    public void CancelCombineTemp() {
        gameObject.SetActive(false);
    }

    #region 드래그 상호작용 및 데이터 getter, setter

    public void SetSubFrame(int index, Frame frame) {
        resetNotActiveSubFrame(index);
        baseFrame.SetFrame(index, frame);
    }

    //서브 프레임이 올려져 있는데 해당위치에 단어와 스위칭
    //서브 프레임에 단어가 있는데 active 한 프레임이 아닐 시 해당 프레임도 인벤으로 돌려놓음
    public void SwitchingFrameToWord(int index, Word word) {
        Frame frame = baseFrame.GetFrame(index);
        for (int i = 0; i < 3; i++) {
            if (frame.IsActive) {
                break;
            }
            else {
                if (frame.GetWord(i) != null) {
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

    #endregion

    //문장 조합
    public void CombineSubmit() {
        string dialogContents = string.Empty;
        if (baseFrame == null) dialogContents = "프레임 없음";
        else if (FrameValidity.Check(baseFrame)) {
            bool isUnselectable = false;
            wordFunction.frameRank = baseFrame.Rank;

            foreach (var eachKeyA in FrameValidity.GetCommonWord(0).keys) {
                Word eachWordA = Word.GetWord(eachKeyA);
                if (WordData.wordProperty["UNSELECT"].Contains(eachWordA.Tag))
                    isUnselectable = true;
            }//TODO: HP 그리고 소리 가 증가한다 : Unselcet 와 selectable 혼재 할 경우 selectMode 되어야.
            //TODO: 그렇다면 Unselectable에 따라 Activate를 다르게 호출하는것이 아니라, ActiveFunction에서 
            //TODO: activeFunction Queue를 조사할 때 Item1이 unselectable인지 조사해서
            //TODO: selectData를 가져오지 않아도 즉시 function이 작동하도록 바꿔야할듯
 

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

    #region 리셋 및 초기화

    //슬롯 닫을 때 초기화
    public void CloseCombineSlot() {
        if (baseFrameOpen) {
            resetBaseFrame();
        }
        baseFrame = null;
        baseFrameOpen = false;
        gameObject.SetActive(false);
    }

    //베이스 프레임 단어 리셋
    private void resetNotActiveBaseFrame() {
        if (!baseFrame.IsActive) {
            for (int i = 0; i < 3; i++) {
                if (baseFrame.GetWord(i) != null) {
                    playerInvenController.AddNewItem(baseFrame.GetWord(i));
                    baseFrame.SetWord(i, null);
                }
                if (baseFrame.GetFrame(i) != null) {
                    Frame tempFrame = baseFrame.GetFrame(i);
                    if (!tempFrame.IsActive) {
                        if (tempFrame.GetWord(i) != null) {
                            playerInvenController.AddNewItem(tempFrame.GetWord(i));
                            tempFrame.SetWord(i, null);
                        }
                    }
                }
            }
        }
    }

    //서브 프레임 단어 리셋
    private void resetNotActiveSubFrame(int frameIndex) {
        if (baseFrame.GetFrame(frameIndex) != null) {
            Frame tempFrame = baseFrame.GetFrame(frameIndex);
            if (!tempFrame.IsActive) {
                for (int i = 0; i < 3; i++) {
                    if (tempFrame.GetWord(i) != null) {
                        playerInvenController.AddNewItem(tempFrame.GetWord(i));
                        tempFrame.SetWord(i, null);
                    }
                }
            }
        }
    }

    //전체 베이스 프레임 리셋
    private void resetBaseFrame() {
        resetNotActiveBaseFrame();
        baseFrame.SetBase(false);
        frameListManager.AddFrame(baseFrame);
    }
    #endregion
}
