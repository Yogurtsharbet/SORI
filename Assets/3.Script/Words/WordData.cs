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

        , isPersist = 1 << 20
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
    KEY,
    FLOOR,
    BIRD,
    HP,
    //TIME,
    MOVE,
}

class WordData {
    private const WordType DefaultType = 
        WordType.isMovable | WordType.isChangable | WordType.isInteractive;

    public static readonly Word[] words = new Word[] {
        
        Word.Create(WordKey.SORI, "소리", WordRank.NORMAL | WordRank.EPIC,
                    WordType.NOUN | DefaultType | WordType.isLiving),

        Word.Create(WordKey.DOOR, "문", WordRank.NORMAL | WordRank.EPIC,
                    WordType.NOUN | DefaultType | WordType.isBreakable),

        Word.Create(WordKey.TREE, "나무", WordRank.NORMAL,
                    WordType.NOUN | DefaultType | WordType.isBreakable),

        Word.Create(WordKey.KEY, "열쇠", WordRank.NORMAL | WordRank.EPIC,
                    WordType.NOUN | DefaultType),

        Word.Create(WordKey.FLOOR, "바닥", WordRank.NORMAL | WordRank.EPIC,
                    WordType.NOUN | DefaultType | WordType.isBreakable),

        Word.Create(WordKey.BIRD, "새", WordRank.NORMAL,
                    WordType.NOUN | DefaultType | WordType.isAnimal | WordType.isLiving),

        Word.Create(WordKey.HP, "체력", WordRank.NORMAL | WordRank.EPIC,
                    WordType.NOUN | WordType.isChangable | WordType.isInteractive),

        Word.Create(WordKey.HP, "시간", WordRank.EPIC,
                    WordType.NOUN | WordType.isInteractive),

        Word.Create(WordKey.MOVE, "움직인다", WordRank.EPIC | WordRank.LEGEND,
                    WordType.VERB | WordType.isMovable),
    };

    public static string ToTag(Word word) {
        switch (word.Key) {
            case WordKey.SORI: return "Player";
            case WordKey.DOOR: return "Door";
            case WordKey.TREE: return "Tree";
            case WordKey.KEY: return "Key";
            case WordKey.FLOOR: return "Floor";
            case WordKey.BIRD: return "Bird";
            case WordKey.HP: return "HP";
        }
        return string.Empty;
    }
}

