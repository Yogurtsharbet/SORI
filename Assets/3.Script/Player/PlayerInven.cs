using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInven : MonoBehaviour {
    private List<Word> inven = new List<Word>();
    private int invenCount;
    public int InvenCount {  get { return invenCount; } }

    private void Awake() {
        initInven();
    }

    private void initInven() {
        invenCount = 12;
        for (int i = 0; i < invenCount; i++) {
            inven.Add(null);
        }
    }

    //�κ��丮 ĭ �߰�
    private void addInvenSlot() {
        inven.Add(null);
    }

    //�κ��丮 ĭ ����
    // 1. ���� �������� �ִ� �ܾ� �ű�� �κ� ���� ����
    // 2. �� ������ ���� ��, ������ �ε����� �ܾ� ����
    private void getRemoveInvenIndex() {
        if (inven[invenCount - 1] == null) {
            inven.RemoveAt(invenCount - 1);
            return;
        }
        else {
            int emptyIndex = -1;
            for (int j = invenCount - 2; j >= 0; j--) {
                if (inven[j] == null) {
                    emptyIndex = j;
                    break;
                }
            }
            if (emptyIndex != -1) {
                inven[emptyIndex] = inven[invenCount - 1];
                inven.RemoveAt(invenCount - 1);
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
        for (int i = 0; i < invenCount; i++) {
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
