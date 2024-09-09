public enum WordType {
    Null = 0

        , NOUN = 1 << 0
        , VERB = 1 << 1
        , ADJ = 1 << 2
        , isMovable = 1 << 3
        , isChangable = 1 << 4
        , isInteractive = 1 << 5
        , isBreakable = 1 << 6
        , isAnimal = 1 << 7
        , isLiving = 1 << 8

    , All = int.MaxValue
}

public enum WordRank {
    _Random = 0

        , NORMAL = 1 << 0
        , EPIC = 1 << 1
        , LEGEND = 1 << 2
        , UNIQUE = 1 << 3
        , SPECIAL = 1 << 4

    , All = int.MaxValue
}

public enum WordKey {
    _Random,
    SORI,
    DOOR,
    TREE,
    //KEY,
    //FLOOR,
    //BIRD,
    //HP,
    //TIME,
    MOVE,
}

class WordData {
    public static readonly Word[] words = new Word[] {

        Word.Create(WordKey.SORI, "소리", WordRank.NORMAL | WordRank.EPIC,
                    WordType.NOUN | WordType.isMovable | WordType.isChangable |
                    WordType.isInteractive | WordType.isLiving),

        Word.Create(WordKey.DOOR, "문", WordRank.NORMAL | WordRank.EPIC,
                    WordType.NOUN | WordType.isMovable | WordType.isChangable |
                    WordType.isInteractive | WordType.isBreakable),

        Word.Create(WordKey.TREE, "나무", WordRank.NORMAL,
                    WordType.NOUN | WordType.isMovable | WordType.isChangable |
                    WordType.isInteractive | WordType.isBreakable),

        Word.Create(WordKey.MOVE, "움직인다", WordRank.EPIC | WordRank.LEGEND,
                    WordType.VERB | WordType.isMovable),
    };

    public static string ToTag(Word word) {
        switch (word.Key) {
            case WordKey.SORI: return "Player";
            case WordKey.DOOR: return "Door";
            case WordKey.TREE: return "Tree";
        }
        return string.Empty;
    }
}

