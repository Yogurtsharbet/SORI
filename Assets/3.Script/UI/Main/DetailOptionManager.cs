using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailOptionManager : MonoBehaviour {
    private Button[] optionButtons;
    private Image[] graphicSelects;
    private Image[] audioSelects;
    private Image[] controlSelects;

    private GraphicOptionsManager graphicOptions;
    private AudioOptionsManager audioOptions;
    private ControlOptionsManager controlOptions;

    private int selectOptionKey = 0;

    public void SetSelectOptionKey(int num) {
        selectOptionKey = num;
    }

    public int SelectOptionKey { get { return selectOptionKey; } }

    private void Awake() {
        optionButtons = GetComponentsInChildren<Button>();
        graphicSelects = optionButtons[0].GetComponentsInChildren<Image>();
        audioSelects = optionButtons[1].GetComponentsInChildren<Image>();
        controlSelects = optionButtons[2].GetComponentsInChildren<Image>();

        graphicOptions = FindObjectOfType<GraphicOptionsManager>();
        audioOptions = FindObjectOfType<AudioOptionsManager>();
        controlOptions = FindObjectOfType<ControlOptionsManager>();
    }

    private void Start() {
        gameObject.SetActive(false);
        for (int i = 0; i < 2; i++) {
            graphicSelects[i].enabled = false;
            audioSelects[i].enabled = false;
            controlSelects[i].enabled = false;
        }
    }

    private void OnEnable() {
        selectOptionKey = 0;
    }

    public void CheckSelectOption(int key) {
        if (key == 0) {
            for (int i = 0; i < 2; i++) {
                graphicSelects[i].enabled = true;
                audioSelects[i].enabled = false;
                controlSelects[i].enabled = false;
            }
        }
        else if (key == 1) {
            for (int i = 0; i < 2; i++) {
                graphicSelects[i].enabled = false;
                audioSelects[i].enabled = true;
                controlSelects[i].enabled = false;
            }
        }
        else {
            for (int i = 0; i < 2; i++) {
                graphicSelects[i].enabled = false;
                audioSelects[i].enabled = false;
                controlSelects[i].enabled = true;
            }
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
