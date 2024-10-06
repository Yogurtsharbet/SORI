using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainDetailManager : MonoBehaviour {
    private Text title;

    private DefaultInputActions action;
    private LoadManager loadManager;
    private MainMenuContainer mainMenuContainer;

    private MainOptionController[] optionControllers;

    private GameObject[] optionObjects = new GameObject[4];

    private bool isOptionMenu = false;
    private bool isOptionDetail = false;

    private int selectOptionKey = 0;

    private void Awake() {
        title = GetComponentInChildren<Text>();

        for (int i = 2; i < 4 + 2; i++) {
            optionObjects[i - 2] = transform.GetChild(i).gameObject;
        }

        action = new DefaultInputActions();
        mainMenuContainer = FindObjectOfType<MainMenuContainer>();
        loadManager = FindObjectOfType<LoadManager>();

        optionControllers = GetComponentsInChildren<MainOptionController>();
    }

    private void OnEnable() {
        action.UI.Enable();
        action.UI.Submit.performed += value => onSubmit();
        action.UI.Navigate.performed += value => onMove(value.ReadValue<Vector2>());
        action.UI.Cancel.performed += value => OnCancel();
    }

    private void OnDisable() {
        action.UI.Disable();
    }

    public void OpenDetailData(int key) {
        if (key == 0) {  //불러오기
            title.text = "불러오기";
            isOptionMenu = false;
            OpenDetailLoad();
        }
        else {          //옵션
            title.text = "옵션";
            isOptionMenu = true;
            OpenOptions();
        }
    }

    private void onSubmit() {
        if (isOptionMenu && !isOptionDetail) {
            //옵션 메뉴
            OpenDetailOption();
        }
        else if (isOptionMenu && isOptionDetail) {
            //옵션 상세메뉴
            return;
        }
        else {
            //TODO: 세이브 파일 로드
        }
    }

    private void onMove(Vector2 pos) {
        if (isOptionMenu && !isOptionDetail) {
            //옵션 메뉴
            optionMenuMove(pos);
        }
        else if (isOptionMenu && isOptionDetail) {
            //옵션 상세메뉴
            return;
        }
        else {
            //세이브 파일 로드
            loadMenuMove(pos);
        }
    }

    public void OnCancel() {
        if (isOptionMenu && !isOptionDetail) {
            //옵션 메뉴
            mainMenuContainer.CloseMainDetail();
        }
        else if (isOptionMenu && isOptionDetail) {
            //옵션 상세 메뉴
            OpenOptions();
        }
        else {
            //세이브 파일 메뉴
            mainMenuContainer.CloseMainDetail();
        }
    }


    //옵션 메뉴 이동 (그래픽, 오디오, 컨트롤)
    private void optionMenuMove(Vector2 pos) {
        int key = selectOptionKey;
        if ((pos.y < 0 && pos.x == 0) || (pos.x > 0 && pos.y == 0)) {
            if (key == 2) {
                selectOptionKey = 0;
            }
            else {
                selectOptionKey = key + 1;
            }
        }
        else if ((pos.y > 0 && pos.x == 0) || (pos.x < 0 && pos.y == 0)) {
            if (key == 0) {
                selectOptionKey = 2;
            }
            else {
                selectOptionKey = key - 1;
            }
        }
        //selectOptionKey = key;
    }

    private void loadMenuMove(Vector2 pos) {
        int key = loadManager.SelectIndex;
        if ((pos.y < 0 && pos.x == 0) || (pos.x > 0 && pos.y == 0)) {
            if (key == 2) {
                loadManager.SetSelectIndex(0);
            }
            else {
                loadManager.SetSelectIndex(key + 1);
            }
        }
        else if ((pos.y > 0 && pos.x == 0) || (pos.x < 0 && pos.y == 0)) {
            if (key == 0) {
                loadManager.SetSelectIndex(2);
            }
            else {
                loadManager.SetSelectIndex(key - 1);
            }
        }
    }

    public void OpenDetailLoad() {
        loadManager.gameObject.SetActive(true);
        optionObjects[0].SetActive(false);
        CloseDetailOptions();
    }

    public void OpenOptions() {
        loadManager.gameObject.SetActive(false);
        optionObjects[0].SetActive(true);
        CloseDetailOptions();
        isOptionDetail = false;
    }

    public void OpenDetailOption() {
        isOptionDetail = true;
        loadManager.gameObject.SetActive(false);
        for (int i = 0; i < optionObjects.Length; i++) {
            if ((i - 1) == selectOptionKey) {
                optionObjects[i].SetActive(true);
            }
            else {
                optionObjects[i].SetActive(false);
            }
        }
    }

    private void CloseDetailOptions() {
        for (int i = 1; i < optionObjects.Length; i++) {
            optionObjects[i].SetActive(false);
        }
    }

    public void CheckSelectOption(int key) {
        selectOptionKey = key;
        for (int i = 0; i < optionControllers.Length; i++) {
            optionControllers[i].ActiveButtonHover(false);
        }
        optionControllers[key].ActiveButtonHover(true);
    }
}
