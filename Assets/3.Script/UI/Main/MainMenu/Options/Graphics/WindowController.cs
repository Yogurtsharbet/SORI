using UnityEngine;
using UnityEngine.UI;

//메인 - 옵션 - 윈도우모드 설정 컨트롤러
public class WindowController : MonoBehaviour {
    private MainDetailManager mainDetailManager;
    private Text text;
    private bool isFullscreen = false;

    private void Awake() {
        mainDetailManager = FindObjectOfType<MainDetailManager>();
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text txt in texts) {
            if (txt.name.Equals("ChangerText")) {
                text = txt;
            }
        }
    }

    private void OnEnable() {
        isFullscreen = mainDetailManager.OptionData.IsFullScreen;
        CheckWindowMode();
    }

    private void CheckWindowMode() {
        (int, int) thisResolution = Resolutions.GetResolutionByNum((int)mainDetailManager.OptionData.ResolutionType);
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
            mainDetailManager.OptionData.SetIsFullScreen(isFullscreen);
        }
        else {
            isFullscreen = false;
            mainDetailManager.OptionData.SetIsFullScreen(isFullscreen);
        }
    }
}
