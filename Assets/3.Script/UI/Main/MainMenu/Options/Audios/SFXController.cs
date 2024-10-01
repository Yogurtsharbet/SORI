using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXController : MonoBehaviour {
    private MainDetailManager mainDetailManager;
    private Slider slider;
    public AudioMixer audioMixer;

    private void Awake() {
        mainDetailManager = FindObjectOfType<MainDetailManager>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start() {
        slider.value = mainDetailManager.OptionData.SfxAudionValue;
        checkVolume(mainDetailManager.OptionData.SfxAudionValue);

        slider.onValueChanged.AddListener(delegate {
            setVolume(slider.value);
        });
    }

    private void checkVolume(float volume) {
        if (volume <= 0) {
            audioMixer.SetFloat("SFX", -80);
        }
        else {
            audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        }
    }

    private void setVolume(float volume) {
        if (volume <= 0) {
            mainDetailManager.OptionData.SetSfxAudionValue(0);
        }
        else {
            mainDetailManager.OptionData.SetSfxAudionValue(volume);
        }
        checkVolume(volume);
    }
}
