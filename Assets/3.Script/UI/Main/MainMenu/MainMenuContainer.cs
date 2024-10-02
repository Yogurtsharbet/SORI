using System.Collections;
using UnityEngine;

public class MainMenuContainer : MonoBehaviour {
    private MainManager mainManager;
    private MainMenuManager mainMenuManager;
    private MainDetailManager detailManager;

    private Animator menuAni;

    private void Awake() {
        mainManager = FindObjectOfType<MainManager>();
        mainMenuManager = FindObjectOfType<MainMenuManager>();
        detailManager = FindObjectOfType<MainDetailManager>();

        menuAni = GetComponentInChildren<Animator>();
    }

    private void Start() {
        detailManager.gameObject.SetActive(false);
    }

    #region This GameObejct Open/Close
    public void OpenMainMenu() {
        gameObject.SetActive(true);
    }

    public void CloseMainMenu() {
        gameObject.SetActive(false);
    }
    #endregion

    public void OpenMainDetail() {
        menuAni.SetBool("Open", true);
        StartCoroutine(DelayedCo(true));
    }

    public void CloseMainDetail() {
        menuAni.SetBool("Open", false);
        StartCoroutine(DelayedCo(false));
    }

    IEnumerator DelayedCo(bool isDetailOpen) {
        yield return new WaitForSeconds(0.35f);
        if (isDetailOpen) {
            mainMenuManager.CloseMainMenu();
            detailManager.gameObject.SetActive(true);
            detailManager.OpenDetailData(mainMenuManager.SelectMenuKey);
        }
        else {
            detailManager.gameObject.SetActive(false);
            mainMenuManager.OpenMainMenu();
        }
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void StartNewGame() {
        mainManager.OpenLoad();
    }
}
