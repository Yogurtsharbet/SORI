using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//메인 - 옵션 - 윈도우모드 설정 컨트롤러
public class WindowController : MonoBehaviour {
    private Text text;
    private bool isFullscreen = false;

    private void Awake() {
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text txt in texts) {
            if (txt.name == "ChangerText") {
                text = txt;
            }
        }
    }

    public void ChangeWindowMode() {
        if (isFullscreen) {
            isFullscreen = true;
            text.text = "전체화면";
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
        else {
            isFullscreen = false;
            text.text = "창모드";
            //TODO: 해상도 설정한것으로 가져오기
            Screen.SetResolution(1280, 720, false);
        }
    }
}
