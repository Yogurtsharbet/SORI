using UnityEngine;

public enum WordType {
    All, NOUN, VERB, ADJ
    //Null = 0

    //    , NOUN = 1 << 0
    //    , VERB = 1 << 1
    //    , ADJ = 1 << 2
    //    , isMovable = 1 << 3
    //    , isChangable = 1 << 4
    //    , isInteractive = 1 << 5
    //    , isBreakable = 1 << 6
    //    , isAnimal = 1 << 7
    //    , isLiving = 1 << 8

    //    , isPersist = 1 << 20
    //, All = int.MaxValue
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
    //DISAPPEAR,
}
//TODO: ENUM Key 삭제 후 String 관리



/// <summary>
/// 
/// 동사 속성을 단일 속성이 아니라 비트 + 비트로 속성을 설정하면
/// 있는 속성가지고도 여러가지 조합의 속성을 만들 수 있꼬
/// 예를들어서
/// wordtype newVerbType = typeA | typeB
/// 
/// 그러면 명사에서도 typeA 와 typeB를 다
/// 
/// 
/// 
/// 1. 선택가능한 상호작용인가?
///     선택가능 : 선택불가 외 전부
///     선택불가 : HP, 소리, 하늘, 가방, 단어 등. 단일객체 또는 추상객체
///     
/// 2. 방향 지정 필요 상호작용인가?
///     방향지정필요 : 움직임 부류 상호작용
///     방향지정불필요 : 그 외 전부
///     
/// 3. 
/// 
/// 속성별 리스트를 만들고 ㅅ속성에 명사 ,동사를 집어넣ㅇ면?????????????????
/// 
/// 
/// </summary>

[System.Serializable]
public class WordDataStruct {
    public Word[] words;
    public WordKey[] isMovable;
};

public class WordData : MonoBehaviour {
    private WordDataStruct Data;
    public static Word[] words;

    private WordKey[] isMovable;

    private void Awake() {
        TextAsset dataFile = Resources.Load<TextAsset>("WordData");
        if(dataFile == null) {
            Debug.Log("Word Data File is not exist");
            Application.Quit();
            return;
        }

        Data = JsonUtility.FromJson<WordDataStruct>(dataFile.text);
        words = Data.words;
        isMovable = Data.isMovable;
    }
    public static string ToTag(Word word) {
        switch (word.Key) {
            case WordKey.SORI: return "Player";
            case WordKey.DOOR: return "Door";
                //case WordKey.TREE: return "Tree";
                //case WordKey.KEY: return "Key";
                //case WordKey.FLOOR: return "Floor";
                //case WordKey.BIRD: return "Bird";
                //case WordKey.HP: return "HP";
        }
        return string.Empty;
    }
}

    //= new Word[] {
    //
    //    Word.Create(WordKey.SORI, "소리", WordRank.NORMAL | WordRank.EPIC,
    //                WordType.NOUN | DefaultType | WordType.isLiving),
    //
    //    Word.Create(WordKey.DOOR, "문", WordRank.NORMAL | WordRank.EPIC,
    //                WordType.NOUN | DefaultType | WordType.isBreakable),
    //
        //Word.Create(WordKey.TREE, "나무", WordRank.NORMAL,
        //            WordType.NOUN | DefaultType | WordType.isBreakable),
        //
        //Word.Create(WordKey.KEY, "열쇠", WordRank.NORMAL | WordRank.EPIC,
        //            WordType.NOUN | DefaultType),
        //
        //Word.Create(WordKey.FLOOR, "바닥", WordRank.NORMAL | WordRank.EPIC,
        //            WordType.NOUN | DefaultType | WordType.isBreakable),
        //
        //Word.Create(WordKey.BIRD, "새", WordRank.NORMAL,
        //            WordType.NOUN | DefaultType | WordType.isAnimal | WordType.isLiving),
        //
        //Word.Create(WordKey.HP, "체력", WordRank.NORMAL | WordRank.EPIC,
        //            WordType.NOUN | WordType.isChangable | WordType.isInteractive),
        //
        //Word.Create(WordKey.HP, "시간", WordRank.EPIC,
        //            WordType.NOUN | WordType.isInteractive),
    //
    //Word.Create(WordKey.MOVE, "움직인다", WordRank.EPIC | WordRank.LEGEND,
    //            WordType.VERB | WordType.isMovable),
    //
//
//dictonry<WordKey, WordKey[] > dic 
//    public WordKey[] isMovable = { WordKey.DOOR , WordKey.MOVE , 넘어진다 날아간다 };
//public WordKey[] isSelecatble = { };
//
//    
//    
//    
//    
//    
//    
//    
//    
//    
//    
//    
//    
//}
//
//