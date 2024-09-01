using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainManager : MonoBehaviour {
    private bool isOpenMenu = false;
    private bool isDetailMenu = false;
    private bool isOpenOption = false;

    private Canvas mainCanvas;
    private Canvas mainMenuCanvas;

    private DefaultInputActions action;
    private Animator menuAni;

    private MainMenuButtonManager mainMenuButtonManager;
    private MainDetailManager mainDetailManager;
    private DetailOptionManager detailOptionManager;

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
        mainDetailManager = FindObjectOfType<MainDetailManager>();
        detailOptionManager = FindObjectOfType<DetailOptionManager>();
    }

    private void Start() {
        openMain();
    }

    private void OnEnable() {
        action.UI.Enable();
        //input system settings
        action.UI.Submit.performed += value => onSubmit();
        action.UI.Navigate.performed += value => onMove(value.ReadValue<Vector2>());
        action.UI.Cancel.performed += value => onCancel();
        mainMenuButtonManager.SetSelectMenuKey(0);
    }

    private void OnDisable() {
        action.UI.Disable();
    }

    #region Active ����
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

    #region Ű���� ��� ����
    private void onSubmit() {
        if (!isOpenMenu) {
            openMainMenu();
            return;
        }

        //���� -> ���� �� �޴�
        if (isOpenMenu && !isDetailMenu) {
            menuAni.SetBool("Open", true);
            mainMenuButtonManager.CloseMenuButtons();
            isDetailMenu = true;
        }

        //���� �� �޴� -> �޴� ����
        if (isDetailMenu && !isOpenOption) {
            if (mainMenuButtonManager.SelectMenuKey == 0 || mainMenuButtonManager.SelectMenuKey == 2) {
                StartCoroutine(DelayedCo(true));
            }
            else if (mainMenuButtonManager.SelectMenuKey == 3) {
                Application.Quit();
            }
            else {
                //TODO: �� ���� ���� �߰�
            }
        }

        //�ɼ� �޴�
        if (isOpenOption) {
            detailOptionManager.OpenOptions();
        }
    }

    private void onMove(Vector2 pos) {
        if (!isOpenMenu) {
            return;
        }

        //���� �޴�
        if (isOpenMenu && !isDetailMenu) {
            mainMenuMove(pos);
        }

        //�ɼ� �޴�
        if (isOpenOption) {
            detailOptionManager.CloseOptions();
            optionMenuMove(pos);
        }
    }

    private void onCancel() {
        if (isDetailMenu && !isOpenOption) {
            mainDetailManager.gameObject.SetActive(false);
            menuAni.SetBool("Open", false);
            StartCoroutine(DelayedCo(false));
            isDetailMenu = false;
        }

        //�ɼ� �޴�
        if (isOpenOption) {

            isOpenOption = false;
        }
    }
    #endregion

    #region �޴� �̵� ����
    //���� �޴� �̵� (�ҷ�����, ������, �ɼ�, ����)
    private void mainMenuMove(Vector2 pos) {
        int key = mainMenuButtonManager.SelectMenuKey;
        if ((pos.y < 0 && pos.x == 0) || (pos.x > 0 && pos.y == 0)) {
            if (key == 3) {
                mainMenuButtonManager.SetSelectMenuKey(0);
            }
            else {
                mainMenuButtonManager.SetSelectMenuKey(key + 1);
            }
        }
        else if ((pos.y > 0 && pos.x == 0) || (pos.x < 0 && pos.y == 0)) {
            if (key == 0) {
                mainMenuButtonManager.SetSelectMenuKey(3);
            }
            else {
                mainMenuButtonManager.SetSelectMenuKey(key - 1);
            }
        }
        mainMenuButtonManager.MenuSelectCheck(key);
    }

    //�ɼ� �޴� �̵� (�׷���, �����, ��Ʈ��)
    private void optionMenuMove(Vector2 pos) {
        int key = detailOptionManager.SelectOptionKey;
        if ((pos.y < 0 && pos.x == 0) || (pos.x > 0 && pos.y == 0)) {
            if (key == 2) {
                detailOptionManager.SetSelectOptionKey(0);
            }
            else {
                detailOptionManager.SetSelectOptionKey(key + 1);
            }
        }
        else if ((pos.y > 0 && pos.x == 0) || (pos.x < 0 && pos.y == 0)) {
            if (key == 0) {
                detailOptionManager.SetSelectOptionKey(2);
            }
            else {
                detailOptionManager.SetSelectOptionKey(key - 1);
            }
        }
        detailOptionManager.CheckSelectOption(key);
    }
    #endregion

    //������ �ڷ�ƾ
    IEnumerator DelayedCo(bool isDetailOpen) {
        yield return new WaitForSeconds(0.35f);
        if (isDetailOpen) {
            mainDetailManager.gameObject.SetActive(true);
            mainDetailManager.OpenDetailData(mainMenuButtonManager.SelectMenuKey);
        }
        else {
            mainMenuButtonManager.OpenMenuButtons();
        }
    }

}
