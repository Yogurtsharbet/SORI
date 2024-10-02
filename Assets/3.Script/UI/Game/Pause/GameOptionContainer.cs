using UnityEngine;
using UnityEngine.InputSystem;

public class GameOptionContainer : MonoBehaviour {
    private GameOptionController[] optionControllers;
    private GameObject[] options = new GameObject[4];

    private DefaultInputActions action;

    private int selectOptionIndex = 0;
    private bool isDetail = false;

    private void Awake() {
        optionControllers = GetComponentsInChildren<GameOptionController>();
        for (int i = 0; i < 4; i++) {
            options[i] = transform.GetChild(i).gameObject;
        }
        action = new DefaultInputActions();
    }

    private void OnEnable() {
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

    private void OpenMainOption() {
        for (int i = 0; i < 4; i++) {
            if (i == 0) {
                options[i].SetActive(true);
            }
            else {
                options[i].SetActive(false);
            }
        }
    }

    public void OpenDetailOption(int index) {
        for (int i = 0; i < 4; i++) {
            if (i == index) {
                options[i].SetActive(true);
            }
            else {
                options[i].SetActive(false);
            }
        }
    }

    private void onSubmit() {
    }

    private void onMove(Vector2 pos) {
    }

    public void OnCancel() {
    }

    public void CheckSelectOption(int key) {
        selectOptionIndex = key;
        for (int i = 0; i < optionControllers.Length; i++) {
            optionControllers[i].ActiveButtonHover(false);
        }
        optionControllers[key].ActiveButtonHover(true);
    }
}
