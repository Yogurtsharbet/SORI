public class InvenSlotManager : CommonInvenSlotManager {
    private SynthesisManager synthesisManager;

    private void Awake() {
        synthesisManager = FindObjectOfType<SynthesisManager>();
    }

    //합성창 index에 있는 단어와 인벤 단어 스위칭
    public void SwitchingInvenToSynthesisSlot(int invenIndex, int index) {
        Word tempWord = playerInvenController.GetWordIndex(invenIndex);
        playerInvenController.RemoveItemIndex(invenIndex);
        playerInvenController.AddItem(synthesisManager.GetSlotWordFromIndex(index), invenIndex);
        synthesisManager.SlotItemChangeFromIndex(index, tempWord, invenIndex);
    }

    //단어 아이템 소모 하기전 행동 취소로 인벤 초기화
    public void TempInvenToPlayerInven() {
        playerInvenController.SetInvenReset(TempInven);
    }

    //선택한 index슬롯에 단어 추가
    public void SetWordAdd(int invenIndex, int index) {
        synthesisManager.SlotItemChangeFromIndex(index, playerInvenController.GetWordIndex(invenIndex), invenIndex);
        playerInvenController.RemoveItemIndex(invenIndex);
    }
}
