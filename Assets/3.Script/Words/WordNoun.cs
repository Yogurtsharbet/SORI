/*****************************************************
 *  Word
 *  └   WordNoun
 *      └   WordLiving
 *      └   WordObject
 *          └   WordEnviroment
 *  └   WordVerb
 *  └   WordAdj
 *  
 * 
 * 
 * 
 *****************************************************/

public class WordNoun : Word {              // 명사
    protected bool isMovable;       // 이동가능
    protected bool isInteractive;   // 형용사가능
    protected bool isChangable;     // 변환가능

    public WordNoun(string name, WordRank[] availableRank)
        : base(name, availableRank) {
        _type = WordType.NOUN;

        isMovable = true;
        isInteractive = true;
        isChangable = true;
    }
}

public class WordObject : WordNoun {        // 물건
    protected bool isBreakable;     // 파괴가능

    public WordObject(string name, WordRank[] availableRank)
        : base(name, availableRank) {
        isBreakable = true;
    }
}

#region Word Object
public class __Key : WordObject {
    public __Key()
        : base(name: "열쇠",
        availableRank: new WordRank[] { WordRank.NORMAL, WordRank.EPIC }) {
        isBreakable = false;
    }
}
#endregion

public class WordEnviroment : WordObject {
    public WordEnviroment(string name, WordRank[] availableRank)
        : base(name, availableRank) {

    }
}

#region Word Enviroment
public class __Door : WordEnviroment {
    public __Door()
        : base(name: "문",
        availableRank: new WordRank[] { WordRank.NORMAL, WordRank.EPIC }) {
    }
}

public class __Time : WordEnviroment {
    public __Time()
        : base(name: "시간",
        availableRank: new WordRank[] { WordRank.EPIC }) {
        isChangable = false;
        isMovable = false;
        isBreakable = false;
    }
}

public class __Floor : WordEnviroment {
    public __Floor()
        : base(name: "바닥",
        availableRank: new WordRank[] { WordRank.NORMAL, WordRank.EPIC }) {
    }
}
#endregion

public class WordLiving : WordNoun {        // 생물

    public WordLiving(string name, WordRank[] availableRank)
        : base(name, availableRank) {

    }
}

#region Word Living
public class __Bird : WordLiving {
    public __Bird()
        : base(name: "새",
        availableRank: new WordRank[] { WordRank.NORMAL }) {
    }
}
#endregion

public class WordPlayer : WordLiving {      // 플레이어
    public WordPlayer(string name, WordRank[] availableRank)
        : base(name, availableRank) {

    }
}

#region Word Player
public class __Sori : WordPlayer {
    public __Sori()
        : base(name: "소리",
        availableRank: new WordRank[] { WordRank.NORMAL, WordRank.EPIC }) {
    }
}

public class __HP : WordPlayer {
    public __HP()
        : base(name: "체력",
        availableRank: new WordRank[] { WordRank.NORMAL, WordRank.EPIC }) {
        isMovable = false;
    }
}

#endregion 

