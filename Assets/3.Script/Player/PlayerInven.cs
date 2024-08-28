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

    //인벤토리 칸 추가
    private void addInvenSlot() {
        inven.Add(null);
    }

    //인벤토리 칸 삭제
    // 1. 제일 마지막에 있는 단어 옮기고 인벤 슬롯 삭제
    // 2. 빈 슬롯이 없을 때, 선택한 인덱스의 단어 삭제
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
                // TODO: 플레이어 INPUT 받아서 플레이어가 선택한 단어 삭제
                //player가 삭제할 단어 index 선택 창 열기
                return;
            }
        }
    }

    //새 단어 추가
    public void AddNewItem(Word newWord) {
        for (int i = 0; i < invenCount; i++) {
            if (inven[i] == null) {
                inven[i] = newWord;
                return;
            }
        }
    }

    //특정 인덱스에 새 단어 추가
    public void AddItemIndex(Word newWord, int index) {
        inven[index] = newWord;
    }

    //특정 인덱스의 단어와 현재 선택한 단어와 인덱스 스위칭
    public void SwitchingItem(int thisIndex, int targetIndex) {
        Word tempWord = inven[targetIndex];
        inven[targetIndex] = inven[thisIndex];
        inven[thisIndex] = tempWord;
    }

    //특정 인덱스의 단어 삭제
    public void RemoveItemIndex(int index) {
        inven[index] = null;
    }

    //특정 인덱스의 단어 getter
    public Word GetWordIndex(int index) {
        return inven[index];
    }

    
}
