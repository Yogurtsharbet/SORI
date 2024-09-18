#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class WordDataEditor : EditorWindow {
    private string filePath = "Resources/WordData";

    private WordDataStruct Data;
    private List<Word> words;
    private WordType wordType;

    private List<int> wordIndexList;
    private bool isSelected;
    private bool isNewAdd;
    private int selectedWord;

    private bool showDropdown;
    private Vector2 scrollPosition;

    private string _keyOrig;
    private string _nameOrig;
    private WordType _typeOrig;
    private WordRank _rankOrig;

    private string _key;
    private string _name;
    private WordType _type;
    private bool[] rankAvailable;

    private string statusLog;
    private bool isDelete;
    private bool isModified;

    [MenuItem("WordData/Edit WordData")]
    private static void Init() {
        WordDataEditor Editor = (WordDataEditor)GetWindow(typeof(WordDataEditor));
        Editor.minSize = new Vector2(500f, 600f);
        Editor.Show();
    }

    private void Awake() {
        wordIndexList = new List<int>();
        rankAvailable = new bool[5];
        Load();
    }

    private void OnGUI() {
        EditorGUILayout.Space(20f);

        #region �⺻ GUI region
        // GUI Ÿ��Ʋ
        EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("WORD DATA", EditorStyles.whiteLargeLabel, GUILayout.Width(100f));
        GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10f);

        // Resources ���� ��� ��� �ʵ�
        EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Data Path", GUILayout.Width(EditorGUIUtility.labelWidth));
        EditorGUILayout.SelectableLabel(filePath, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(30f);

        // WordType �� �˻� ���� ��Ӵٿ�
        EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Word Type", GUILayout.Width(EditorGUIUtility.labelWidth));
        wordType = (WordType)EditorGUILayout.EnumPopup(wordType);
        if (GUI.changed) Load(wordType);
        GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10f);

        // Edit ��� �ܾ� ���� ��Ӵٿ�
        EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("SELECT WORD", GUILayout.Width(EditorGUIUtility.labelWidth));
        if (GUILayout.Button(wordIndexList.Count > 0 ? words[wordIndexList[selectedWord]].Name : "",
            EditorStyles.popup, GUILayout.Width(EditorGUIUtility.labelWidth)))
            if (wordIndexList.Count > 0) showDropdown = !showDropdown;
        Rect buttonRect = GUILayoutUtility.GetLastRect();
        Rect dropdownArea = new Rect(buttonRect.x, buttonRect.y + buttonRect.height, buttonRect.width, 100);
        GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();

        // ��Ӵٿ� ��ư Ŭ���� �޴� ���
        if (showDropdown) DrawDropdown(dropdownArea);
        else {
            EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
            if (GUILayout.Button("Add New...", GUILayout.Width(150f))) OnAddNewButtonClicked();
            GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.Space(showDropdown ?
            Mathf.Min(wordIndexList.Count * EditorGUIUtility.singleLineHeight + 50, 150) : 50f);
        #endregion

        #region �ܾ� ���� ���� region
        // Select Word ���� �ܾ� ���� ���� â
        if (isSelected) {
            isModified = false;

            // Key �Է� �ʵ�
            EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
            GUI.contentColor = _key != _keyOrig ? Color.yellow : Color.white;
            EditorGUILayout.LabelField("KEY", GUILayout.Width(150f));
            _key = EditorGUILayout.TextField(_key, GUILayout.Width(150f)); GUI.contentColor = Color.white;
            GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();

            // Name �Է� �ʵ�
            EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
            GUI.contentColor = _name != _nameOrig ? Color.yellow : Color.white;
            EditorGUILayout.LabelField("Name", GUILayout.Width(150f));
            _name = EditorGUILayout.TextField(_name, GUILayout.Width(150f)); GUI.contentColor = Color.white;
            GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();

            // Rank �Է� üũ�ڽ�
            for (int i = 0; i < 5; i++) {
                EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
                isModified |= (((int)_rankOrig & (1 << i)) != 0) != rankAvailable[i];
                GUI.contentColor = ((((int)_rankOrig & (1 << i)) != 0) != rankAvailable[i]) ? Color.yellow : Color.white;
                EditorGUILayout.LabelField(((WordRank)(1 << i)).ToString() + " Rank", GUILayout.Width(150f)); GUI.contentColor = Color.white;
                rankAvailable[i] = EditorGUILayout.Toggle(rankAvailable[i], GUILayout.Width(150f));
                GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
            }

            // Type �Է� ��Ӵٿ�
            EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
            GUI.contentColor = _type != _typeOrig ? Color.yellow : Color.white;
            EditorGUILayout.LabelField("Word Type", GUILayout.Width(150f));
            _type = (WordType)EditorGUILayout.EnumPopup(_type, GUILayout.Width(150f)); GUI.contentColor = Color.white;
            GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(20f);

            if (isNewAdd) 
                isModified &= _key != _keyOrig & _name != _nameOrig & _type != _typeOrig;
            else isModified |= _key != _keyOrig | _name != _nameOrig | _type != _typeOrig;

            // ���� & ���� ��ư
            EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
            GUI.enabled = isModified; GUI.contentColor = Color.green;
            if (GUILayout.Button(isNewAdd ? "Add" : "Modify", GUILayout.Width(100f), GUILayout.Height(30f))) OnSaveButtonClicked();

            GUI.enabled = !isModified; GUI.contentColor = Color.red;
            if (!isNewAdd)
                if (GUILayout.Button("DELETE", GUILayout.Width(100f), GUILayout.Height(30f))) OnDeleteButtonClicked();
            GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(50f);

        }
        #endregion

        EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        GUI.contentColor = Color.yellow;
        EditorGUILayout.LabelField(statusLog, EditorStyles.wordWrappedLabel);
        GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();

        if (isDelete) {
            statusLog = "   WARNING!\nAre you sure?";
            EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
            GUI.contentColor = Color.white;
            if (GUILayout.Button("Yes", GUILayout.Width(100f), GUILayout.Height(25f))) Delete();
            if (GUILayout.Button("NO", GUILayout.Width(100f), GUILayout.Height(25f))) { isDelete = false; statusLog = string.Empty; }
            GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
        }

    }

    private void Save() {
        string jsonData = JsonUtility.ToJson(Data, true);
        string path = Path.Combine(Application.dataPath, filePath + ".json");
        try {
            File.WriteAllText(path, jsonData);
            AssetDatabase.Refresh();
        }
        catch (IOException e) {
            Debug.LogError($"Error saving data: {e.Message}");
        }
    }

    private void Load(WordType type = WordType.All) {
        TextAsset dataFile = Resources.Load<TextAsset>("WordData");
        if (dataFile == null) {
            Debug.Log("Word Data File is not exist");
            Application.Quit();
            return;
        }
        Data = JsonUtility.FromJson<WordDataStruct>(dataFile.text);
        words = Data.words.ToList();

        wordIndexList.Clear();
        for (int i = 0; i < words.Count; i++) {
            if (type == WordType.All || words[i].Type == type)
                wordIndexList.Add(i);
        }

        isSelected = false;
        showDropdown = false;
        selectedWord = 0;
        isNewAdd = false;
        isDelete = false;
        statusLog = string.Empty;
    }

    private void DrawDropdown(Rect area) {
        float dropdownHeight = Mathf.Min(wordIndexList.Count * EditorGUIUtility.singleLineHeight, 100);
        Rect scrollRect = new Rect(area.x, area.y, area.width, dropdownHeight);
        scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition,
            new Rect(0, 0, area.width - 16, wordIndexList.Count * EditorGUIUtility.singleLineHeight));

        for (int i = 0; i < wordIndexList.Count; i++) {
            if (GUI.Button(new Rect(0, i * EditorGUIUtility.singleLineHeight, area.width - 16, EditorGUIUtility.singleLineHeight * 1.2f),
                words[wordIndexList[i]].Name)) {
                selectedWord = i;
                showDropdown = false;
                isSelected = true;
                isNewAdd = false;
                isDelete = false;
                statusLog = string.Empty;

                _keyOrig = _key = words[wordIndexList[i]].Key.ToString();
                _nameOrig = _name = words[wordIndexList[i]].Name;
                _typeOrig = _type = words[wordIndexList[i]].Type;
                _rankOrig = words[wordIndexList[i]].Rank;

                for (int j = 0; j < 5; j++)
                    rankAvailable[j] = ((int)_rankOrig & (1 << j)) != 0;
            }
        }
        GUI.EndScrollView();
    }

    private WordRank RankAvailableToWordRank() {
        int wordRank = 0;
        for(int i = 0; i < rankAvailable.Length; i++) {
            if (rankAvailable[i]) wordRank = wordRank | (1 << i);
        }
        return (WordRank)wordRank;
    }

    private void OnAddNewButtonClicked() {
        isSelected = true;
        isNewAdd = true;
        isDelete = false;
        statusLog = string.Empty;
        selectedWord = 0;

        _keyOrig = _key = string.Empty;
        _nameOrig = _name = string.Empty;
        _typeOrig = _type = WordType.All;
        _rankOrig = 0;
        for (int j = 0; j < 5; j++) {
            rankAvailable[j] = ((int)_rankOrig & (1 << j)) != 0;
        }
    }

    private void OnSaveButtonClicked() {
        // ADD, Modify ���
        if (isNewAdd) {
            words.Add(Word.Create(
                Convert<WordKey>.ToEnum(_key), _name, RankAvailableToWordRank(), _type));
        }
        else {
            // enum �ҰŸ� string���� ������ �ʿ䵵 ����
            words[wordIndexList[selectedWord]]._key = Convert<WordKey>.ToEnum(_key);
            words[wordIndexList[selectedWord]]._name = _name;
            words[wordIndexList[selectedWord]]._type = _type;
            words[wordIndexList[selectedWord]]._rank = RankAvailableToWordRank();
        }
        Data.words = words.ToArray();
        Save();
        Load();
        statusLog = "Modify Complete";
    }

    private void OnDeleteButtonClicked() {
        isDelete = true;
    }

    private void Delete() {
        if (words[wordIndexList[selectedWord]]._key.ToString() != _key)
            statusLog = "Remove Failed! Data Crashed.";
        else {
            words.RemoveAt(wordIndexList[selectedWord]);
            Data.words = words.ToArray();
            Save();
            Load();
            statusLog = "Remove Complete";
        }
    }
}
#endif

public class Convert<T> where T : Enum {
    private static Dictionary<string, T> _cachedDictionary;

    static Convert() {
        _cachedDictionary = new Dictionary<string, T>();
        foreach (T enumValue in Enum.GetValues(typeof(T))) 
            _cachedDictionary[enumValue.ToString()] = enumValue;
    }

    public static T ToEnum(string value) {
        if (_cachedDictionary.TryGetValue(value, out T result)) return result;
        throw new ArgumentException($"'{value}' is not a valid value for enum {typeof(T).Name}");
    }
}