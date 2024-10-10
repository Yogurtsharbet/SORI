using System.Linq;
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

    private Frame tempFrame;
    public Frame TempFrame => tempFrame;

    //조합 가능여부
    private bool canCombine;
    public bool CanCombine => canCombine;

    private FrameListManager frameListManager;
    private SubmitButtonController submitButton;
    private PlayerInvenController playerInvenController;
    private CombineContainer combineContainer;

    private WordFunction wordFunction;

    private GameObject[] frameByType = new GameObject[4];
    private List<(int, CombineSlotController)> combineSlotControllers = new List<(int, CombineSlotController)>();

    private void Awake() {
        frameListManager = FindObjectOfType<FrameListManager>();
        submitButton = FindObjectOfType<SubmitButtonController>();
        playerInvenController = FindObjectOfType<PlayerInvenController>();
        combineContainer = FindObjectOfType<CombineContainer>();
        wordFunction = GameManager.Instance.wordFunction;

        for (int i = 0; i < 4; i++) {
            frameByType[i] = gameObject.transform.GetChild(i).gameObject;
            CombineSlotController[] tempSlots = frameByType[i].GetComponentsInChildren<CombineSlotController>();
            for (int j = 0; j < tempSlots.Length; j++) {
                combineSlotControllers.Add((i, tempSlots[j]));
            }
        }
    }

    private void Start() {
        CloseCombineSlot();
    }

    private void OnEnable() {
        if (tempFrame != null) {
            baseFrame = tempFrame;
            frameByType[((int)baseFrame.Type - 1)].SetActive(true);
            setActiveFrameData(baseFrame);
        }
    }

    private void OnDisable() {
        for (int i = 0; i < 4; i++)
            frameByType[i].SetActive(false);
    }

    public void OpenBaseFrameSlot(Frame frame) {
        //이미 baseFrame이 open 되어있는 상태
        if (baseFrameOpen) {
            resetBaseFrame();
            for (int i = 0; i < frameByType.Length; i++) {
                frameByType[i].SetActive(false);
            }
        }
        else {
            gameObject.SetActive(true);
            baseFrameOpen = true;
        }

        frame.SetBase(true);
        baseFrame = frame;
        frameByType[((int)baseFrame.Type - 1)].SetActive(true);

        if (baseFrame.IsActive) {       //문장틀에 문장이 들어가있는 상태
            canCombine = false;             //조합이 완료된 상태
            submitButton.ButtonToRemove();
            setActiveFrameData(baseFrame);
        }
        else {
            canCombine = true;              //조합하기를 눌러야 하는 상태
            submitButton.ButtonToSubmit();
        }
        submitButton.CheckSubmitInteractable();
    }

    public void CancelCombineTemp() {
        gameObject.SetActive(false);
    }

    #region 드래그 상호작용 및 데이터 getter, setter

    private void setActiveFrameData(Frame baseFrame) {
        List<CombineSlotController> tempSlots = new List<CombineSlotController>();
        for (int i = 0; i < combineSlotControllers.Count; i++) {
            if (combineSlotControllers[i].Item1 == ((int)baseFrame.Type - 1)) {
                tempSlots.Add(combineSlotControllers[i].Item2);
            }
        }

        if (baseFrame.Type != FrameType.NotA) {
            int wordCount = 0;
            for (int i = 0; i < baseFrame.CountOfFrame(); i++) {
                if (baseFrame.GetWord(i) != null) {
                    for (int j = 0; j < tempSlots.Count; j++) {
                        if (tempSlots[j].SlotIndex == i && !tempSlots[j].IsSub) {
                            tempSlots[j].OpenWord(baseFrame.GetWord(i));
                            wordCount++;
                        }
                    }
                }
            }

            if (wordCount < baseFrame.CountOfFrame()) {
                for (int i = 0; i < baseFrame.CountOfFrame(); i++) {
                    Frame _tempFrame = baseFrame.GetFrame(i);
                    if (_tempFrame == null) continue;

                    foreach (var slot in tempSlots) {
                        if (slot.SlotIndex != i || slot.IsSub) continue;

                        slot.OpenFrame(_tempFrame.Type);

                        for (int k = 0; k < _tempFrame.CountOfFrame(); k++) {
                            Word word = _tempFrame.GetWord(k);
                            if (word == null) continue;

                            foreach (var subSlot in tempSlots) {
                                if (subSlot.SlotIndex == k && subSlot.IsSub && subSlot.ParentType == _tempFrame.Type) {
                                    subSlot.OpenWord(word);
                                }
                            }
                        }
                    }
                }
            }
        }
        else {
            for (int i = 0; i < baseFrame.CountOfFrame(); i++) {
                if (baseFrame.GetWord(i) != null) {
                    for (int j = 0; j < tempSlots.Count; j++) {
                        if (tempSlots[j].SlotIndex == i && !tempSlots[j].IsSub) {
                            tempSlots[j].OpenWord(baseFrame.GetWord(i));
                        }
                    }
                }
            }
        }
    }

    public void SetSubFrame(int index, Frame frame) {
        if (baseFrame.GetWord(index) != null) {
            playerInvenController.AddNewItem(baseFrame.GetWord(index));
            baseFrame.SetWord(index, null);
        }
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

    public void FrameToList(Frame frame) {
        frameListManager.AddFrame(frame);
        frameListManager.UpdateSlotData(frameListManager.PreviousValue);
    }

    public void ResetTempFrame() {
        tempFrame = null;
    }

    public void TempResetAndAddList() {
        for (int i = 0; i < tempFrame.CountOfFrame(); i++) {
            if (tempFrame.GetFrame(i) != null) {
                Frame frame = tempFrame.GetFrame(i);
                for (int j = 0; j < frame.CountOfFrame(); j++) {
                    frame.SetWord(i, null);
                }
                frameListManager.AddFrame(frame);
                tempFrame.SetFrame(i, null);
            }

            if (tempFrame.GetWord(i) != null) {
                baseFrame.SetWord(i, null);
            }
        }
        FrameToList(tempFrame);
        tempFrame = null;
    }

    #endregion

    //문장 조합
    public void CombineSubmit() {
        if (canCombine) {
            //조합
            if (FrameValidity.Check(baseFrame)) {
                wordFunction.frameRank = baseFrame.Rank;

                int i;
                for (i = 0; i < FrameValidity.GetCommonWord(0).keys.Count; i++) {
                    Word eachWord = Word.GetWord(FrameValidity.GetCommonWord(0).keys[i]);
                    if (!WordData.wordProperty["UNSELECT"].Contains(eachWord.Tag)) break;
                }
                if (i < FrameValidity.GetCommonWord(0).keys.Count) {
                    tempFrame = baseFrame;
                    (FindObjectOfType<GameManager>().gameState as State_Combine).OnCombineSubmit();
                }
                else FrameActivate.Activate(null);

                //선택 view에서 frame 임시 저장
                combineContainer.CloseCombineField();
            }
        }
        else {
            //프레임 비우기
            for (int i = 0; i < baseFrame.CountOfFrame(); i++) {
                if (baseFrame.GetFrame(i) != null) {
                    Frame frame = baseFrame.GetFrame(i);
                    for (int j = 0; j < frame.CountOfFrame(); j++) {
                        frame.SetWord(i, null);
                    }
                    frameListManager.AddFrame(frame);
                    baseFrame.SetFrame(i, null);
                }
                if (baseFrame.GetWord(i) != null) {
                    baseFrame.SetWord(i, null);
                }
            }
            baseFrameOpen = false;
            baseFrame.SetActive(false);
            baseFrame.SetBase(false);
            frameListManager.AddFrame(baseFrame);
            baseFrame = null;
            slotAllClose();
            submitButton.ButtonToSubmit();
            submitButton.CheckSubmitInteractable();
            gameObject.SetActive(false);
        }
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
            Frame _tempFrame = baseFrame.GetFrame(frameIndex);
            if (!_tempFrame.IsActive) {
                for (int i = 0; i < 3; i++) {
                    if (_tempFrame.GetWord(i) != null) {
                        playerInvenController.AddNewItem(_tempFrame.GetWord(i));
                        _tempFrame.SetWord(i, null);
                    }
                }
            }
        }
    }

    //전체 베이스 프레임 리셋
    private void resetBaseFrame() {
        resetNotActiveBaseFrame();
        baseFrame.SetBase(false);
        slotAllClose();
        frameListManager.AddFrame(baseFrame);
    }

    //프레임 조합 -> 사용처리
    public void TempFrameResetWordUse() {
        for (int i = 0; i < tempFrame.CountOfFrame(); i++) {
            if (tempFrame.GetFrame(i) != null) {
                Frame frame = tempFrame.GetFrame(i);
                for (int j = 0; j < frame.CountOfFrame(); j++) {
                    frame.SetWord(i, null);
                }
                frameListManager.AddFrame(frame);
                tempFrame.SetFrame(i, null);
            }
            if (tempFrame.GetWord(i) != null) {
                tempFrame.SetWord(i, null);
            }
        }
        tempFrame.SetActive(false);
        tempFrame.SetBase(false);
        FrameToList(tempFrame);
        tempFrame = null;
    }

    private void slotAllClose() {
        for (int i = 0; i < combineSlotControllers.Count; i++) {
            combineSlotControllers[i].Item2.CloseAllSlot();
        }
    }
    #endregion
}
