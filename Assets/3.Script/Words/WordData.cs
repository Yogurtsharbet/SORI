using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using WordKey = System.UInt16;
using WordTag = System.String;

public enum WordType {
    All, NOUN, VERB, ADJ
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

[System.Serializable]
public class WordDataStruct {
    //TODO: 신규 동사 property 추가 시 반드시 우선 작성
    public Word[] words;
    public WordTag[] isUnselectable;
    public WordTag[] isMovable;
    public WordTag[] isChangable;
};

public class WordData : MonoBehaviour {
    private WordDataStruct Data;
    public static Word[] words;

    public static Dictionary<WordTag, WordTag[]> wordProperty;
    private WordTag[] isUnselectable;
    private WordTag[] isMovable;
    private WordTag[] isChangable;

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
        isUnselectable = Data.isUnselectable;
        isMovable = Data.isMovable;
        isChangable = Data.isChangable;

        //TODO: 신규 동사 property 추가 시 반드시 우선 작성
        wordProperty = new Dictionary<WordTag, WordTag[]>();
        wordProperty.Add("UNSELECT", isUnselectable);
        wordProperty.Add("MOVE", isMovable);
        wordProperty.Add("CHANGE", isChangable);

        #region DEBUGGING
        // FOR DEBUGGING : Make a new JSON
        //string filePath = "Resources/WordData";
        //Data = new WordDataStruct();
        //words = new Word[3];
        //words[0] = Word.Create(0, "Player", "소리", WordRank.EPIC, WordType.NOUN);
        //words[1] = Word.Create(1, "Door", "문", WordRank.NORMAL | WordRank.EPIC, WordType.NOUN);
        //words[2] = Word.Create(2, "Move", "움직이다", WordRank.EPIC, WordType.VERB);
        //isMovable = new WordKey[] { 0, 1, 2 };
        //Data.words = words;
        //Data.isMovable = isMovable;

        //string jsonData = JsonUtility.ToJson(Data, true);
        //string path = Path.Combine(Application.dataPath, filePath + ".json");
        //try {
        //    File.WriteAllText(path, jsonData);
        //    AssetDatabase.Refresh();
        //}
        //catch (IOException e) {
        //    Debug.LogError($"Error saving data: {e.Message}");
        //}
        #endregion
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
//     키 리스트
//}
//
//