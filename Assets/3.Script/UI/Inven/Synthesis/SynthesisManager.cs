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

    //�ش� ���� index�� �ܾ� return
    public Word GetSlotWordFromIndex(int index) {
        return slotControllers[index].GetSlotWord();
    }

    //���� index�� �� �ܾ� �߰�
    public void SlotItemChangeFromIndex(int index, Word word, int invenIndex) {
        slotControllers[index].RemoveSlotWord();
        slotControllers[index].SetSlotWord(word, invenIndex);
    }

    //�ܾ� ��ü ������ ���� �κ����� �̵�
    public void ResetAllSlot() {
        for (int i = 0; i < slotControllers.Length; i++) {
            slotControllers[i].RemoveSlotWord();
        }
        invenSlotManager.TempInvenToPlayerInven();
    }

    //�ռ�â ������ �ܾ� �κ����� �̵� 
    public void ResetSlotIndex(int index) {
        if (slotControllers[index].GetSlotWord() != null) {
            playerInvenController.AddItem(slotControllers[index].GetSlotWord(), slotControllers[index].GetWordOriginInvenIndex());
            slotControllers[index].RemoveSlotWord();
        }
    }

    public void GetNewWord() {
        if (CheckCanSynthesis()) {
            //�ռ��ϱ� ��ư ��������, �������� �� �ܾ� ����
            Word newWord = Word.GetWord();
            slotControllers[3].SetSlotWord(newWord);
            //�����ܾ� 2�� �ִٰ� �κ����� �̵�
        }
        else {
            string dialogContents = "�ܾ� �ռ��� �� �� �����ϴ�\n�ռ� ������ Ȯ�����ּ���.";
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
