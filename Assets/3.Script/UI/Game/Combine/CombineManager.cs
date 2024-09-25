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

    private Frame tempFrame;

    //조합 가능여부
    private bool canCombine;
    public bool CanCombine => canCombine;

    private FrameListManager frameListManager;
    private SubmitButtonController submitButton;
    private PlayerInvenController playerInvenController;
    private CombineContainer combineContainer;

    private SelectControl selectControl;

    private GameObject[] frameByType = new GameObject[4];
    private List<(int, CombineSlotController)> combineSlotControllers = new List<(int, CombineSlotController)>();

    private void Awake() {
        frameListManager = FindObjectOfType<FrameListManager>();
        selectControl = FindObjectOfType<SelectControl>();
        submitButton = FindObjectOfType<SubmitButtonController>();

        combineContainer = FindObjectOfType<CombineContainer>();
        playerInvenController = FindObjectOfType<PlayerInvenController>();

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
            //TODO: 하위 슬롯 끄기
        }
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
                    Frame tempFrame = baseFrame.GetFrame(i);
                    if (tempFrame == null) continue;

                    foreach (var slot in tempSlots) {
                        if (slot.SlotIndex != i || slot.IsSub) continue;

                        slot.OpenFrame(tempFrame.Type);

                        for (int k = 0; k < tempFrame.CountOfFrame(); k++) {
                            Word word = tempFrame.GetWord(k);
                            if (word == null) continue;

                            foreach (var subSlot in tempSlots) {
                                if (subSlot.SlotIndex == k && subSlot.IsSub && subSlot.ParentType == tempFrame.Type) {
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

    #endregion

    //문장 조합
    public void CombineSubmit() {
        if (canCombine) {
            string dialogContents = string.Empty;
            if (baseFrame == null) dialogContents = "프레임 없음";
            else if (FrameValidity.Check(baseFrame)) {

                bool isUnselectable = false;
                foreach (var eachKeyA in FrameValidity.GetCommonWord(0).keys) {
                    Word eachWordA = Word.GetWord(eachKeyA);
                    if (WordData.wordProperty["UNSELECT"].Contains(eachWordA.Tag))
                        isUnselectable = true;
                }

                //TODO: 사용되면 소모, 조합되고 영구적 -> LIST로 넣기
                if (isUnselectable) FrameActivate.Activate();   //사용되는 timming
                else {
                    CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);
                }

                //선택 view에서 frame 임시 저장
                tempFrame = baseFrame;
                combineContainer.CloseCombineField();
            }
            //TODO: Dialog -> FrameValidity 에서 띄워주는 게 좋을 듯?
            if (dialogContents != string.Empty)
                DialogManager.Instance.OpenDefaultDialog(dialogContents, DialogType.FAIL);
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
        slotAllClose();
        frameListManager.AddFrame(baseFrame);
    }

    private void slotAllClose() {
        for (int i = 0; i < combineSlotControllers.Count; i++) {
            combineSlotControllers[i].Item2.CloseAllSlot();
        }
    }
    #endregion
}
