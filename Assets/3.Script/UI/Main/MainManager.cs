using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// [UI] 메인 - 메인 화면 매니저
public class MainManager : MonoBehaviour {
    private MainTitle mainTitle;
    private MainMenuContainer mainMenu;
    private MainLoading mainLoading;

    private void Awake() {
        mainTitle = FindObjectOfType<MainTitle>();
        mainMenu = FindObjectOfType<MainMenuContainer>();
        mainLoading = FindObjectOfType<MainLoading>();
    }

    private void Start() {
        OpenMainTitle();
    }

    public void OpenMainTitle() {
        mainTitle.OpenMainTitle();
        mainMenu.CloseMainMenu();
        mainLoading.gameObject.SetActive(false);
    }

    public void OpenMainMenu() {
        mainTitle.CloseMainTitle();
        mainMenu.OpenMainMenu();
        mainLoading.gameObject.SetActive(false);
    }

    public void OpenLoad() {
        mainTitle.CloseMainTitle();
        mainMenu.CloseMainMenu();
        mainLoading.gameObject.SetActive(true);
    }
}
