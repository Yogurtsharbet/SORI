using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuManager : MonoBehaviour {
    private DefaultInputActions action;
    private MainManager mainManager;
    private MainMenuContainer mainMenuContainer;

    private MainMenuButtonController[] buttonControllers;
    private int selectMenuKey = 0;
    public int SelectMenuKey => selectMenuKey;

    private void Awake() {
        action = new DefaultInputActions();
        mainManager = FindObjectOfType<MainManager>();
        mainMenuContainer = FindObjectOfType<MainMenuContainer>();
        buttonControllers = GetComponentsInChildren<MainMenuButtonController>();
    }

    private void OnEnable() {
        action.UI.Enable();
        action.UI.Submit.performed += value => onSubmit();
        action.UI.Navigate.performed += value => onMove(value.ReadValue<Vector2>());
        action.UI.Cancel.performed += value => onCancel();
    }

    private void Start() {
        MenuSelectCheck(selectMenuKey);
        buttonControllers[0].gameObject.SetActive(false);
    }

    private void OnDisable() {
        action.UI.Disable();
    }

    private void onSubmit() {
        switch (selectMenuKey) {
            case 0:
            case 2:
                mainMenuContainer.OpenMainDetail();
                break;
            case 1:
                mainMenuContainer.StartNewGame();
                break;
            case 3:
                mainMenuContainer.QuitGame();
                break;
        }
    }

    private void onMove(Vector2 pos) {
        mainMenuMove(pos);
    }

    private void onCancel() {
        mainManager.OpenMainTitle();
    }

    //메인 메뉴 이동 (불러오기, 새게임, 옵션, 종료)
    private void mainMenuMove(Vector2 pos) {
        int key = selectMenuKey;
        if ((pos.y < 0 && pos.x == 0) || (pos.x > 0 && pos.y == 0)) {
            if (key == 3) {
                selectMenuKey = 1;
            }
            else {
                selectMenuKey = key + 1;
            }
        }
        else if ((pos.y > 0 && pos.x == 0) || (pos.x < 0 && pos.y == 0)) {
            if (key == 1) {
                selectMenuKey = 3;
            }
            else {
                selectMenuKey = key - 1;
            }
        }
        MenuSelectCheck(selectMenuKey);
    }

    public void SetHoverButton(int num) {
        MenuSelectCheck(num);
        selectMenuKey = num;
    }

    //선택 체크
    public void MenuSelectCheck(int key) {
        for (int i = 0; i < buttonControllers.Length; i++) {
            buttonControllers[i].DisActiveSelectImage();
        }
        buttonControllers[key].ActiveSelectImage();
    }

    public void CloseMainMenu() {
        gameObject.SetActive(false);
    }

    public void OpenMainMenu() {
        gameObject.SetActive(true);
    }
}
