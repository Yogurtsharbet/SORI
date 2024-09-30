using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButtonController : MonoBehaviour, IPointerEnterHandler {
    private Image[] selectImages;
    private MainMenuManager mainMenuButtonManager;

    private void Awake() {
        selectImages = GetComponentsInChildren<Image>();
        mainMenuButtonManager = FindObjectOfType<MainMenuManager>();
        DisActiveSelectImage();
    }

    public void ActiveSelectImage() {
        foreach(Image select in selectImages) {
            select.enabled = true;
        }
    }

    public void DisActiveSelectImage() {
        foreach (Image select in selectImages) {
            select.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (name.Equals("LoadGame")) {
            mainMenuButtonManager.SetHoverButton(0);
        }
        else if (name.Equals("NewGame")) {
            mainMenuButtonManager.SetHoverButton(1);
        }
        else if (name.Equals("Option")) {
            mainMenuButtonManager.SetHoverButton(2);
        }
        else {
            mainMenuButtonManager.SetHoverButton(3);
        }
    }
}
