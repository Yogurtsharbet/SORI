using UnityEngine;
using UnityEngine.UI;

public class VSyncController : MonoBehaviour {
    private MainDetailManager mainDetailManager;
    bool isActive = false;
    private Text text;

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
        CheckVsync();
    }

    private void CheckVsync() {
        isActive = mainDetailManager.OptionData.ActiveVSync;
        if (isActive) {
            text.text = "켜기";

            QualitySettings.vSyncCount = 1;
        }
        else {
            text.text = "끄기";
            QualitySettings.vSyncCount = 0;
        }
    }

    public void ChangeActive() {
        isActive = !isActive;
        mainDetailManager.OptionData.SetActiveVSync(isActive);
        CheckVsync();
    }
}
