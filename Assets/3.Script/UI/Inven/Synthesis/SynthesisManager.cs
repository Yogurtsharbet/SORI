using UnityEngine;

public class SynthesisManager : MonoBehaviour {
    private SynthesisSlotController[] slotControllers;
    private InvenSlotManager invenSlotManager;
    private PlayerInvenController playerInvenController;
    private DialogController dialogController;

    private void Awake() {
        playerInvenController = FindObjectOfType<PlayerInvenController>();
        slotControllers = FindObjectsOfType<SynthesisSlotController>();
        invenSlotManager = FindObjectOfType<InvenSlotManager>();
        dialogController = FindObjectOfType<DialogController>();
    }

    public RectTransform GetSlotRectTransform(int num) {
        return slotControllers[num].GetComponent<RectTransform>();
    }

    public bool GetExistFromIndex(int index) {
        return slotControllers[index].GetWordExist();
    }

    //해당 슬롯 index의 단어 return
    public Word GetSlotWordFromIndex(int index) {
        return slotControllers[index].GetSlotWord();
    }

    //슬롯 index에 새 단어 추가
    public void SlotItemChangeFromIndex(int index, Word word, int invenIndex) {
        slotControllers[index].RemoveSlotWord();
        slotControllers[index].SetSlotWord(word, invenIndex);
    }

    //단어 전체 삭제후 원래 인벤으로 이동
    public void ResetAllSlot() {
        for (int i = 0; i < slotControllers.Length; i++) {
            slotControllers[i].RemoveSlotWord();
        }
        invenSlotManager.TempInvenToPlayerInven();
    }

    //합성창 슬롯의 단어 인벤으로 이동 
    public void ResetSlotIndex(int index) {
        if (slotControllers[index].GetSlotWord() != null) {
            playerInvenController.AddItem(slotControllers[index].GetSlotWord(), slotControllers[index].GetWordOriginInvenIndex());
            slotControllers[index].RemoveSlotWord();
        }
    }

    public void GetNewWord() {
        if (CheckCanSynthesis()) {
            //합성하기 버튼 눌렀을때, 랜덤으로 새 단어 얻음
            Word newWord = Word.GetWord();
            slotControllers[3].SetSlotWord(newWord);
            //얻은단어 2초 있다가 인벤으로 이동
        }
        else {
            string dialogContents = "단어 합성을 할 수 없습니다\n합성 슬롯을 확인해주세요.";
            dialogController.OpenDialog(dialogContents, DialogType.FAIL);
        }
    }

    private bool CheckCanSynthesis() {
        if (slotControllers[0].GetWordExist() && slotControllers[1].GetWordExist() && slotControllers[2].GetWordExist()) {
            return true;
        }
        else {
            return false;
        }
    }
}
