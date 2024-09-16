using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [UI] ���� - ���� �Ŵ���. ���� â�� �� ���� Ʋ
public class CombineManager :MonoBehaviour {
    private FrameListContainer sentencesManager;
    private SubmitButtonController submitButton;

    //TODO: �ϳ��� �������� �ٲٱ�
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

        //TODO: �ϳ��� �������� �ٲٱ�
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

        if (selectedFrame.IsActive) {       //����Ʋ�� ������ ���ִ� ����
            canCombine = false;             //������ �Ϸ�� ����
            submitButton.ButtonToRemove();
        }
        else {
            canCombine = true;              //�����ϱ⸦ ������ �ϴ� ����
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
        //TODO: ���Կ� �ִ� ���� ������� ������ -> ���
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

    //���� ����
    public void CombineSubmit() {
        if (selectedFrame == null) return;
        string dialogContents = string.Empty;

        if (selectedFrame.CheckSentenceValidity()) {
            //sentencesManager.SetSlotSentence(selectKey, selectedFrame);
            //selectKey = -1; //TODO: ESC�� ���ƿ� �� �����Ƿ� selectKey�� ����Ǿ�� �� �ʿ� ����

            //TODO: ���ô�� �������� ��� SetTargetTag ���� �ʿ�
            selectControl.SetTargetTag(selectedFrame.wordA.ToTag());
            CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);
            combineContainer.CloseCombineField();
            halfInvenContainer.CloseCombineInven();

            //TODO: Ȯ���ϴ��� �����ؼ� ����ϴ� ��쿡�� ������ �Ŀ� �Ҹ��ؾ���
        }
        else
            dialogContents = "������ ���� �� �� �����ϴ�\n�ܾ Ȯ�����ּ���.";

        if (dialogContents != string.Empty)
            DialogManager.Instance.OpenDefaultDialog(dialogContents, DialogType.FAIL);
    }

    public void Activate(GameObject target, GameObject indicator) {
        selectedFrame.Activate(target, indicator);
    }
}
