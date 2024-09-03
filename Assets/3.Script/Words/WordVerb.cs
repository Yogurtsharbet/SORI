using UnityEngine;
using DG.Tweening;

public class WordVerb : Word {
    public WordVerb(string name, WordRank rank)
        : base(name, rank) {
        _type |= WordType.VERB;
    }

}

#region Word Verb
public class __Move : WordVerb {
    public __Move()
        : base(name: "움직인다", rank: WordRank.EPIC | WordRank.LEGEND ) {
        _type |= WordType.isMovable;
    }

    public void Function(Transform origin, Vector3 target) {
        origin.DOMove(target, 3f, true);
    }
}
#endregion
