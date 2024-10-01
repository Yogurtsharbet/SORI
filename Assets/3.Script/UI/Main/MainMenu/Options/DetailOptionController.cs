using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DetailOptionController : MonoBehaviour, IPointerEnterHandler {
    private DetailOptionManager detailOptionManager;
    private Image[] images;

    private void Awake() {
        detailOptionManager = FindObjectOfType<DetailOptionManager>();
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
            detailOptionManager.CheckSelectOption(0);
        }
        else if (name.Contains("Audio")) {
            detailOptionManager.CheckSelectOption(1);
        }
        else if (name.Contains("Control")) {
            detailOptionManager.CheckSelectOption(2);
        }
    }
}
