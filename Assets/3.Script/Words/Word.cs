using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WordType {
    NOUN = 0,
    ADJ,
    VERB
}

public enum WordRank {
    NORMAL = 0,
    EPIC,
    LEGEND,
    UNIQUE,
    SPECIAL
}

public class Word : MonoBehaviour {
    private WordType _type;
    public WordType Type { get { return _type; } }

    private string _name;

    public string Name { get { return _name; } }

    private WordRank _rank;
    public WordRank Rank { get { return _rank; } }
}
