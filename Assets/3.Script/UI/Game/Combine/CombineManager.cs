using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [UI] 조합 - 조합 매니저. 조합 창의 한 문장 틀
public class CombineManager :MonoBehaviour {
    private FrameListContainer sentencesManager;
    private SubmitButtonController submitButton;

    //TODO: 하나로 합쳐지면 바꾸기
    private CombineContainer combineContainer;
    private HalfInvenContainer halfInvenContainer;

    private SelectControl selectControl;

    private Frame selectedFrame;
    private bool canCombine;

    private GameObject[] frameObjects = new GameObject[4];

    public bool CanCombine => canCombine;

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
        gameObject.SetActive(true);

        selectedFrame = frame;
        activeFrameType((int)frame.Type - 1);

        if (selectedFrame.IsActive) {       //문장틀에 문장이 들어가있는 상태
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
        selectedFrame = null;
        gameObject.SetActive(false);
    }

    public void CancelCombineTemp() {
        gameObject.SetActive(false);
    }

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
        if (selectedFrame == null) return;
        string dialogContents = string.Empty;

        if (selectedFrame.CheckSentenceValidity()) {
            //sentencesManager.SetSlotSentence(selectKey, selectedFrame);
            //selectKey = -1; //TODO: ESC로 돌아올 수 있으므로 selectKey가 저장되어야 할 필요 있음

            //TODO: 선택대상 여러개일 경우 SetTargetTag 수정 필요
            selectControl.SetTargetTag(selectedFrame.wordA.ToTag());
            CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);
            combineContainer.CloseCombineField();
            halfInvenContainer.CloseCombineInven();

            //TODO: 확정하더라도 선택해서 사용하는 경우에는 선택한 후에 소모해야함
        }
        else
            dialogContents = "문장을 조합 할 수 없습니다\n단어를 확인해주세요.";

        if (dialogContents != string.Empty)
            DialogManager.Instance.OpenDefaultDialog(dialogContents, DialogType.FAIL);
    }

    public void Activate(GameObject target, GameObject indicator) {
        selectedFrame.Activate(target, indicator);
    }
}
