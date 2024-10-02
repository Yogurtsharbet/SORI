using UnityEngine;
using UnityEngine.UI;

//메인 - 옵션 - 윈도우모드 설정 컨트롤러
public class WindowController : MonoBehaviour {
    private OptionDataManager optionDataManager;
    private Text text;
    private bool isFullscreen = false;

    private void Awake() {
        optionDataManager = FindObjectOfType<OptionDataManager>();
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text txt in texts) {
            if (txt.name.Equals("ChangerText")) {
                text = txt;
            }
        }
    }

    private void OnEnable() {
        isFullscreen = optionDataManager.OptionData.IsFullScreen;
        CheckWindowMode();
    }

    private void CheckWindowMode() {
        (int, int) thisResolution = Resolutions.GetResolutionByNum((int)optionDataManager.OptionData.ResolutionType);
        if (isFullscreen) {
            text.text = "전체화면";
            Screen.SetResolution(thisResolution.Item1, thisResolution.Item2, true);
        }
        else {
            text.text = "창모드";
            Screen.SetResolution(thisResolution.Item1, thisResolution.Item2, false);
        }
    }

    public void ChangeWindowMode() {
        if (!isFullscreen) {
            isFullscreen = true;
            optionDataManager.OptionData.SetIsFullScreen(isFullscreen);
        }
        else {
            isFullscreen = false;
            optionDataManager.OptionData.SetIsFullScreen(isFullscreen);
        }
    }
}
