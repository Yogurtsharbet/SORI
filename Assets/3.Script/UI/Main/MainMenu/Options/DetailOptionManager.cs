using UnityEngine;
using UnityEngine.UI;

public class DetailOptionManager : MonoBehaviour {
    private Button[] optionButtons;

    private GraphicOptionsManager graphicOptions;
    private AudioOptionsManager audioOptions;
    private ControlOptionsManager controlOptions;

    private DetailOptionController[] detailOptions;

    private int selectOptionKey = 0;

    public void SetSelectOptionKey(int num) {
        selectOptionKey = num;
    }

    public int SelectOptionKey { get { return selectOptionKey; } }

    private void Awake() {
        optionButtons = GetComponentsInChildren<Button>();
        detailOptions = GetComponentsInChildren<DetailOptionController>();

        graphicOptions = FindObjectOfType<GraphicOptionsManager>();
        audioOptions = FindObjectOfType<AudioOptionsManager>();
        controlOptions = FindObjectOfType<ControlOptionsManager>();
    }

    private void Start() {
        for (int i = 0; i < 2; i++) {
            if (i == 0)
                detailOptions[i].ActiveButtonHover(true);
            else
                detailOptions[i].ActiveButtonHover(false);
        }
    }

    private void OnDisEnable() {
        selectOptionKey = 0;
    }

    public void CheckSelectOption(int key) {
        for (int i = 0; i < 2; i++) {
            if (i == key)
                detailOptions[i].ActiveButtonHover(true);
            else
                detailOptions[i].ActiveButtonHover(false);
        }
    }

    public void OpenOptions() {
        if (selectOptionKey == 0) {
            graphicOptions.OpenGraphicOption();
        }
        else if (selectOptionKey == 1) {
            audioOptions.OpenAudioOption();
        }
        else {
            controlOptions.OpenControlOption();
        }
    }

    public void CloseOptions() {
        graphicOptions.CloseGraphicOption();
        audioOptions.CloseAudioOption();
        controlOptions.CloseControlOption();
    }
}
