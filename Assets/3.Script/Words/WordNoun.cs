public class WordNoun : Word {              // ���
    public WordNoun(string name, WordRank rank)
        : base(name, rank) {
        _type |= WordType.NOUN;
    }
}

public class __Key : WordNoun {
    public __Key()
        : base(name: "����", rank: WordRank.NORMAL | WordRank.EPIC) {
        _type |=
            WordType.isMovable | WordType.isChangable | WordType.isInteractive;
    }
}

public class __Door : WordNoun {
    public __Door()
        : base(name: "��", rank: WordRank.NORMAL | WordRank.EPIC) {
        _type |=
            WordType.isMovable | WordType.isChangable |
            WordType.isInteractive | WordType.isBreakable;
    }
}

public class __Time : WordNoun {
    public __Time()
        : base(name: "�ð�", rank: WordRank.EPIC) {
        _type |= WordType.isInteractive;
    }
}

public class __Floor : WordNoun {
    public __Floor()
        : base(name: "�ٴ�", rank: WordRank.NORMAL | WordRank.EPIC) {
        _type |=
            WordType.isMovable | WordType.isChangable |
            WordType.isInteractive | WordType.isBreakable;
    }
}

public class __Bird : WordNoun {
    public __Bird()
        : base(name: "��", rank: WordRank.NORMAL) {
        _type |=
            WordType.isMovable | WordType.isChangable |
            WordType.isInteractive | WordType.isAnimal | WordType.isLiving;
    }
}

public class __Sori : WordNoun {
    public __Sori()
        : base(name: "�Ҹ�", rank: WordRank.NORMAL | WordRank.EPIC) {
        _type |=
            WordType.isMovable | WordType.isChangable |
            WordType.isInteractive | WordType.isLiving;
    }
}

public class __HP : WordNoun {
    public __HP()
        : base(name: "ü��", rank: WordRank.NORMAL | WordRank.EPIC) {
        _type |=
            WordType.isChangable | WordType.isInteractive;
    }
}


