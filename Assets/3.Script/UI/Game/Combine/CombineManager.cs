using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [UI] ���� - ���� ���� �Ŵ���. ���� â�� �� ���� Ʋ
public class CombineManager : MonoBehaviour {
    private CombineSlotController[] combineSlotControllers;
    private SentencesManager sentencesManager;
    private DialogController dialogController;

    //TODO: �ϳ��� �������� �ٲٱ�
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

        //TODO: �ϳ��� �������� �ٲٱ�
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

        if (selectedFrame.IsActive) {       //����Ʋ�� ������ ���ִ� ����
            canCombine = false;             //������ �Ϸ�� ����
            for (int i = 0; i < selectedFrame.BlankCount; i++)
                SetSlotWords(i, selectedFrame.GetWord(i));
        }
        else {
            canCombine = true;              //�����ϱ⸦ ������ �ϴ� ����
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

    //���� ����
    public void CombineSubmit() {
        if (selectedFrame == null) return;
        string dialogContents = string.Empty;

        for (int i = 0; i < selectedFrame.BlankCount; i++)
            selectedFrame.SetWord(i, combineSlotControllers[i].SlotWord);

        if(selectedFrame.CheckSentenceValidity()) {
            sentencesManager.SetSlotSentence(selectKey, selectedFrame);
            selectKey = -1; //TODO: ESC�� ���ƿ� �� �����Ƿ� selectKey�� ����Ǿ�� �� �ʿ� ����
            for (int i = 0; i < selectedFrame.BlankCount; i++)
                SetSlotWords(i, null);

            //TODO: ���ô�� �������� ��� SetTargetTag ���� �ʿ�
            selectControl.SetTargetTag(selectedFrame.wordA.Tag);
            CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);
            combineContainer.CloseCombineField();
            halfInvenContainer.CloseCombineInven();
        }
        else 
            dialogContents = "������ ���� �� �� �����ϴ�\n�ܾ Ȯ�����ּ���.";

        if(dialogContents != string.Empty) 
            dialogController.OpenDialog(dialogContents, DialogType.FAIL);
    }

    public void Activate(GameObject target, GameObject indicator) {
        selectedFrame.Activate(target, indicator);
    }
}
