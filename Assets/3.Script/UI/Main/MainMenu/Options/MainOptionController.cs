using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainOptionController : MonoBehaviour, IPointerEnterHandler {
    private MainDetailManager mainDetailManager;
    private Image[] images;

    private void Awake() {
        mainDetailManager = FindObjectOfType<MainDetailManager>();
        images = GetComponentsInChildren<Image>();
    }

    public void ActiveButtonHover(bool yn) {
        if (yn) {
            foreach (Image image in images) {
                image.enabled = true;
            }
        }
        else {
            foreach (Image image in images) {
                image.enabled = false;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (name.Contains("Graphic")) {
            mainDetailManager.CheckSelectOption(0);
        }
        else if (name.Contains("Audio")) {
            mainDetailManager.CheckSelectOption(1);
        }
        else if (name.Contains("Control")) {
            mainDetailManager.CheckSelectOption(2);
        }
    }
}
