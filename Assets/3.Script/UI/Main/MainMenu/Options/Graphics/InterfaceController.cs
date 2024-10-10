using UnityEngine;
using UnityEngine.UI;

//메인 - 옵션 - 인터페이스 설정 컨트롤러
public class InterfaceController : MonoBehaviour {
    private OptionDataManager optionDataManager;
    private Slider slider;
    private CanvasScaler[] canvasScaler;

    private void Awake() {
        optionDataManager = FindObjectOfType<OptionDataManager>();
        slider = GetComponentInChildren<Slider>();
        canvasScaler = GetComponentsInParent<CanvasScaler>();
    }

    private void Start() {
        CheckUIScale(optionDataManager.OptionData.UIScale);
        slider.value = canvasScaler[0].scaleFactor;

        slider.onValueChanged.AddListener(delegate {
            SetUIScale(slider.value);
        });

    }

    private void SetUIScale(float scaleFactor) {
        optionDataManager.OptionData.SetUIScale(scaleFactor);
        CheckUIScale(scaleFactor);
    }

    private void CheckUIScale(float scaleFactor) {
        if (canvasScaler[0].uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize) {
            for (int i = 0; i < canvasScaler.Length; i++) {
                canvasScaler[i].scaleFactor = scaleFactor;
            }
        }
    }
}
