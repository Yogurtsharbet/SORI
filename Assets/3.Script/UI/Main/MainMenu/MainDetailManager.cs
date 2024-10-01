using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainDetailManager : MonoBehaviour {
    [SerializeField] private OptionData optionData;
    public OptionData OptionData => optionData;
    private Text title;

    private DefaultInputActions action;
    private DetailOptionManager detailOptionManager;
    private LoadManager loadManager;
    private MainMenuContainer mainMenuContainer;

    private bool isOptionMenu = false;
    private bool isOptionDetail = false;

    private void Awake() {
        title = GetComponentInChildren<Text>();

        action = new DefaultInputActions();
        detailOptionManager = FindObjectOfType<DetailOptionManager>();
        mainMenuContainer = FindObjectOfType<MainMenuContainer>();
        loadManager = FindObjectOfType<LoadManager>();
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

    private void onSubmit() {
        if (isOptionMenu && !isOptionDetail) {
            //옵션 메뉴
            isOptionDetail = true;
        }
        else if (isOptionMenu && isOptionDetail) {
            //옵션 상세메뉴

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

        }
        else {
            //세이브 파일 로드
            loadMenuMove(pos);
        }
    }

    public void OnCancel() {
        if (isOptionMenu && !isOptionDetail) {
            mainMenuContainer.CloseMainDetail();
        }
        else if (isOptionMenu && isOptionDetail) {

        }
        else {
            mainMenuContainer.CloseMainDetail();
        }
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
            OpenDetailOption();
        }
    }

    //옵션 메뉴 이동 (그래픽, 오디오, 컨트롤)
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
        //TODO: 선택한 인덱스 체크 추가하기
    }

    public void OpenDetailLoad() {
        loadManager.gameObject.SetActive(true);
        detailOptionManager.gameObject.SetActive(false);
    }

    public void OpenDetailOption() {
        loadManager.gameObject.SetActive(false);
        detailOptionManager.gameObject.SetActive(true);
    }

    public void CloseDetails() {
        loadManager.gameObject.SetActive(false);
        detailOptionManager.gameObject.SetActive(false);
    }
}
