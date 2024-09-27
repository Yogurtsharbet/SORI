using UnityEngine;
using UnityEngine.InputSystem;

public class MainLogoController : MonoBehaviour {
    private MainManager mainManager;
    private DefaultInputActions action;

    private void Awake() {
        action = new DefaultInputActions();
        mainManager = FindObjectOfType<MainManager>();
    }

    private void OnEnable() {
        action.UI.Enable();
        action.UI.Click.performed += value => onClick();
    }

    private void OnDisable() {
        action.UI.Disable();
    }

    private void onClick() {
        mainManager.OpenMainMenu();
    }
}
