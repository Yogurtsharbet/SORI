using System.Collections;
using UnityEngine;

// [UI] 합성 - 합성 전체 매니저
public class SynthesisManager : MonoBehaviour {
    private SynthesisSlotController[] slotControllers;
    private InvenSlotManager invenSlotManager;
    private PlayerInvenController playerInvenController;

    private void Awake() {
        playerInvenController = FindObjectOfType<PlayerInvenController>();
        slotControllers = GetComponentsInChildren<SynthesisSlotController>();
        invenSlotManager = FindObjectOfType<InvenSlotManager>();
    }

    public RectTransform GetSlotRectTransform(int num) {
        return slotControllers[num].GetComponent<RectTransform>();
    }

    /// <summary>
    /// 조합창에 단어가 존재하는지 여부
    /// </summary>
    /// <param name="index"></param>
    /// <returns>bool</returns>
    public bool GetExistFromIndex(int index) {
        return slotControllers[index].GetWordExist();
    }

    /// <summary>
    /// 단어 전체 삭제, 인벤에 넣어줌
    /// </summary>
    public void ResetAllSlot() {
        for (int i = 0; i < slotControllers.Length; i++) {
            playerInvenController.AddNewItem(slotControllers[i].GetSlotWord());
            slotControllers[i].RemoveSlotWord();
        }
    }

    #region 단어 합성
    private bool CheckCanSynthesis() {
        if (slotControllers[0].GetWordExist() && slotControllers[1].GetWordExist() && slotControllers[2].GetWordExist()) {
            return true;
        }
        else {
            return false;
        }
    }

    public void GetNewWord() {
        if (CheckCanSynthesis()) {
            //합성하기 버튼 눌렀을때, 랜덤으로 새 단어 얻음
            Word newWord = Word.GetWord();
            slotControllers[3].SetSlotWord(newWord);
            //얻은단어 2초 있다가 인벤으로 이동
            StartCoroutine(removeDelay());
        }
        else {
            string dialogContents = "단어 합성을 할 수 없습니다\n합성 슬롯을 확인해주세요.";
            DialogManager.Instance.OpenDefaultDialog(dialogContents, DialogType.FAIL);
        }
    }
    private IEnumerator removeDelay() {
        yield return new WaitForSeconds(2f);

        playerInvenController.AddNewItem(slotControllers[3].GetSlotWord());

        for (int i = 0; i < slotControllers.Length; i++) {
            slotControllers[i].RemoveSlotWord();
        }
    }
    #endregion

    /// <summary>
    /// 인벤 단어, 조합창 단어 스위칭
    /// </summary>
    /// <param name="invenIndex"></param>
    /// <param name="syntheIndex"></param>
    public void WordSwitchingToSynthesis(int invenIndex, int syntheIndex) {
        if (slotControllers[syntheIndex].GetWordExist()) {
            Word word = slotControllers[syntheIndex].GetSlotWord();
            slotControllers[syntheIndex].SetSlotWord(playerInvenController.GetWordIndex(invenIndex));
            playerInvenController.RemoveItemIndex(invenIndex);
            playerInvenController.AddItem(word, invenIndex);
        }
        else {
            slotControllers[syntheIndex].SetSlotWord(playerInvenController.GetWordIndex(invenIndex));
            playerInvenController.RemoveItemIndex(invenIndex);
        }
    }

    //해당 슬롯 index의 단어 return
    public Word GetSlotWordFromIndex(int index) {
        return slotControllers[index].GetSlotWord();
    }

    //슬롯 index에 새 단어 추가
    public void SlotItemChangeFromIndex(int index, Word word) {
        slotControllers[index].RemoveSlotWord();
        slotControllers[index].SetSlotWord(word);
    }

    public void SlotWordDelete(int index) {
        Word word = slotControllers[index].GetSlotWord();
        playerInvenController.AddNewItem(word);
        slotControllers[index].RemoveSlotWord();
    }
}
