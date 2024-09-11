using UnityEngine;
using UnityEngine.UI;

// [UI] 인벤토리 - 선택한 슬롯 컨트롤러
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
