using UnityEngine;
using UnityEngine.UI;

//메인 - 옵션 - 인터페이스 설정 컨트롤러
public class InterfaceController : MonoBehaviour {
    private Slider slider;
    private CanvasScaler[] canvasScaler;

    private void Awake() {
        slider = GetComponentInChildren<Slider>();
        canvasScaler = GetComponentsInParent<CanvasScaler>();
    }

    private void Start() {
        slider.onValueChanged.AddListener(delegate {
            SetUIScale(slider.value);
        });
        slider.value = canvasScaler[0].scaleFactor;

    }

    public void SetUIScale(float scaleFactor) {
        if (canvasScaler[0].uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize) {
            for (int i = 0; i < canvasScaler.Length; i++) {
                canvasScaler[i].matchWidthOrHeight = scaleFactor;
            }
        }
    }
}
