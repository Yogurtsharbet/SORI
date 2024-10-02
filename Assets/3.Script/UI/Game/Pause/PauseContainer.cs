using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// [UI] 일시정지 - 일시정지 컨테이너 관리
public class PauseContainer : MonoBehaviour {
    [SerializeField] private GameObject bgPanel;
    private int index = 0;

    private Vector3 openPos = new Vector3(-246f, 27f, 0);
    private Vector3 closePos = new Vector3(-246f, -864f, 0);

    private Button returnButton;
    private Button optionButton;
    private Button mainButton;
    private Button exitButton;

    private DefaultInputActions action;
    private PauseButtonController[] pauseButtonController;

    private void Awake() {
        action = new DefaultInputActions();
        Button[] buttons = GetComponentsInChildren<Button>();
        pauseButtonController = new PauseButtonController[buttons.Length];
        int i = 0;
        foreach (Button btn in buttons) {
            if (btn.name.Equals("Return"))
                returnButton = btn;
            else if (btn.name.Equals("Option"))
                optionButton = btn;
            else if (btn.name.Equals("Main"))
                mainButton = btn;
            else
                exitButton = btn;
            pauseButtonController[i] = btn.GetComponent<PauseButtonController>();
            i++;
        }
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenPause() {
        gameObject.SetActive(true);
        bgPanel.SetActive(true);
        FunctionMove(gameObject.transform, openPos);
    }

    public void ClosePause() {
        gameObject.SetActive(false);
        FunctionMove(gameObject.transform, closePos);
        gameObject.SetActive(false);
    }

    private void OnEnable() {
        Time.timeScale = 0;
        action.UI.Enable();
        action.UI.Submit.performed += value => onSubmit();
        action.UI.Navigate.performed += value => onMove(value.ReadValue<Vector2>());
    }

    private void OnDisable() {
        action.UI.Disable();
        Time.timeScale = 1f;
    }

    private void onSubmit() {
        switch (index) {
            case 0:
                ClickReturn();
                break;
            case 1:
                OpenOption();
                break;
            case 2:
                GoMain();
                break;
            case 3:
                ClickExitButton();
                break;
        }
    }

    #region 메뉴 이동 및 선택 UI
    private void onMove(Vector2 pos) {
        menuMove(pos);
        MenuSelectCheck(index);
    }

    private void menuMove(Vector2 pos) {
        int key = index;
        if ((pos.y < 0 && pos.x == 0) || (pos.x > 0 && pos.y == 0)) {
            if (key == 3) {
                key = 0;
            }
            else {
                key = key + 1;
            }
        }
        else if ((pos.y > 0 && pos.x == 0) || (pos.x < 0 && pos.y == 0)) {
            if (key == 0) {
                key = 3;
            }
            else {
                key = key - 1;
            }
        }
        index = key;
    }

    public void SetHoverButton(int num) {
        MenuSelectCheck(num);
        index = num;
    }

    public void MenuSelectCheck(int key) {
        for (int i = 0; i < pauseButtonController.Length; i++) {
            if (i != key)
                pauseButtonController[i].DisActiveSelectImage();
        }
        pauseButtonController[key].ActiveSelectImage();
    }

    private void FunctionMove(Transform origin, Vector3 destiny) {
        origin.DOLocalMove(destiny, 1f, true).SetUpdate(true);
    }
    #endregion

    #region 기능
    public void ClickReturn() {
        gameObject.SetActive(false);
    }

    public void OpenOption() {

    }

    public void GoMain() {
        //TODO: 저장하고 메인 씬으로 이동
        SceneManager.LoadSceneAsync("Main");
    }

    public void ClickExitButton() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(); // 어플리케이션 종료
#endif
    }

    #endregion
}
