using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [Sentence] 문장 - 조합 문장 클래스
public class Sentence {
    private (Word, Word) sentenceWords;   //조합 단어

    public (Word, Word) SentenceWords => sentenceWords;

    public Sentence(Word wordItem1, Word wordItem2) {
        sentenceWords = (wordItem1, wordItem2);
    }

}
