using UnityEngine;
using UnityEngine.UI;

//메인 - 옵션 - 윈도우모드 설정 컨트롤러
public class WindowController : MonoBehaviour {
    private OptionDataManager optionDataManager;
    private Text text;
    private int _screenMode = 0;

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
        _screenMode = optionDataManager.OptionData.ScreenMode;
        CheckWindowMode();
    }

    private void CheckWindowMode() {
        (int, int) thisResolution = Resolutions.GetResolutionByNum((int)optionDataManager.OptionData.ResolutionType);
        if (_screenMode == 0) {
            text.text = "전체화면";
            Screen.SetResolution(thisResolution.Item1, thisResolution.Item2, FullScreenMode.ExclusiveFullScreen);
        }
        else if (_screenMode == 1) {
            text.text = "전체 창모드";
            Screen.SetResolution(thisResolution.Item1, thisResolution.Item2, FullScreenMode.FullScreenWindow);
        }
        else {
            text.text = "창모드";
            Screen.SetResolution(thisResolution.Item1, thisResolution.Item2, FullScreenMode.Windowed);
        }
    }

    public void ChangeLeft() {
        if (_screenMode == 0) {
            _screenMode = 2;
        }
        else {
            _screenMode--;
        }
        optionDataManager.OptionData.SetIsFullScreen(_screenMode);
        CheckWindowMode();
    }

    public void ChangeRight() {
        if (_screenMode == 2) {
            _screenMode = 0;
        }
        else {
            _screenMode++;
        }
        optionDataManager.OptionData.SetIsFullScreen(_screenMode);
        CheckWindowMode();
    }
}
