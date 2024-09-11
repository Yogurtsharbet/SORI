using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [UI] ���� - ���� ���� �Ŵ���. ���� â�� �� ���� Ʋ
public class CombineSlotManager : MonoBehaviour {
    private CombineSlotController[] combineSlotControllers;
    private SentencesManager sentencesManager;
    private DialogController dialogController;

    private int selectKey;
    private bool canCombine;

    public bool CanCombine => canCombine;

    private void Awake() {
        combineSlotControllers = GetComponentsInChildren<CombineSlotController>();
        sentencesManager = FindObjectOfType<SentencesManager>();
        dialogController = FindObjectOfType<DialogController>();
    }

    private void Start() {
        CloseCombineSlot();
        combineSlotControllers[0].CloseSlot();
        combineSlotControllers[1].CloseSlot();
    }

    public void OpenCombineSlot(int key, Sentence sentence) {
        gameObject.SetActive(true);
        selectKey = key;
        if (sentence != null) {
            canCombine = false;
            SetSlotWords(0, sentence.SentenceWords.Item1);
            SetSlotWords(1, sentence.SentenceWords.Item2);
        }
        else {
            canCombine = true;
        }
    }

    public void CloseCombineSlot() {
        for(int i = 0; i < combineSlotControllers.Length; i++) {
            SetSlotWords(i, null);
        }
        gameObject.SetActive(false);
    }


    public RectTransform GetSlotRectTransform(int num) {
        return combineSlotControllers[num].GetComponent<RectTransform>();
    }

    public void SetSlotWords(int index, Word word) {
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
        if (combineSlotControllers[0].SlotWord != null && combineSlotControllers[1].SlotWord != null) {
            //TODO: ���� ���� ���� �߰�
            Sentence newSentence = new Sentence(combineSlotControllers[0].SlotWord, combineSlotControllers[1].SlotWord);
            sentencesManager.SetSlotSentence(selectKey, newSentence);
            SetSlotWords(0, null);
            SetSlotWords(1, null);
            selectKey = -1;
        }
        else {
            string dialogContents = "������ ���� �� �� �����ϴ�\n�ܾ Ȯ�����ּ���.";
            dialogController.OpenDialog(dialogContents, DialogType.FAIL);
        }
    }
}
