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

    private void OnEnable() {
        UpdateInvenInvoke();        
    }

    private void testData() {
        for (int i = 0; i < 12; i++) {
            if (i == 11) inven[i] = Word.GetWord(WordData.Search("ROCK").Key);
            else if (i == 10) inven[i] = Word.GetWord(WordData.Search("MOVE").Key);
            else inven[i] = Word.GetWord();
        }
        UpdateInvenInvoke();
    }

    private void initInven() {
        invenOpenCount = 12;
        for (int i = 0; i < invenOpenCount; i++) {
            AddInvenSlot();
        }
    }

    /// <summary>
    /// 인벤토리 칸 추가
    /// </summary>
    public void AddInvenSlot() {
        inven.Add(null);
    }

    /// <summary>
    /// 인벤토리 칸 삭제
    /// 1. 제일 마지막에 있는 단어 옮기고 인벤 슬롯 삭제
    /// 2. 빈 슬롯이 없을 때, 선택한 인덱스의 단어 삭제
    /// </summary>
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
                //player가 삭제할 단어 index 선택 창 열기
                return;
            }
        }
        UpdateInvenInvoke();
    }

    /// <summary>
    /// 빈칸에 새 단어 추가
    /// </summary>
    /// <param name="newWord">Word</param>
    // TODO: 빈칸이 없을때 처리 추가필요
    public void AddNewItem(Word newWord) {
        for (int i = 0; i < invenOpenCount; i++) {
            if (inven[i] == null) {
                inven[i] = newWord;
                break;
            }
        }
        UpdateInvenInvoke();
    }

    /// <summary>
    /// 특정 인덱스에 새 단어 추가
    /// </summary>
    /// <param name="newWord">Word</param>
    /// <param name="index">int</param>
    public void AddItem(Word newWord, int index) {
        inven[index] = newWord;
        UpdateInvenInvoke();
    }

    /// <summary>
    /// 특정 인덱스의 단어와 현재 선택한 단어와 인덱스 스위칭
    /// </summary>
    /// <param name="thisIndex">int</param>
    /// <param name="targetIndex">int</param>
    public void SwitchingItem(int thisIndex, int targetIndex) {
        if (inven[targetIndex] == null) {
            inven[targetIndex] = inven[thisIndex];
            inven[thisIndex] = null;
        }
        else {
            Word tempWord = inven[targetIndex];
            inven[targetIndex] = inven[thisIndex];
            inven[thisIndex] = tempWord;
        }
        UpdateInvenInvoke();
    }

    /// <summary>
    /// 특정 인덱스의 단어 삭제
    /// </summary>
    /// <param name="index">int</param>
    public void RemoveItemIndex(int index) {
        inven[index] = null;
        UpdateInvenInvoke();
    }

    /// <summary>
    /// 특정 인덱스의 단어 
    /// </summary>
    /// <param name="index"></param>
    /// <returns>Word</returns>
    public Word GetWordIndex(int index) {
        return inven[index];
    }

    public void SetInvenReset(List<Word> list) {
        List<Word> newList = new List<Word>();
        for (int i = 0; i < list.Count; i++) {
            newList.Add(list[i]);
        }
        inven = newList;
        UpdateInvenInvoke();
    }

    public int ExistInvenCount() {
        int count = 0;
        for (int i = 0; i < inven.Count; i++) {
            if (inven[i] != null) {
                count++;
            }
        }
        return count;
    }
}
