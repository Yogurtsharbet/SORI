using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [UI] 조합 - 조합 슬롯 매니저. 조합 창의 한 문장 틀
public class CombineManager : MonoBehaviour {
    private CombineSlotController[] combineSlotControllers;
    private SentencesManager sentencesManager;
    private DialogController dialogController;

    //TODO: 하나로 합쳐지면 바꾸기
    private CombineContainer combineContainer;
    private HalfInvenContainer halfInvenContainer;

    private SelectControl selectControl;

    private Frame selectedFrame;
    private int selectKey;
    private bool canCombine;

    public bool CanCombine => canCombine;

    private void Awake() {
        combineSlotControllers = GetComponentsInChildren<CombineSlotController>();
        sentencesManager = FindObjectOfType<SentencesManager>();
        dialogController = FindObjectOfType<DialogController>();
        selectControl = FindObjectOfType<SelectControl>();

        //TODO: 하나로 합쳐지면 바꾸기
        combineContainer = FindObjectOfType<CombineContainer>();
        halfInvenContainer = FindObjectOfType<HalfInvenContainer>();
    }

    private void Start() {
        CloseCombineSlot();
        combineSlotControllers[0].CloseSlot();
        combineSlotControllers[1].CloseSlot();
    }

    public void OpenCombineSlot(int key, Frame frame) {
        gameObject.SetActive(true);

        selectKey = key;
        selectedFrame = frame;

        if (selectedFrame.IsActive) {       //문장틀에 문장이 들어가있는 상태
            canCombine = false;             //조합이 완료된 상태
            for (int i = 0; i < selectedFrame.BlankCount; i++)
                SetSlotWords(i, selectedFrame.GetWord(i));
        }
        else {
            canCombine = true;              //조합하기를 눌러야 하는 상태
        }
    }

    public void CloseCombineSlot() {
        gameObject.SetActive(false);
    }

    public RectTransform GetSlotRectTransform(int num) {
        return combineSlotControllers[num].GetComponent<RectTransform>();
    }

    public void SetSlotWords(int index, Word word) {
        selectedFrame.SetWord(index, word);
        combineSlotControllers[index].SetSlotWord(word);
    }

    public bool CheckIsSlotExist(int index) {
        if (combineSlotControllers[index].SlotWord != null) {
            return true;
        }
        else {
            return false;
        }
    }

    public Word SwitchingWordToInven(int index, Word word) {
        Word slotWord = combineSlotControllers[index].SlotWord;
        combineSlotControllers[index].SetSlotWord(word);
        return slotWord;
    }

    //문장 조합
    public void CombineSubmit() {
        if (selectedFrame == null) return;
        string dialogContents = string.Empty;

        for (int i = 0; i < selectedFrame.BlankCount; i++)
            selectedFrame.SetWord(i, combineSlotControllers[i].SlotWord);

        if(selectedFrame.CheckSentenceValidity()) {
            sentencesManager.SetSlotSentence(selectKey, selectedFrame);
            selectKey = -1; //TODO: ESC로 돌아올 수 있으므로 selectKey가 저장되어야 할 필요 있음
            for (int i = 0; i < selectedFrame.BlankCount; i++)
                SetSlotWords(i, null);

            //TODO: 선택대상 여러개일 경우 SetTargetTag 수정 필요
            selectControl.SetTargetTag(selectedFrame.wordA.Tag);
            CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);
            combineContainer.CloseCombineField();
            halfInvenContainer.CloseCombineInven();
        }
        else 
            dialogContents = "문장을 조합 할 수 없습니다\n단어를 확인해주세요.";

        if(dialogContents != string.Empty) 
            dialogController.OpenDialog(dialogContents, DialogType.FAIL);
    }

    public void Activate(GameObject target, GameObject indicator) {
        selectedFrame.Activate(target, indicator);
    }
}
