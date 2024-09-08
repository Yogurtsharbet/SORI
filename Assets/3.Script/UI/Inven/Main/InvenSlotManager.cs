public class InvenSlotManager : CommonInvenSlotManager {
    private SynthesisManager synthesisManager;

    private void Awake() {
        synthesisManager = FindObjectOfType<SynthesisManager>();
    }

    //�ռ�â index�� �ִ� �ܾ�� �κ� �ܾ� ����Ī
    public void SwitchingInvenToSynthesisSlot(int invenIndex, int index) {
        Word tempWord = playerInvenController.GetWordIndex(invenIndex);
        playerInvenController.RemoveItemIndex(invenIndex);
        playerInvenController.AddItem(synthesisManager.GetSlotWordFromIndex(index), invenIndex);
        synthesisManager.SlotItemChangeFromIndex(index, tempWord, invenIndex);
    }

    //�ܾ� ������ �Ҹ� �ϱ��� �ൿ ��ҷ� �κ� �ʱ�ȭ
    public void TempInvenToPlayerInven() {
        playerInvenController.SetInvenReset(TempInven);
    }

    //������ index���Կ� �ܾ� �߰�
    public void SetWordAdd(int invenIndex, int index) {
        synthesisManager.SlotItemChangeFromIndex(index, playerInvenController.GetWordIndex(invenIndex), invenIndex);
        playerInvenController.RemoveItemIndex(invenIndex);
    }
}
