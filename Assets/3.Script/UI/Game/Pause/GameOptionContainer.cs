using UnityEngine;
using UnityEngine.InputSystem;

public class GameOptionContainer : MonoBehaviour {
    private PauseContainer pauseContainer;
    private GameOptionController[] optionControllers;
    private GameObject[] options = new GameObject[3];

    private DefaultInputActions action;

    private int selectOptionIndex = 0;
    private bool isDetail = false;

    private void Awake() {
        pauseContainer = FindObjectOfType<PauseContainer>();
        optionControllers = GetComponentsInChildren<GameOptionController>();
        for (int i = 0; i < 3; i++) {
            options[i] = transform.GetChild(i).gameObject;
        }
        action = new DefaultInputActions();
    }

    private void OnEnable() {
        selectOptionIndex = 0;
        action.UI.Enable();
        action.UI.Submit.performed += value => onSubmit();
        action.UI.Navigate.performed += value => onMove(value.ReadValue<Vector2>());
        action.UI.Cancel.performed += value => OnCancel();
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        action.UI.Disable();
    }

    public void OpenOption() {
        gameObject.SetActive(true);
        OpenMainOption();
    }

    public void OpenMainOption() {
        for (int i = 0; i < 3; i++) {
            if (i == 0) {
                options[i].SetActive(true);
            }
            else {
                options[i].SetActive(false);
            }
        }
    }

    private void onSubmit() {
        if (!isDetail) {
            OpenDetailOption();
        }
    }

    private void onMove(Vector2 pos) {
        if (!isDetail) {
            menuMove(pos);
        }
    }

    public void OnCancel() {
        if (!isDetail) {
            pauseContainer.OpenPause();
        }
    }

    private void menuMove(Vector2 pos) {
        int key = selectOptionIndex;
        if ((pos.y < 0 && pos.x == 0) || (pos.x > 0 && pos.y == 0)) {
            if (key == 1) {
                key = 0;
            }
            else {
                key = key + 1;
            }
        }
        else if ((pos.y > 0 && pos.x == 0) || (pos.x < 0 && pos.y == 0)) {
            if (key == 0) {
                key = 1;
            }
            else {
                key = key - 1;
            }
        }
        selectOptionIndex = key;
    }

    public void CheckSelectOption(int key) {
        selectOptionIndex = key;
        for (int i = 0; i < optionControllers.Length; i++) {
            optionControllers[i].ActiveButtonHover(false);
        }
        optionControllers[key].ActiveButtonHover(true);
    }

    public void OpenDetailOption() {
        for (int i = 0; i < options.Length; i++) {
            if ((i - 1) == selectOptionIndex) {
                options[i].gameObject.SetActive(true);
            }
            else {
                options[i].gameObject.SetActive(false);
            }
        }
    }
}
