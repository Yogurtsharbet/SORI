/*****************************************************
 *  Word
 *  ��   WordNoun
 *      ��   WordLiving
 *      ��   WordObject
 *          ��   WordEnviroment
 *  ��   WordVerb
 *  ��   WordAdj
 *  
 * 
 * 
 * 
 *****************************************************/

public class WordNoun : Word {              // ���
    protected bool isMovable;       // �̵�����
    protected bool isInteractive;   // ����簡��
    protected bool isChangable;     // ��ȯ����

    public WordNoun(string name, WordRank[] availableRank)
        : base(name, availableRank) {
        _type = WordType.NOUN;

        isMovable = true;
        isInteractive = true;
        isChangable = true;
    }
}

public class WordObject : WordNoun {        // ����
    protected bool isBreakable;     // �ı�����

    public WordObject(string name, WordRank[] availableRank)
        : base(name, availableRank) {
        isBreakable = true;
    }
}

#region Word Object
public class __Key : WordObject {
    public __Key()
        : base(name: "����",
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
        : base(name: "��",
        availableRank: new WordRank[] { WordRank.NORMAL, WordRank.EPIC }) {
    }
}

public class __Time : WordEnviroment {
    public __Time()
        : base(name: "�ð�",
        availableRank: new WordRank[] { WordRank.EPIC }) {
        isChangable = false;
        isMovable = false;
        isBreakable = false;
    }
}

public class __Floor : WordEnviroment {
    public __Floor()
        : base(name: "�ٴ�",
        availableRank: new WordRank[] { WordRank.NORMAL, WordRank.EPIC }) {
    }
}
#endregion

public class WordLiving : WordNoun {        // ����

    public WordLiving(string name, WordRank[] availableRank)
        : base(name, availableRank) {

    }
}

#region Word Living
public class __Bird : WordLiving {
    public __Bird()
        : base(name: "��",
        availableRank: new WordRank[] { WordRank.NORMAL }) {
    }
}
#endregion

public class WordPlayer : WordLiving {      // �÷��̾�
    public WordPlayer(string name, WordRank[] availableRank)
        : base(name, availableRank) {

    }
}

#region Word Player
public class __Sori : WordPlayer {
    public __Sori()
        : base(name: "�Ҹ�",
        availableRank: new WordRank[] { WordRank.NORMAL, WordRank.EPIC }) {
    }
}

public class __HP : WordPlayer {
    public __HP()
        : base(name: "ü��",
        availableRank: new WordRank[] { WordRank.NORMAL, WordRank.EPIC }) {
        isMovable = false;
    }
}

#endregion 

