using UnityEngine;
using WordKey = System.UInt16;

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

//public enum WordKey {
//    _Random,
//    SORI,
//    DOOR,
//    TREE,
//    //KEY,
//    //FLOOR,
//    //BIRD,
//    //HP,
//    //TIME,
//    MOVE,
//    //DISAPPEAR,
//}

//TODO: ENUM Key 삭제 후 String 관리
// WordKey가 Enum일 경우
//      JSON 저장 불러오기 시에 상수로 저장됨
//      상수 - string 변환 시 박싱 언박싱 코스트
//      새로운 Key 추가 시 코드 수정 불가피
// WordKey가 string일 경우
//      Property 지정 시 string 사용하면 오타 찾기 어려움
//      Enum 상수 값 비교보다 string 비교가 속도 더 느릴 것
//      Key가 string이면 굳이 Key를 안쓰고 wordName으로 대체가능

[System.Serializable]
public class WordDataStruct {
    public Word[] words;
    public WordKey[] isMovable;
};

public class WordData : MonoBehaviour {
    private WordDataStruct Data;
    public static Word[] words;

    private WordKey[] isMovable;

    public const WordKey RandomKey = 0;

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

    //public static string ToTag(Word word) {
    //    switch (word.Key) {
    //        case WordKey.SORI: return "Player";
    //        case WordKey.DOOR: return "Door";
    //            //case WordKey.TREE: return "Tree";
    //            //case WordKey.KEY: return "Key";
    //            //case WordKey.FLOOR: return "Floor";
    //            //case WordKey.BIRD: return "Bird";
    //            //case WordKey.HP: return "HP";
    //    }
    //    return string.Empty;
    //}
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
//     키 리스트
//}
//
//