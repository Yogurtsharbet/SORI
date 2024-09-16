#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class WordDataEditor : EditorWindow {
    private string filePath = "Resource/WordData";

    private Word[] Data;
    private WordType wordType;

    private int selectedWord;
    private string[] wordNameList;

    private bool showDropdown;
    private Vector2 scrollPosition;


    [MenuItem("WordData/Edit WordData")]
    private static void Init() {
        WordDataEditor Editor = (WordDataEditor)GetWindow(typeof(WordDataEditor));
        Editor.Show();
    }

    private void Awake() {
        Load();
    }

    private void OnGUI() {
        EditorGUILayout.Space(20f);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("WORD DATA", EditorStyles.whiteLargeLabel,
            GUILayout.Width(EditorStyles.whiteLargeLabel.CalcSize(new GUIContent("WORD DATA")).x));
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10f);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Data Path", GUILayout.Width(EditorGUIUtility.labelWidth));
        EditorGUILayout.SelectableLabel(filePath, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(30f);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Word Type", GUILayout.Width(EditorGUIUtility.labelWidth));
        wordType = (WordType)EditorGUILayout.EnumPopup(wordType);
        if (GUI.changed) Load(wordType);
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10f);

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("SELECT WORD", GUILayout.Width(EditorGUIUtility.labelWidth));
        if (GUILayout.Button(wordNameList.Length > 0 ? wordNameList[selectedWord] : "",
            EditorStyles.popup, GUILayout.Width(EditorGUIUtility.labelWidth))) {
            showDropdown = !showDropdown;
        }
        Rect buttonRect = GUILayoutUtility.GetLastRect();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (showDropdown) {
            Rect dropdownArea = new Rect(buttonRect.x, buttonRect.y + buttonRect.height, buttonRect.width, 100);
            DrawDropdown(dropdownArea);
        }

        EditorGUILayout.Space(showDropdown ? 130f : 20f);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Data Path", GUILayout.Width(EditorGUIUtility.labelWidth));
        EditorGUILayout.SelectableLabel(filePath, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        EditorGUILayout.EndHorizontal();

    }

    private void Load(WordType type = WordType.All) {
        TextAsset dataFile = Resources.Load<TextAsset>("WordData");
        if (dataFile == null) {
            Debug.Log("Word Data File is not exist");
            Application.Quit();
            return;
        }
        Data = JsonUtility.FromJson<WordDataStruct>(dataFile.text).words;

        if (type != WordType.All) {
            List<Word> tempData = new List<Word>();
            foreach (var each in Data)
                if (each.Type == type) tempData.Add(each);
            Data = tempData.ToArray();
        }

        wordNameList = new string[Data.Length];
        for (int i = 0; i < wordNameList.Length; i++)
            wordNameList[i] = Data[i].Name;
    }

    private void DrawDropdown(Rect area) {
        float dropdownHeight = Mathf.Min(wordNameList.Length * EditorGUIUtility.singleLineHeight, 100);
        Rect scrollRect = new Rect(area.x, area.y, area.width, dropdownHeight);

        scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition, new Rect(0, 0, area.width - 16, wordNameList.Length * EditorGUIUtility.singleLineHeight));
        for (int i = 0; i < wordNameList.Length; i++) {
            if (GUI.Button(new Rect(0, i * EditorGUIUtility.singleLineHeight, area.width - 16, EditorGUIUtility.singleLineHeight), wordNameList[i])) {
                selectedWord = i;
                showDropdown = false;
            }
        }
        GUI.EndScrollView();
    }
}
#endif
