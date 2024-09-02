using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordManager : MonoBehaviour {
    private Word[] words;

    private void Awake() {
        words = new Word[] {
            new __Door(),
            new __Key(),
            new __Sori(),
            new __Floor(),
            new __Bird(),
            new __HP(),
            new __Time()
        };
    }

    public Word GetRandomWord() {
        return words[Random.Range(0, words.Length)];
    }
}
