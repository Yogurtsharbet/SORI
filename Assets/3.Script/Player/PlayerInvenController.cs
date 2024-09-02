using System.Collections.Generic;
using UnityEngine;

public class PlayerInvenController : MonoBehaviour {
    private List<Word> inven = new List<Word>();
    private List<Word> Inven { get { return inven; } }

    private int invenOpenCount;
    public int InvenOpenCount { get { return invenOpenCount; } }

    public delegate void OnInvenChanged(List<Word> inven);
    public event OnInvenChanged InvenChanged;

    public void UpdateInvenInvoke() {
        InvenChanged?.Invoke(inven);
    }

    private void Awake() {
        initInven();
    }

    private void initInven() {
        invenOpenCount  = 12;
        for (int i = 0; i < invenOpenCount; i++) {
            AddInvenSlot(); 
        }
    }

    //�κ��丮 ĭ �߰�
    public void AddInvenSlot() {
        inven.Add(null);
    }

    //�κ��丮 ĭ ����
    // 1. ���� �������� �ִ� �ܾ� �ű�� �κ� ���� ����
    // 2. �� ������ ���� ��, ������ �ε����� �ܾ� ����
    private void getRemoveInvenIndex() {
        if (inven[invenOpenCount - 1] == null) {
            inven.RemoveAt(invenOpenCount - 1);
            return;
        }
        else {
            int emptyIndex = -1;
            for (int j = invenOpenCount - 2; j >= 0; j--) {
                if (inven[j] == null) {
                    emptyIndex = j;
                    break;
                }
            }
            if (emptyIndex != -1) {
                inven[emptyIndex] = inven[invenOpenCount - 1];
                inven.RemoveAt(invenOpenCount - 1);
            }
            else {
                //inven.RemoveAt(GetRemoveIndex());
                // TODO: �÷��̾� INPUT �޾Ƽ� �÷��̾ ������ �ܾ� ����
                //player�� ������ �ܾ� index ���� â ����
                return;
            }
        }
    }

    //�� �ܾ� �߰�
    public void AddNewItem(Word newWord) {
        for (int i = 0; i < invenOpenCount; i++) {
            if (inven[i] == null) {
                inven[i] = newWord;
                return;
            }
        }
    }

    //Ư�� �ε����� �� �ܾ� �߰�
    public void AddItemIndex(Word newWord, int index) {
        inven[index] = newWord;
    }

    //Ư�� �ε����� �ܾ�� ���� ������ �ܾ�� �ε��� ����Ī
    public void SwitchingItem(int thisIndex, int targetIndex) {
        Word tempWord = inven[targetIndex];
        inven[targetIndex] = inven[thisIndex];
        inven[thisIndex] = tempWord;
    }

    //Ư�� �ε����� �ܾ� ����
    public void RemoveItemIndex(int index) {
        inven[index] = null;
    }

    //Ư�� �ε����� �ܾ� getter
    public Word GetWordIndex(int index) {
        return inven[index];
    }

}
