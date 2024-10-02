using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOptionController : MonoBehaviour, IPointerEnterHandler {
    private GameOptionContainer gameOptionContainer;
    private Image[] images;

    private void Awake() {
        gameOptionContainer = FindObjectOfType<GameOptionContainer>();
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
            gameOptionContainer.CheckSelectOption(0);
        }
        else if (name.Contains("Audio")) {
            gameOptionContainer.CheckSelectOption(1);
        }
        else if (name.Contains("Control")) {
            gameOptionContainer.CheckSelectOption(2);
        }
    }
}
