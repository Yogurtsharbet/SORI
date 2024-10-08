using System;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionController : MonoBehaviour {
    private OptionDataManager optionDataManager;
    private CanvasScaler[] canvasScaler;

    private Text text;
    private int resolKey = 0;

    private void Awake() {
        optionDataManager = FindObjectOfType<OptionDataManager>();
        canvasScaler = GetComponentsInParent<CanvasScaler>();
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text txt in texts) {
            if (txt.name.Equals("ChangerText")) {
                text = txt;
            }
        }
    }

    private void OnEnable() {
        resolKey = (int)optionDataManager.OptionData.ResolutionType;
        CheckResolution();
    }

    private void CheckResolution() {
        (int, int) thisResolution = Resolutions.GetResolutionByNum(resolKey);
        text.text = $" {thisResolution.Item1} x {thisResolution.Item2}";
        changeCanvasResolution(thisResolution);
        int screenMode = optionDataManager.OptionData.ScreenMode;
        if (screenMode == 0) {
            Screen.SetResolution(thisResolution.Item1, thisResolution.Item2, FullScreenMode.ExclusiveFullScreen);
        }
        else if (screenMode == 1) {
            Screen.SetResolution(thisResolution.Item1, thisResolution.Item2, FullScreenMode.FullScreenWindow);
        }
        else {
            Screen.SetResolution(thisResolution.Item1, thisResolution.Item2, FullScreenMode.Windowed);
        }
    }

    public void ChangeRight() {
        if (resolKey == Enum.GetNames(typeof(ResolutionType)).Length - 1) {
            resolKey = 0;
        }
        else {
            resolKey = resolKey + 1;
        }
        ResolutionType type = Resolutions.GetResolutionTypeByNum(resolKey);
        optionDataManager.OptionData.SetResolutionType(type);
        CheckResolution();
    }

    public void ChangeLeft() {
        if (resolKey == 0) {
            resolKey = Enum.GetNames(typeof(ResolutionType)).Length - 1;
        }
        else {
            resolKey = resolKey - 1;
        }
        ResolutionType type = Resolutions.GetResolutionTypeByNum(resolKey);
        optionDataManager.OptionData.SetResolutionType(type);
        CheckResolution();
    }

    private void changeCanvasResolution((int, int) changeResolution) {
        foreach (CanvasScaler scaler in canvasScaler) {
            if (scaler.uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize) {
                scaler.referenceResolution = new Vector2(changeResolution.Item1, changeResolution.Item2);
            }
        }
    }
}
