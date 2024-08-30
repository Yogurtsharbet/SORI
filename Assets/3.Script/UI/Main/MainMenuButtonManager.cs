using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonManager : MonoBehaviour {
    private int selectMenuKey = 0;

    private Button[] menuButtons;
    private MainMenuButtonController[] buttonControllers;

    public void SetSelectMenuKey(int num) {
        selectMenuKey = num;
    }

    public int SelectMenuKey { get { return selectMenuKey; } }

    private void Awake() {
        menuButtons = GetComponentsInChildren<Button>();
        buttonControllers = new MainMenuButtonController[menuButtons.Length];
        for (int i = 0; i < menuButtons.Length; i++) {
            buttonControllers[i] = menuButtons[i].GetComponent<MainMenuButtonController>();
        }
    }

    private void Start() {
        MenuSelectCheck(selectMenuKey);
    }

    public void MenuSelectCheck(int key) {
        for (int i = 0; i < buttonControllers.Length; i++) {
            buttonControllers[i].DisActiveSelectImage();
        }
        buttonControllers[key].ActiveSelectImage();
    }

    public void CloseMenuButtons() {
        for(int i = 0; i < menuButtons.Length; i++) {
            menuButtons[i].gameObject.SetActive(false);
        }
    }
}
