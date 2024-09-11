using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [UI] �Ͻ����� - �Ͻ����� �����̳� ����
public class PauseContainer : MonoBehaviour {
    private int index = 0;

    private Button returnButton;
    private Button optionButton;
    private Button mainButton;
    private Button exitButton;

    private void Awake() {
        Button[] buttons = GetComponentsInChildren<Button>();

        foreach(Button btn in buttons) {
            if (btn.name.Equals("Return"))
                returnButton = btn;
            else if (btn.name.Equals("Option"))
                optionButton = btn;
            else if (btn.name.Equals("Main"))
                mainButton = btn;
            else
                exitButton = btn;
        }
    }

    private void OnEnable() {
        Time.timeScale = 0;
    }

    private void OnDisable() {
        Time.timeScale = 1f;
    }

    public void ClickExitButton() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // ���ø����̼� ����
#endif
    }

    public void ClickReturn() {
        gameObject.SetActive(false);
    }
}
