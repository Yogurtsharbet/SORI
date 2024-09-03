using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VSyncController : MonoBehaviour {
    bool isActive = false;
    private Text text;

    private void Awake() {
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text txt in texts) {
            if (txt.name.Equals("ChangerText")) {
                text = txt;
            }
        }
    }

    public void ChangeActive() {
        isActive = !isActive;

        if (isActive) {
            text.text = "ÄÑ±â";
            QualitySettings.vSyncCount = 1;
        }
        else {
            text.text = "²ô±â";
            QualitySettings.vSyncCount = 0;
        }
    }
}
