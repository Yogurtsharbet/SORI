using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainManager : MonoBehaviour {
    private bool isOpenMenu = false;
    private bool isDetailMenu = false;

    private Canvas mainCanvas;
    private Canvas mainMenuCanvas;

    private DefaultInputActions action;
    private Animator menuAni;

    private MainMenuButtonManager mainMenuButtonManager;

    private void Awake() {
        Canvas[] canvases = GetComponentsInChildren<Canvas>();
        foreach (Canvas c in canvases) {
            if (c.name.Equals("MainCanvas")) {
                Debug.Log(c.name);
                mainCanvas = c;
            }
            else {
                mainMenuCanvas = c;
            }
        }
        action = new DefaultInputActions();

        menuAni = mainMenuCanvas.GetComponentInChildren<Animator>();
        mainMenuButtonManager = FindObjectOfType<MainMenuButtonManager>();
    }

    private void Start() {
        openMain();
    }

    private void OnEnable() {
        action.UI.Enable();
        action.UI.Submit.performed += value => onSubmit();
        action.UI.Navigate.performed += value => onMove(value.ReadValue<Vector2>());
        mainMenuButtonManager.SetSelectMenuKey(0);
    }

    private void OnDisable() {
        action.UI.Disable();
    }

    #region CanvasOpenSettings
    private void openMainMenu() {
        mainCanvas.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);
        isOpenMenu = true;
    }

    private void openMain() {
        mainCanvas.gameObject.SetActive(true);
        mainMenuCanvas.gameObject.SetActive(false);
        isOpenMenu = false;
    }
    #endregion

    private void onSubmit() {
        if (!isOpenMenu) {
            openMainMenu();
            return;
        }

        //메인 메뉴 → 상세
        if (isOpenMenu && !isDetailMenu) {
            menuAni.SetBool("Open", true);
            mainMenuButtonManager.CloseMenuButtons();
        }

        if (isDetailMenu) {

        }
    }

    private void onMove(Vector2 pos) {
        if (!isOpenMenu) {
            return;
        }

        if(isOpenMenu && !isDetailMenu) {
            mainMenuMove(pos);
        }
    }

    private void mainMenuMove(Vector2 pos) {
        int key = mainMenuButtonManager.SelectMenuKey;
        if ((pos.y < 0 && pos.x == 0) || (pos.x > 0 && pos.y == 0)) {
            if (key == 3) {
                mainMenuButtonManager.SetSelectMenuKey(0);
            }
            else {
                mainMenuButtonManager.SetSelectMenuKey(key+1);
            }
        }
        else if ((pos.y > 0 && pos.x == 0) || (pos.x < 0 && pos.y == 0)) {
            if (key == 0) {
                mainMenuButtonManager.SetSelectMenuKey(3);
            }
            else {
                mainMenuButtonManager.SetSelectMenuKey(key-1);
            }
        }
        mainMenuButtonManager.MenuSelectCheck(key);
    }
   
}
