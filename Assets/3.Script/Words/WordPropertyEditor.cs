#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using WordKey = System.UInt16;
using WordTag = System.String;

public class WordPropertyEditor : EditorWindow {
    private string filePath = "Resources/WordData";
    private WordDataStruct Data;

    private string[] propertyList;
    private int property = 0;

    private Vector2 scrollPosition;
    private string statusLog;
    private bool isShowProperty;

    [MenuItem("WordData/Edit WordProperty")]
    private static void Init() {
        WordPropertyEditor Editor = (WordPropertyEditor)GetWindow(typeof(WordPropertyEditor));
        Editor.minSize = new Vector2(500f, 600f);
        Editor.Show();
    }

    private void Awake() {
        Load();
        Type wordDataType = typeof(WordDataStruct);
        FieldInfo[] fields = wordDataType.GetFields(BindingFlags.Public | BindingFlags.Instance);
        propertyList = Array.FindAll(fields, 
            field => field.FieldType == typeof(WordTag[])).Select(field => field.Name).ToArray();
    }

    private void OnGUI() {
        EditorGUILayout.Space(20f);

        // GUI 타이틀
        EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("WORD PROPERTY", EditorStyles.whiteLargeLabel, GUILayout.Width(120f));
        GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10f);

        // Resources 저장 경로 출력 필드
        EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Data Path", GUILayout.Width(EditorGUIUtility.labelWidth));
        EditorGUILayout.SelectableLabel(filePath, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(30f);

        // Property 항목 선택 필드
        EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("SELECT PROPERTY", GUILayout.Width(150f));
        property = EditorGUILayout.Popup(property, propertyList, GUILayout.Width(200f));
        if (GUI.changed) { isShowProperty = true; statusLog = string.Empty; }
        GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(20f);

        if (propertyList.Length > 0 && isShowProperty) {
            string selectedPropertyName = propertyList[property];
            FieldInfo selectedField = typeof(WordDataStruct).GetField(selectedPropertyName);

            if (selectedField != null) {
                WordTag[] selectedArray = (WordTag[])selectedField.GetValue(Data);

                if (selectedArray != null) {
                    EditorGUILayout.Space(20f);
                    EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
                    EditorGUILayout.SelectableLabel(selectedPropertyName, EditorStyles.toolbarSearchField, GUILayout.Width(200f));
                    GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();

                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(250f));
                    for (int i = 0; i < selectedArray.Length; i++) {
                        EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
                        EditorGUILayout.LabelField($"Target Word {i}", GUILayout.Width(150f));
                        WordTag tempTag = EditorGUILayout.TagField(selectedArray[i], GUILayout.Width(150f));
                        if (GUI.changed) {
                            int j;
                            for (j = 0; j < selectedArray.Length;j++) 
                                if (i != j && selectedArray[j] == tempTag) break;
                            if (j == selectedArray.Length) { 
                                selectedArray[i] = tempTag; 
                                statusLog = string.Empty; 
                            }
                            else statusLog = "Already existed tag!"; 
                        }
                        if (GUILayout.Button("X", GUILayout.Width(20))) {
                            selectedArray = selectedArray.Where((val, idx) => idx != i).ToArray();
                            selectedField.SetValue(Data, selectedArray);
                        }
                        GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndScrollView(); EditorGUILayout.Space(50f);

                    EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
                    GUI.contentColor = Color.yellow;
                    if (GUILayout.Button("Add Tag..", GUILayout.Width(120f), GUILayout.Height(30f))) {
                        Array.Resize(ref selectedArray, selectedArray.Length + 1);
                        selectedArray[selectedArray.Length - 1] = "";
                        selectedField.SetValue(Data, selectedArray);
                    }
                    GUI.contentColor = Color.green;
                    if (GUILayout.Button("SAVE", GUILayout.Width(120f), GUILayout.Height(30f))) {
                        Save();
                        statusLog = "Save Complete!";
                    }
                    GUI.contentColor = Color.red;
                    if (GUILayout.Button("Discard", GUILayout.Width(120f), GUILayout.Height(30f))) {
                        Load();
                        statusLog = string.Empty;
                    }
                    GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
                }
            }
        }

        EditorGUILayout.BeginHorizontal(); GUILayout.FlexibleSpace();
        GUI.contentColor = Color.yellow;
        EditorGUILayout.LabelField(statusLog, EditorStyles.wordWrappedLabel);
        GUILayout.FlexibleSpace(); EditorGUILayout.EndHorizontal();
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
        isShowProperty = false;
    }

    private void Load() {
        TextAsset dataFile = Resources.Load<TextAsset>("WordData");
        if (dataFile == null) {
            Debug.Log("Word Data File is not exist");
            Application.Quit();
            return;
        }
        Data = JsonUtility.FromJson<WordDataStruct>(dataFile.text);
        statusLog = string.Empty;
    }
}
#endif
