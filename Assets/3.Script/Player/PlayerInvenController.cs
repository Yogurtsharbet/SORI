using System.Collections.Generic;
using UnityEngine;

public class PlayerInvenController : MonoBehaviour {
    private List<Word> inven = new List<Word>();
    public List<Word> Inven { get { return inven; } }

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
    private void Start() {
        testData();
    }

    private void testData() {
        inven[0] = new Word("부서지다", WordRank.NORMAL);
        inven[1] = new Word("상자", WordRank.NORMAL);
        inven[2] = new Word("새", WordRank.NORMAL);
        inven[3] = new Word("이동속도", WordRank.NORMAL);
        inven[4] = new Word("탄다", WordRank.NORMAL);
        UpdateInvenInvoke();
    }

    private void initInven() {
        invenOpenCount = 12;
        for (int i = 0; i < invenOpenCount; i++) {
            AddInvenSlot();
        }
    }

    //인벤토리 칸 추가
    public void AddInvenSlot() {
        inven.Add(null);
    }

    //인벤토리 칸 삭제
    // 1. 제일 마지막에 있는 단어 옮기고 인벤 슬롯 삭제
    // 2. 빈 슬롯이 없을 때, 선택한 인덱스의 단어 삭제
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
                // TODO: 플레이어 INPUT 받아서 플레이어가 선택한 단어 삭제
                //player가 삭제할 단어 index 선택 창 열기
                return;
            }
        }
        UpdateInvenInvoke();
    }

    //새 단어 추가
    public void AddNewItem(Word newWord) {
        for (int i = 0; i < invenOpenCount; i++) {
            if (inven[i] == null) {
                inven[i] = newWord;
                return;
            }
        }
        UpdateInvenInvoke();
    }

    //특정 인덱스에 새 단어 추가
    public void AddItemIndex(Word newWord, int index) {
        inven[index] = newWord;
        UpdateInvenInvoke();
    }

    //특정 인덱스의 단어와 현재 선택한 단어와 인덱스 스위칭
    public void SwitchingItem(int thisIndex, int targetIndex) {
        Word tempWord = inven[targetIndex];
        inven[targetIndex] = inven[thisIndex];
        inven[thisIndex] = tempWord;
        UpdateInvenInvoke();
    }

    //특정 인덱스의 단어 삭제
    public void RemoveItemIndex(int index) {
        inven[index] = null;
        UpdateInvenInvoke();
    }

    //특정 인덱스의 단어 getter
    public Word GetWordIndex(int index) {
        return inven[index];
    }

    public void SetInvenReset(List<Word> list) {
        inven = list;
    }

}
