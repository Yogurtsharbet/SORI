using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [Sentence] ���� - ���� ���� Ŭ����
public class Sentence {
    private (Word, Word) sentenceWords;   //���� �ܾ�

    public (Word, Word) SentenceWords => sentenceWords;

    public Sentence(Word wordItem1, Word wordItem2) {
        sentenceWords = (wordItem1, wordItem2);
    }

}
