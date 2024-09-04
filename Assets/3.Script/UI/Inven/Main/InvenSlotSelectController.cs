using UnityEngine;
using UnityEngine.UI;

public class InvenSlotSelectController : MonoBehaviour {
    private Image[] selectImages;

    private void Awake() {
        selectImages = GetComponentsInChildren<Image>();
    }

    private void Start() {
        DisEnable();
    }

    public void Enable() {
        selectImages[0].enabled = true;
        selectImages[1].enabled = true;
    }

    public void DisEnable() {
        selectImages[0].enabled = false;
        selectImages[1].enabled = false;
    }
}
