using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonController : MonoBehaviour, IPointerEnterHandler {
    private Image[] selectImages;
    private PauseContainer pauseContainer;

    private void Awake() {
        selectImages = GetComponentsInChildren<Image>();
        pauseContainer = FindObjectOfType<PauseContainer>();
        DisActiveSelectImage();
    }

    public void ActiveSelectImage() {
        foreach (Image select in selectImages) {
            select.enabled = true;
        }
    }

    public void DisActiveSelectImage() {
        foreach (Image select in selectImages) {
            select.enabled = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (name.Equals("Return")) {
            pauseContainer.SetHoverButton(0);
        }
        else if (name.Equals("Option")) {
            pauseContainer.SetHoverButton(1);
        }
        else if (name.Equals("Main")) {
            pauseContainer.SetHoverButton(2);
        }
        else {
            pauseContainer.SetHoverButton(3);
        }
    }
}
