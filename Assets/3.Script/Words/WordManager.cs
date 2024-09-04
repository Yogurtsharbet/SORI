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
            new __Time(),
            new __Move()
        };
    }

    public Word GetRandomWord() {
        return new Word(words[Random.Range(0, words.Length)]);
    }

    private void Update() {
        __Door door = new __Door();
        if (door is __Door()) ;
        else if (door is __Key()) ;
            
        return;
    }
}
