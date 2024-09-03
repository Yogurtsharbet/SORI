using System;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionController : MonoBehaviour {
    private CanvasScaler[] canvasScaler;

    private Text text;
    private int resolKey = 0;


    private void Awake() {
        canvasScaler = GetComponentsInParent<CanvasScaler>();
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text txt in texts) {
            if (txt.name.Equals("ChangerText")) {
                text = txt;
            }
        }
    }

    public void ChangeRight() {
        if (resolKey == Enum.GetNames(typeof(ResolutionType)).Length - 1) {
            resolKey = 0;
        }
        else {
            resolKey = resolKey + 1;
        }
        (int, int) thisResolution = Resolutions.GetResolutionByNum(resolKey);
        text.text = $" {thisResolution.Item1} x {thisResolution.Item2}";
    }

    public void ChangeLeft() {
        if (resolKey == 0) {
            resolKey = Enum.GetNames(typeof(ResolutionType)).Length - 1;
        }
        else {
            resolKey = resolKey - 1;
        }
        (int, int) thisResolution = Resolutions.GetResolutionByNum(resolKey);
        text.text = $" {thisResolution.Item1} x {thisResolution.Item2}";

#if UNITY_EDITOR
        UnityEditor.SceneView.lastActiveSceneView.position = new Rect(0, 0, thisResolution.Item1, thisResolution.Item2);
#endif
        changeCanvasResolution(thisResolution);
    }

    private void changeCanvasResolution((int, int) changeResolution) {
        if (canvasScaler[0].uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize) {
            for (int i = 0; i < canvasScaler.Length; i++) {
                canvasScaler[i].referenceResolution = new Vector2(changeResolution.Item1, changeResolution.Item2);
            }
        }
    }
}
