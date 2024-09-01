using System;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionController : MonoBehaviour {
    private Text text;
    private int resolKey = 0;

    private void Awake() {
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text txt in texts) {
            if (txt.name == "ChangerText") {
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
        if(resolKey == 0) {
            resolKey = Enum.GetNames(typeof(ResolutionType)).Length - 1;
        }
        else {
            resolKey = resolKey - 1;
        }
        (int, int) thisResolution = Resolutions.GetResolutionByNum(resolKey);
        text.text = $" {thisResolution.Item1} x {thisResolution.Item2}";
    }
}
