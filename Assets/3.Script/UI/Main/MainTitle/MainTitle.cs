using UnityEngine;
using UnityEngine.InputSystem;

public class MainTitle : MonoBehaviour {
    private MainManager mainManager;
    private DefaultInputActions action;

    private void Awake() {
        action = new DefaultInputActions();
        mainManager = FindObjectOfType<MainManager>();
    }

    private void OnEnable() {
        action.UI.Enable();
        action.UI.Submit.performed += value => onSubmit();
        action.UI.Click.performed += value => onClick();
    }

    private void OnDisable() {
        action.UI.Disable();
    }

    private void onSubmit() {
        mainManager.OpenMainMenu();
    }
    private void onClick() {
        mainManager.OpenMainMenu();
    }

    public void OpenMainTitle() {
        gameObject.SetActive(true);
    }

    public void CloseMainTitle() {
        gameObject.SetActive(false);
    }

}

