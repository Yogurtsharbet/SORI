using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtonController : MonoBehaviour {
    private Image[] selectImages;

    private void Awake() {
        selectImages = GetComponentsInChildren<Image>();
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
}
